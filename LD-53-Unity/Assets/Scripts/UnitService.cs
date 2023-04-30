using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class UnitService : IService, IInject, IStart, IUpdate
{
    public class UnitState
    {
        public uint AttackDamage;
        public uint Health;
    }
    
    private AssetsCollection _assetsCollection;
    private readonly List<KeyValuePair<ObjectGridElement, UnitState>> _statesByObjects = new();
    private UIService _uiService;
    private GameFlowService _flowService;
    private GridService _gridService;
    private EnemyService _enemyService;

    void IInject.Inject()
    {
        _assetsCollection = Services.Get<AssetsCollection>();
        _flowService = Services.Get<GameFlowService>();
        _gridService = Services.Get<GridService>();
        _uiService = Services.Get<UIService>();
        _enemyService = Services.Get<EnemyService>();
    }

    public void CreateUnitState(ObjectGridElement element, uint? health = null, uint? attackDamage = null)
    {
        var unitState = new UnitState();
        _statesByObjects.Add(new KeyValuePair<ObjectGridElement, UnitState>(element, unitState));
        
        unitState.Health = health.GetValueOrDefault(_assetsCollection.GetElementHealth(element));
        unitState.AttackDamage = attackDamage.GetValueOrDefault(_assetsCollection. GetElementAttackDamage(element));

        _uiService.AddHealthView(element, unitState.Health);
    }

    public void ChangeUnitHealth(ObjectGridElement element, int delta)
    {
        if (element == null)
        {
            throw new NullReferenceException(
                $"{nameof(UnitService)} нельзя выполнить {nameof(ChangeUnitHealth)}, {nameof(element)} == null");
        }

        var (unitElement, unitState) = _statesByObjects.FirstOrDefault(s => s.Key == element);
        var state = _statesByObjects.FirstOrDefault(s => s.Key == element);

        if (state.Value is null)
        {
            throw new ArgumentOutOfRangeException(
                $"{nameof(UnitService)} {nameof(_statesByObjects)} не содержит стейт для элемента {element.name}" +
                $" типа {element.Type} айди {element.GetInstanceID()}");
        }

        unitState.Health += (uint)Math.Max(-unitState.Health, delta);

        _uiService.UpdatedHealthView(element, unitState.Health);

        if (unitState.Health == 0)
        {
            if (element.Type == ObjectType.Player)
            {
                _flowService.LoseGame(_assetsCollection.GameConfig.EndHealthLoseReasonText);
            }
            else
            {
                RemoveUnit(unitElement);
            }
        }
    }

    private void RemoveUnit(ObjectGridElement element)
    {
        for (int i = 0; i < _statesByObjects.Count; i++)
        {
            if (_statesByObjects[i].Key == element)
            {
                _uiService.RemoveHealthView(element);
                _statesByObjects.RemoveAt(i);
                _gridService.RemoveElement(element);
                if (element is EnemyView view) _enemyService.RemoveEnemy(view);
                return;
            }
        }
    }

    void IStart.GameStart()
    {
    }
    
    void IUpdate.GameUpdate(float delta)
    {
    }

    public void CreateUnitStates(ObjectGridElement[,] objects)
    {
        for (var i = 0; i < objects.GetLength(0); i++)
        {
            for (var j = 0; j < objects.GetLength(1); j++)
            {
                var element = objects[i, j];
                if (element is null)
                {
                    continue;
                }

                if (element.Type is ObjectType.EndLevel or ObjectType.Obstacle or ObjectType.Encounter)
                {
                    continue;
                }
                
                CreateUnitState(element);
            }
        }
    }

    public IEnumerator AttackObject(ObjectGridElement target, uint? damage = null)
    {
        damage = damage.GetValueOrDefault(_statesByObjects.FirstOrDefault(s => s.Key.Type == ObjectType.Player).Value
            .AttackDamage);
        ChangeUnitHealth(target, -(int)damage);
        yield break;
    }
}
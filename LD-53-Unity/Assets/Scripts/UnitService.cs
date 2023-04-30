using System;
using System.Collections.Generic;
using System.Linq;

public class UnitService : IService, IInject, IStart, IUpdate
{
    public class UnitState
    {
        public uint Health;
    }
    
    private AssetsCollection _assetsCollection;
    private readonly List<KeyValuePair<ObjectGridElement, UnitState>> _statesByObjects = new();
    private UIService _uiService;

    void IInject.Inject()
    {
        _assetsCollection = Services.Get<AssetsCollection>();
        _uiService = Services.Get<UIService>();
    }

    public void CreateUnitState(ObjectGridElement element, uint? health = null)
    {
        var unitState = new UnitState();
        _statesByObjects.Add(new KeyValuePair<ObjectGridElement, UnitState>(element, unitState));
        
        unitState.Health = health.GetValueOrDefault(_assetsCollection.GetElementHealth(element));
        
        _uiService.AddHealthView(element, unitState.Health);
    }

    public void ChangeUnitHealth(ObjectGridElement element, int delta)
    {
        if (element == null)
        {
            throw new NullReferenceException(
                $"{nameof(UnitService)} нельзя выполнить {nameof(ChangeUnitHealth)}, {nameof(element)} == null");
        }

        var unitState = new UnitState();
        var state = _statesByObjects.FirstOrDefault(s => s.Key == element);

        if (state.Value is null)
        {
            throw new ArgumentOutOfRangeException(
                $"{nameof(UnitService)} {nameof(_statesByObjects)} не содержит стейт для элемента {element.name}" +
                $" типа {element.Type} айди {element.GetInstanceID()}");
        }

        unitState.Health += (uint)Math.Max(-unitState.Health, delta);

        _uiService.UpdatedHealthView(element, unitState.Health);
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

                if (element.Type is ObjectType.EndLevel or ObjectType.Obstacle)
                {
                    continue;
                }
                
                CreateUnitState(element);
            }
        }
    }
}
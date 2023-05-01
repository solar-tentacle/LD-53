using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AttackAction : CardAction
{
    protected PlayerService _playerService;
    private PlayerView _playerView;
    protected GridService _gridService;
    private readonly List<(GroundGridElement, ObjectGridElement)> _elements = new();
    protected UnitService _unitService;
    protected ObjectGridElement _selectedElement;
    private AssetsCollection _assetCollection;
    private ObjectType[] _enemyTypes;

    public override void Init()
    {
        _playerService = Services.Get<PlayerService>();
        _playerView = _playerService.PlayerView;
        _gridService = Services.Get<GridService>();
        _unitService = Services.Get<UnitService>();
        _assetCollection = Services.Get<AssetsCollection>();
        _enemyTypes = _assetCollection.GetObjectsWithHealth();
    }

    public override IEnumerator Select()
    {
        Vector2Int pos = _gridService.GetObjectPosition(_playerView);
        FillElements(pos);

        foreach (var element in _elements)
        {
            var (ground, gridObject) = element;
            yield return ground.EnableHighlight(HighlightType.Attack);
        }
    }

    public override IEnumerator Deselect()
    {
        foreach (var element in _elements)
        {
            var (ground, gridObject) = element;
            yield return ground.DisableHighlight(HighlightType.Attack);
        }
    }

    public override bool CanExecute()
    {
        _selectedElement = null;
        if (_gridService.TryGetMouseGridPos(out Vector2Int pos))
        {
            _selectedElement = _gridService.GetObjectView(pos);
            if (_selectedElement == null)
            {
                return false;
            }

            return _elements.Exists(element => element.Item2 == _selectedElement);
        }

        return false;
    }

    public override IEnumerator Execute()
    {
        yield return _unitService.AttackObject(_playerView, _selectedElement);
    }

    private void FillElements(Vector2Int pos)
    {
        var range = 1;
        _elements.Clear();
        _gridService.GetSurroundingElements(pos.x, pos.y, range, _elements, _enemyTypes);
    }
}

[Serializable]
public class LifeStealAttackAction : AttackAction
{
    [SerializeField] private uint _damage = 1;
    [SerializeField] private uint _heal = 1;

    public override IEnumerator Execute()
    {
        yield return _unitService.ChangeUnitHealth(_playerService.PlayerView, (int) _heal);
        yield return _unitService.AttackObject(_playerService.PlayerView, _selectedElement, _damage);
    }
}
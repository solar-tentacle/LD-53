using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AttackAction : CardAction
{
    private PlayerService _playerService;
    private PlayerView _playerView;
    private GridService _gridService;
    private readonly List<(GroundGridElement, ObjectGridElement)> _elements = new();
    private UnitService _unitService;
    private ObjectGridElement _selectedElement;

    public override void Init()
    {
        _playerService = Services.Get<PlayerService>();
        _playerView = _playerService.PlayerView;
        _gridService = Services.Get<GridService>();
        _unitService = Services.Get<UnitService>();
    }

    public override IEnumerator Select()
    {
        Vector2Int pos = _gridService.GetObjectPosition(_playerView);
        FillElements(pos);

        foreach (var element in _elements)
        {
            var (ground, gridObject) = element;
            yield return ground.EnableMoveHighlight();
            //yield return gridObject.EnableHighlight();
        }
    }

    public override IEnumerator Deselect()
    {
        foreach (var element in _elements)
        {
            var (ground, gridObject) = element;
            yield return ground.DisableMoveHighlight();
            //yield return gridObject.EnableHighlight();
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
        yield return _unitService.AttackObject(_selectedElement);
    }

    private void FillElements(Vector2Int pos)
    {
        var range = 1;
        _elements.Clear();
        _gridService.GetSurroundingElements(pos.x, pos.y, range, _elements);
    }
}
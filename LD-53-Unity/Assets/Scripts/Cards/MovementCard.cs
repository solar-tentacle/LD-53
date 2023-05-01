using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MovementCard : CardAction
{
    [SerializeField] private List<Vector2Int> _directions;
    private PlayerService _playerService;
    private PlayerView _playerView;
    private GridService _gridService;
    private List<GroundGridElement> _elements;

    public int DirectionsCount => _directions.Count;

    public override void Init()
    {
        _playerService = Services.Get<PlayerService>();
        _playerView = _playerService.PlayerView;
        _gridService = Services.Get<GridService>();
    }

    public override IEnumerator Select()
    {
        Vector2Int pos = _gridService.GetObjectPosition(_playerView);
        _elements = GetElements(pos);

        foreach (GroundGridElement element in _elements)
        {
            yield return element.EnableHighlight(HighlightType.Move);
        }
    }

    public override IEnumerator Deselect()
    {
        foreach (GroundGridElement element in _elements)
        {
            yield return element.DisableHighlight(HighlightType.Move);
        }
    }

    public override bool CanExecute()
    {
        if (_gridService.TryGetMouseGridPos(out Vector2Int pos))
        {
            GroundGridElement element = _gridService.GetGroundView(pos);
            return _elements.Contains(element);
        }

        return false;
    }

    public override IEnumerator Execute()
    {
        if (_gridService.TryGetMouseGridPos(out Vector2Int pos))
        {
            yield return _playerService.Move(pos);
        }
    }

    private List<GroundGridElement> GetElements(Vector2Int pos)
    {
        List<GroundGridElement> elements = new();
        foreach (Vector2Int direction in _directions)
        {
            _gridService.TryAddGroundElement(elements, pos + direction);
        }

        return elements;
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlusMovement : CardAction
{
    private PlayerService _playerService;
    private PlayerView _playerView;
    private GridService _gridService;
    private List<GroundGridElement> _elements;
    private Plane _plane = new(Vector3.up, 0);

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
            yield return element.EnableHighlight();
        }
    }

    public override IEnumerator Deselect()
    {
        foreach (GroundGridElement element in _elements)
        {
            yield return element.DisableHighlight();
        }
    }

    public override bool CanExecute()
    {
        if (TryGetMouseGridPos(out Vector2Int pos))
        {
            GroundGridElement element = _gridService.GetGroundView(pos);
            return _elements.Contains(element);
        }

        return false;
    }

    public override IEnumerator Execute()
    {
        if (TryGetMouseGridPos(out Vector2Int pos))
        {
            yield return _playerService.Move(pos);
        }
    }

    private List<GroundGridElement> GetElements(Vector2Int pos)
    {
        List<GroundGridElement> elements = new();
        TryAddElement(elements, pos + Vector2Int.up);
        TryAddElement(elements, pos + Vector2Int.down);
        TryAddElement(elements, pos + Vector2Int.left);
        TryAddElement(elements, pos + Vector2Int.right);

        return elements;
    }

    private void TryAddElement(List<GroundGridElement> buffer, Vector2Int pos)
    {
        if (_gridService.TryGetGroundView(pos, out GroundGridElement element))
        {
            buffer.Add(element);
        }
    }

    private bool TryGetMouseGridPos(out Vector2Int pos)
    {
        pos = Vector2Int.zero;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (_plane.Raycast(ray, out float distance))
        {
            Vector3 worldPosition = ray.GetPoint(distance);

            pos = _gridService.GetMatrixPosition(worldPosition);
            return true;
        }

        return false;
    }
}
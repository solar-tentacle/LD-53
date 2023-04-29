using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDropService : Service, IUpdate
{
    private ItemView _selectedItem;
    private RaycastHit[] _buffer;

    public void GameUpdate(float delta)
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (RayCast(out var col) && col.gameObject.TryGetComponent<ItemView>(out var item))
            {
                _selectedItem = item;
            }
        }

        if (_selectedItem is not null)
        {
            Cursor.visible = false;
            UpdateItemPosition();
        }

        if (Input.GetMouseButtonUp(0))
        {
            _selectedItem = null;
            Cursor.visible = true;
        }
    }

    private void UpdateItemPosition()
    {
        if (_selectedItem is null) return;
        var pos = new Vector3(Input.mousePosition.x, Input.mousePosition.y,
            Camera.main.WorldToScreenPoint(_selectedItem.transform.position).z);
        var worldPosition = Camera.main.ScreenToWorldPoint(pos);
        _selectedItem.transform.position = new Vector3(worldPosition.x, 1f, worldPosition.z);
    }

    private bool RayCast(out Collider col)
    {
        col = null;
        var screenMousePosFar = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            Camera.main.farClipPlane);

        var screenMousePosNear = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            Camera.main.nearClipPlane);

        var worldMousePosFar = Camera.main.ScreenToWorldPoint(screenMousePosFar);
        var worldMousePosNear = Camera.main.ScreenToWorldPoint(screenMousePosNear);
        var ray = new Ray(worldMousePosNear, worldMousePosFar - worldMousePosNear);
        var dragAndDroppable = Physics.RaycastNonAlloc(ray, _buffer, 100f, 6);

        if (dragAndDroppable > 0)
        {
            col = _buffer[0].collider;
        }

        return dragAndDroppable > 0;
    }
}
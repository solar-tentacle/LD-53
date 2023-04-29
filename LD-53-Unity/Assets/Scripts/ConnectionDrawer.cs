using UnityEngine;

public class ConnectionDrawer
{
    private readonly ConnectionView _prefab;
    private readonly GridService _gridService;

    public ConnectionDrawer(ConnectionView prefab)
    {
        _prefab = prefab;
        _gridService = Services.Get<GridService>();
    }

    public ConnectionView Draw(ItemContext a, ItemContext b)
    {
        Vector3 pos = (_gridService.GetTileWorldPoint(a.Point) + _gridService.GetTileWorldPoint(b.Point)) / 2;
        ConnectionView view = Object.Instantiate(_prefab);
        view.transform.position = pos;
        return view;
    }
}
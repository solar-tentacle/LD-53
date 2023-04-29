using System.Collections.Generic;
using UnityEngine;

public class ConnectionService : Service, IInject
{
    private List<ConnectionView> _views = new();
    private GridService _gridService;
    private ConnectionDrawer _drawer;

    public void Inject()
    {
        _gridService = Services.Get<GridService>();
        _drawer = new ConnectionDrawer(Services.Get<AssetsCollection>().ConnectionViewPrefab);
    }

    private void UpdateConnections()
    {
        ClearViews();

        List<ItemContext> contexts = _gridService.GetTileContexts();

        for (int i = 0; i < contexts.Count - 1; i++)
        {
            for (int j = i + 1; j < contexts.Count; j++)
            {
                if (CanConnectTiles(contexts[i], contexts[j]))
                {
                    _views.Add(_drawer.Draw(contexts[i], contexts[j]));
                }
            }
        }
    }

    private void ClearViews()
    {
        for (int i = 0; i < _views.Count; i++)
        {
            Destroy(_views[i].gameObject);
        }

        _views.Clear();
    }

    private static bool CanConnectTiles(ItemContext a, ItemContext b)
    {
        if (a.Type is ItemType.Sphere && b.Type is ItemType.Sphere)
        {
            return Vector2Int.Distance(a.Point, b.Point) < 2;
        }

        return false;
    }
}
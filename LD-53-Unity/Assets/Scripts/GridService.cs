using System.Collections.Generic;
using UnityEngine;

public class GridService : Service
{
    private ItemType[][] _gridTypes;
    private ItemView[][] _gridViews;

    public int SizeX { get; private set; }
    public int SizeY { get; private set; }
    public ItemType GetItemType(Vector2Int point) => _gridTypes[point.x][point.y];
    public ItemView GetView(Vector2Int point) => _gridViews[point.x][point.y];
    public Vector3 GetTileWorldPoint(Vector2Int point) => _gridViews[point.x][point.y].transform.position;

    public List<ItemContext> GetTileContexts()
    {
        List<ItemContext> contexts = new();

        for (int x = 0; x < SizeX; x++)
        {
            for (int y = 0; y < SizeY; y++)
            {
                contexts.Add(new ItemContext(_gridTypes[x][y], new Vector2Int(x, y)));
            }
        }

        return contexts;
    }
}
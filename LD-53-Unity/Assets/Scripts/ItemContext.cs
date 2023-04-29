using UnityEngine;

public readonly struct ItemContext
{
    public readonly TileType Type;
    public readonly Vector2Int Point;

    public ItemContext(TileType type, Vector2Int point)
    {
        Type = type;
        Point = point;
    }
}
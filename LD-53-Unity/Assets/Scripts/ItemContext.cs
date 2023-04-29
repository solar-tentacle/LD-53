using UnityEngine;

public readonly struct ItemContext
{
    public readonly ItemType Type;
    public readonly Vector2Int Point;

    public ItemContext(ItemType type, Vector2Int point)
    {
        Type = type;
        Point = point;
    }
}
using System;
using UnityEngine;

public class PortalGridElement : ObjectGridElement
{
    public PortalData Data;
}

[Serializable] public class PortalData
{
    public Vector2Int Direction = Vector2Int.up;
}
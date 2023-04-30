using System.Collections.Generic;
using UnityEngine;

public class GridService : IService, IStart
{
    private GroundGridElement[,] _ground;
    private ObjectGridElement[,] _objects;
    private ItemView[,] _gridViews;

    public int SizeX { get; private set; }
    public int SizeY { get; private set; }
    public GroundType GetGroundType(Vector2Int point) => _ground[point.x, point.y].Type;
    public ItemView GetView(Vector2Int point) => _gridViews[point.x, point.y];
    public Vector3 GetWorldPoint(Vector2Int point) => _ground[point.x, point.y].transform.position;

    void IStart.GameStart()
    {
        AssetsCollection assetsCollection = Services.Get<AssetsCollection>();
        GridBuilder builder = new(assetsCollection.LevelDataHolder.LevelData);
        builder.Build(out _ground, out _objects);
        UnitService unitService = Services.Get<UnitService>();
        unitService.CreateUnitStates(_objects);
    }
}
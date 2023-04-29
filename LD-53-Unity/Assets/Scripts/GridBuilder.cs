using UnityEngine;

public class GridBuilder
{
    private readonly LevelData _data;
    private readonly AssetsCollection _assetsCollection;

    public GridBuilder(LevelData data)
    {
        _data = data;
        _assetsCollection = Services.Get<AssetsCollection>();
    }

    public void Build()
    {
        Transform gridTransform = new GameObject("Grid").transform;
        for (int i = 0; i < _data.Tiles.GetLength(0); i++)
        {
            for (int j = 0; j < _data.Tiles.GetLength(1); j++)
            {
                GroundType type = _data.Tiles[i, j].TileView.GetComponent<GroundGridElement>().Type;
                GroundGridElement element = Object.Instantiate(_assetsCollection.GetGround(type), gridTransform);
                element.transform.position = new Vector3(i, 0, j);
            }
        }
    }
}
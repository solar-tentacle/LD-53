using UnityEngine;

public class GridBuilder
{
    private readonly LevelData _data;
    private readonly AssetsCollection _assetsCollection;

    private Transform _gridTransform;

    public GridBuilder(LevelData data)
    {
        _data = data;
        _assetsCollection = Services.Get<AssetsCollection>();
    }

    public GroundGridElement[,] Build()
    {
        _gridTransform = new GameObject("Grid").transform;
        return BuildGround();
    }

    private GroundGridElement[,] BuildGround()
    {
        GroundGridElement[,] ground = new GroundGridElement[_data.BackTiles.GetLength(0), _data.BackTiles.GetLength(1)];
        
        for (int i = 0; i < _data.BackTiles.GetLength(0); i++)
        {
            for (int j = 0; j < _data.BackTiles.GetLength(1); j++)
            {
                GroundType type = _data.BackTiles[i, j].TileView.GetComponent<GroundGridElement>().Type;
                GroundGridElement element = Object.Instantiate(_assetsCollection.GetGround(type), _gridTransform);
                element.transform.position = new Vector3(i, 0, j);
                ground[i, j] = element;
            }
        }

        return ground;
    }
}
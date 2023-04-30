using UnityEngine;

public class GridService : IService, IStart
{
    private GroundGridElement[,] _ground;
    private ObjectGridElement[,] _objects;
    private ItemView[,] _gridViews;

    void IStart.GameStart()
    {
        AssetsCollection assetsCollection = Services.Get<AssetsCollection>();
        GridBuilder builder = new(assetsCollection.LevelDataHolder.LevelData);
        builder.Build(out _ground, out _objects);
    }

    public int SizeX { get; private set; }
    public int SizeY { get; private set; }
    public GroundType GetGroundType(Vector2Int point) => _ground[point.x, point.y].Type;
    public ItemView GetView(Vector2Int point) => _gridViews[point.x, point.y];
    public Vector3 GetWorldPoint(Vector2Int point) => _ground[point.x, point.y].transform.position;

    public Vector2Int GetMatrixPosition(Vector3 pos)
    {
        float max = float.MaxValue;
        Vector2Int res = Vector2Int.zero;
        
        for (int i = 0; i < _ground.GetLength(0); i++)
        {
            for (int j = 0; j < _ground.GetLength(1); j++)
            {
                float distance = Vector3.Distance(_ground[i,j].transform.position, pos);
                if (distance < max)
                {
                    max = distance;
                    res = new Vector2Int(i, j);
                }
            }
        }

        return res;
    }

    public Vector2Int GetObjectPosition(ObjectGridElement element)
    {
        for (int i = 0; i < _objects.GetLength(0); i++)
        {
            for (int j = 0; j < _objects.GetLength(1); j++)
            {
                if (_objects[i, j] == element) return new Vector2Int(i, j);
            }
        }
        
        return Vector2Int.zero;
    }

    public PlayerView GetPlayerView()
    {
        foreach (ObjectGridElement element in _objects)
        {
            if (element is PlayerView view) return view;
        }

        return null;
    }

    public void Move(ObjectGridElement element, Vector2Int pos)
    {
        Vector2Int oldPos = GetObjectPosition(element);
        _objects[oldPos.x, oldPos.y] = null;
        _objects[pos.x, pos.y] = element;
    }
}
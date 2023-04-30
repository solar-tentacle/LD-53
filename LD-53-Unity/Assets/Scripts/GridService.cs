using UnityEngine;

public class GridService : IService, IStart
{
    private GroundGridElement[,] _ground;
    private ObjectGridElement[,] _objects;

    void IStart.GameStart()
    {
        AssetsCollection assetsCollection = Services.Get<AssetsCollection>();
        BuildLevel(assetsCollection.GetLevelData(GameFlowService.LevelIndex));
    }

    public void BuildLevel(LevelData levelData)
    {
        GridBuilder builder = new(levelData);
        builder.Build(out _ground, out _objects);
    }

    public GroundGridElement GetGroundView(Vector2Int point) => _ground[point.x, point.y];

    public bool TryGetGroundView(Vector2Int point, out GroundGridElement element)
    {
        element = null;
        if (IsInBounds(point))
        {
            element = GetGroundView(point);
            return true;
        }

        return false;
    }

    public Vector3 GetWorldPoint(Vector2Int point) => _ground[point.x, point.y].transform.position;

    public Vector2Int GetMatrixPosition(Vector3 pos)
    {
        float max = float.MaxValue;
        Vector2Int res = Vector2Int.zero;

        for (int i = 0; i < _ground.GetLength(0); i++)
        {
            for (int j = 0; j < _ground.GetLength(1); j++)
            {
                float distance = Vector3.Distance(_ground[i, j].transform.position, pos);
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

    public bool IsInBounds(Vector2Int pos)
    {
        if (pos.x < 0 || pos.x > _ground.GetLength(0)) return false;
        if (pos.y < 0 || pos.y > _ground.GetLength(1)) return false;
        return true;
    }
}
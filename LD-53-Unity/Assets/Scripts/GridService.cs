using System.Collections.Generic;
using System.Linq;
using Sirenix.Utilities;
using UnityEngine;

public class GridService : IService, IStart
{
    private GroundGridElement[,] _ground;
    private ObjectGridElement[,] _objects;
    private Plane _plane = new(Vector3.up, 0);

    void IStart.GameStart()
    {
        AssetsCollection assetsCollection = Services.Get<AssetsCollection>();
        BuildLevel(assetsCollection.GetLevelData(GameFlowService.LevelIndex));

        UnitService unitService = Services.Get<UnitService>();
        unitService.CreateUnitStates(_objects);
    }

    public void BuildLevel(LevelData levelData)
    {
        GridBuilder builder = new(levelData);
        builder.Build(out _ground, out _objects);
    }

    public GroundGridElement GetGroundView(Vector2Int point) => _ground[point.x, point.y];
    public ObjectGridElement GetObjectView(Vector2Int point) => _objects[point.x, point.y];

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

    public Vector2Int GetGroundPosition(GroundGridElement element)
    {
        for (int i = 0; i < _ground.GetLength(0); i++)
        {
            for (int j = 0; j < _ground.GetLength(1); j++)
            {
                if (_ground[i, j] == element) return new Vector2Int(i, j);
            }
        }

        return Vector2Int.zero;
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

    public void TryAddGroundElement(List<GroundGridElement> buffer, Vector2Int pos)
    {
        if (TryGetGroundView(pos, out GroundGridElement element))
        {
            if (element.Type is GroundType.Water) return;

            if (_objects[pos.x, pos.y] != null &&
                _objects[pos.x, pos.y].Type is not ObjectType.Player or ObjectType.EndLevel) return;

                buffer.Add(element);
        }
    }

    public void GetSurroundingElements(int rowIndex, int colIndex, int range,
        List<(GroundGridElement, ObjectGridElement)> buffer, params ObjectType[] objectTypes)
    {
        for (int row = rowIndex - range; row <= rowIndex + range; row++)
        {
            for (int col = colIndex - range; col <= colIndex + range; col++)
            {
                if (row == rowIndex && col == colIndex) {
                    continue;
                }

                if (row >= 0 && row < _ground.GetLength(0) && col >= 0 && col < _ground.GetLength(1))
                {
                    var objectGridElement = _objects[row, col];

                    if (!objectTypes.IsNullOrEmpty() &&
                        (objectGridElement == null || !objectTypes.Contains(objectGridElement.Type)))
                    {
                        continue;
                    }

                    buffer.Add((_ground[row, col], objectGridElement));
                }
            }
        }
    }

    public bool TryGetMouseGridPos(out Vector2Int pos)
    {
        pos = Vector2Int.zero;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (_plane.Raycast(ray, out float distance))
        {
            Vector3 worldPosition = ray.GetPoint(distance);

            pos = GetMatrixPosition(worldPosition);
            return true;
        }

        return false;
    }

    public void RemoveElement(ObjectGridElement element)
    {
        var position = GetObjectPosition(element);
        _objects[position.x, position.y] = null;
        GameObject.Destroy(element.gameObject);
    }
    
    public List<EnemyView> GetEnemies()
    {
        List<EnemyView> res = new();
        foreach (ObjectGridElement element in _objects)
        {
            if (element is EnemyView view)
            {
                res.Add(view);
            }
        }

        return res;
    }
}
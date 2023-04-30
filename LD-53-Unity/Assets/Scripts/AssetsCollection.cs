using System;
using System.Linq;
using Game.Level;
using UnityEngine;

public class AssetsCollection : Service
{
    public LevelDataHolder[] Levels;
    public ConnectionView ConnectionViewPrefab;
    public PlayerView PlayerView;
    [SerializeField] private GroundGridElement[] _grounds;
    [SerializeField] private ObjectGridElement[] _objects;
    [SerializeField] private ObjectStats[] _objectsStats;

    public GameConfig GameConfig;

    public GroundGridElement GetGround(GroundType type) => _grounds.FirstOrDefault(e => e.Type == type);
    public ObjectGridElement GetObject(ObjectType type) => _objects.FirstOrDefault(e => e.Type == type);
    public ObjectStats GetObjectStats(ObjectType type)
    {
        var result = _objectsStats.FirstOrDefault(e => e.Type == type);
        if (result is null)
        {
            Debug.LogError($"в конфигах статов объектов {nameof(AssetsCollection)} -> {nameof(_objectsStats)} нет объекта с типом {type}");
        }
        return result;
    }


    public uint GetElementHealth(ObjectGridElement element)
    {
        return GetElementHealth(element.Type);
    }
    
    public uint GetElementHealth(ObjectType type)
    {
        var objectStats = GetObjectStats(type);
        return objectStats?.Health ?? 1;
    }
    
    [Serializable] public class ObjectStats
    {
        public ObjectType Type;
        public uint Health = 1;
    }

    public LevelData GetLevelData(uint index)
    {
        index = (uint)Mathf.Repeat(index, Levels.Length);
        return Levels[index].LevelData;
    }
}
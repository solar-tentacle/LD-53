using System;
using System.Collections.Generic;
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
    public ObjectGridElement GetObject(ObjectType type)
    {
        var result = _objects.FirstOrDefault(e => e.Type == type);
        if (result is null)
        {
            Debug.LogError($"в конфигах {nameof(AssetsCollection)} -> {nameof(_objects)} нет объекта с типом {type}");
        }
        return result;
    }

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
    
    public uint GetElementAttackDamage(ObjectGridElement element)
    {
        return GetElementAttackDamage(element.Type);
    }
    
    public uint GetElementHealth(ObjectType type)
    {
        var objectStats = GetObjectStats(type);
        return objectStats?.Health ?? 1;
    }
    
    public uint GetElementAttackDamage(ObjectType type)
    {
        var objectStats = GetObjectStats(type);
        return objectStats?.AttackDamage ?? 1;
    }
    
    [Serializable] public class ObjectStats
    {
        public ObjectType Type;
        public uint Health = 1;
        public uint AttackDamage = 1;
    }

    public LevelData GetLevelData(uint index)
    {
        index = (uint)Mathf.Repeat(index, Levels.Length);
        return Levels[index].LevelData;
    }

    public ObjectType[] GetObjectsWithHealth()
    {
        return _objects
            .Where(o => _objectsStats.Any(s => s.Type != ObjectType.Player && s.Type == o.Type && s.Health > 0))
            .Select(o => o.Type).ToArray();
    }

    public bool IsLastLevel(uint levelIndex)
    {
        return levelIndex == Levels.Length - 1;
    }
}
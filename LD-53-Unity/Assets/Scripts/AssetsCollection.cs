using System.Linq;
using Game.Level;
using UnityEngine;

public class AssetsCollection : Service
{
    public LevelDataHolder LevelDataHolder;
    public ConnectionView ConnectionViewPrefab;
    public PlayerView PlayerView;
    [SerializeField] private GroundGridElement[] _grounds;
    [SerializeField] private ObjectGridElement[] _objects;

    public GameConfig GameConfig;

    public GroundGridElement GetGround(GroundType type) => _grounds.FirstOrDefault(e => e.Type == type);
    public ObjectGridElement GetObject(ObjectType type) => _objects.FirstOrDefault(e => e.Type == type);
}
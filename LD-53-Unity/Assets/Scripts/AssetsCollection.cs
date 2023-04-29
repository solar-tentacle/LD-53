using System.Linq;
using Game.Level;

public class AssetsCollection : Service
{
    public LevelDataHolder LevelDataHolder;
    public ConnectionView ConnectionViewPrefab;
    public PlayerView PlayerView;
    public GroundGridElement[] _grounds;

    public GroundGridElement GetGround(GroundType type) => _grounds.FirstOrDefault(e => e.Type == type);
}
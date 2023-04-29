using UnityEngine;

public class PlayerService : Service, IInject
{
    private PlayerView _playerView;
    
    public void Inject()
    {
        _playerView = Instantiate(Services.Get<AssetsCollection>().PlayerView);
    }
}
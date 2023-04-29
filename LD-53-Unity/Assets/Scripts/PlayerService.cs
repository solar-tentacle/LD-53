using System.Collections;
using UnityEngine;

public class PlayerService : Service, IInject
{
    private PlayerView _playerView;
    
    public void Inject()
    {
        _playerView = Instantiate(Services.Get<AssetsCollection>().PlayerView);
        
    }

    public void Move(Vector3 movePosition)
    {
        StartCoroutine(MoveCrt(movePosition));
    }

    public IEnumerator MoveCrt(Vector3 movePosition)
    {
        yield return _playerView.Movement.Move(movePosition);
    }
}
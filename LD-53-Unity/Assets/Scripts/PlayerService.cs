using System.Collections;
using UnityEngine;

public class PlayerService : Service, IInject, IStart
{
    private PlayerView _playerView;
    private AssetsCollection _assetsCollection;
    private GridService _gridService;

    public void Inject()
    {
        _assetsCollection = Services.Get<AssetsCollection>();
        _gridService = Services.Get<GridService>();
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

    public void GameStart()
    {
        _playerView = Instantiate(_assetsCollection.PlayerView);
        Vector3 pos = _gridService.GetWorldPoint(Vector2Int.zero);
        pos.y = 1;
        _playerView.transform.position = pos;
    }
}
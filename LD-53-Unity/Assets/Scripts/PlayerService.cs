using System.Collections;
using UnityEngine;

public class PlayerService : Service, IInject, IStart
{
    private static readonly int IdleTrigger = Animator.StringToHash("Idle");
    private static readonly int RunTrigger = Animator.StringToHash("Run");

    private PlayerView _playerView;
    private AssetsCollection _assetsCollection;
    private GridService _gridService;

    void IInject.Inject()
    {
        _assetsCollection = Services.Get<AssetsCollection>();
        _gridService = Services.Get<GridService>();
    }

    void IStart.GameStart()
    {
        _playerView = CreatePlayerView();
        _playerView.Animator.SetTrigger(IdleTrigger);
    }

    public void Move(Vector3 movePosition)
    {
        StartCoroutine(MoveCrt(movePosition));
    }

    public IEnumerator MoveCrt(Vector3 movePosition)
    {
        yield return _playerView.Movement.Move(movePosition, OnStartMove);

        _playerView.Animator.SetTrigger(IdleTrigger);
    }

    private void OnStartMove()
    {
        _playerView.Animator.SetTrigger(RunTrigger);
    }

    private PlayerView CreatePlayerView()
    {
        PlayerView view = Instantiate(_assetsCollection.PlayerView);
        Vector3 pos = _gridService.GetWorldPoint(Vector2Int.zero);
        pos.y = 1;
        view.transform.position = pos;

        return view;
    }
}
using System.Collections;
using UnityEngine;

public class PlayerService : Service, IInject
{
    private static readonly int IdleTrigger = Animator.StringToHash("Idle");
    private static readonly int RunTrigger = Animator.StringToHash("Run");
    
    private PlayerView _playerView;

    public void Inject()
    {
        _playerView = Instantiate(Services.Get<AssetsCollection>().PlayerView);
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
}
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class PlayerMovement
{
    private static readonly int IdleTrigger = Animator.StringToHash("Idle");
    private static readonly int RunTrigger = Animator.StringToHash("Run");
    
    private const float RotationTime = 0.5f;
    private const float MoveTimePerCell = 1f;

    private readonly PlayerView _view;
    private readonly GridService _gridService;

    public PlayerMovement(PlayerView view)
    {
        _view = view;
        _gridService = Services.Get<GridService>();
    }

    public void Init()
    {
        _view.Animator.SetTrigger(IdleTrigger);
    }

    public IEnumerator Move(Vector2Int pos)
    {
        _gridService.Move(_view, pos);
        Vector3 newPosition = _gridService.GetWorldPoint(pos);
        newPosition.y = _view.transform.position.y;
        
        Vector3 direction = (newPosition - _view.transform.position);
        direction.y = 0;
        direction.Normalize();
        Quaternion newRotation = Quaternion.LookRotation(direction);
        
        yield return _view.transform.DORotateQuaternion(newRotation, RotationTime).WaitForCompletion();
        
        _view.Animator.SetTrigger(RunTrigger);
        
        yield return _view.transform.DOMove(newPosition, MoveTimePerCell).WaitForCompletion();
        
        _view.Animator.SetTrigger(IdleTrigger);
    }
}
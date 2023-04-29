using System.Collections;
using DG.Tweening;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _rotationTime = 0.5f;
    [SerializeField] private float _moveTimePerCell = 1f;
    
    public IEnumerator Move(Vector3 movePosition)
    {
        var direction = movePosition - transform.position;
        var newRotation = Quaternion.FromToRotation(transform.forward, direction);
        
        yield return transform.DORotate(newRotation.eulerAngles, _rotationTime).WaitForCompletion();
        yield return transform.DOMove(movePosition, _moveTimePerCell).WaitForCompletion();
    }
}
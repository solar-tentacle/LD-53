using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _rotationTime = 0.5f;
    [SerializeField] private float _moveTimePerCell = 1f;
    
    public IEnumerator Move(Vector3 movePosition, Action onStartMove)
    {
        var direction = (movePosition - transform.position).normalized;
        var deltaRotation = Quaternion.FromToRotation(transform.forward, direction);
        var newRotation = transform.rotation * deltaRotation;
        
        yield return transform.DORotateQuaternion(newRotation, _rotationTime).WaitForCompletion();
        
        onStartMove?.Invoke();
        
        yield return transform.DOMove(movePosition, _moveTimePerCell).WaitForCompletion();
    }
}
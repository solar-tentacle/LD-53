using UnityEngine;

public class PlayerView : ObjectGridElement
{
    [SerializeField] private Animator _animator;
    public Animator Animator => _animator;
}
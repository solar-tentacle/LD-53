using UnityEngine;

public class PlayerView : ObjectGridElement
{
    [SerializeField] private PlayerMovement _movement;
    public PlayerMovement Movement => _movement;

    [SerializeField] private Animator _animator;
    public Animator Animator => _animator;
}
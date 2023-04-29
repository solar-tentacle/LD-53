using UnityEngine;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private PlayerMovement _movement;
    public PlayerMovement Movement => _movement;
}
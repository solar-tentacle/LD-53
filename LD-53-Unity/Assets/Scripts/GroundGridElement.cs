using UnityEngine;

public class GroundGridElement : MonoBehaviour
{
    [SerializeField] private GroundType _type;

    public GroundType Type => _type;
}
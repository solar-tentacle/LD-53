using UnityEngine;

public class GroundGridElement : GridElement
{
    [SerializeField] private GroundType _type;

    public GroundType Type => _type;
}
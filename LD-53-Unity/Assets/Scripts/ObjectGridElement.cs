using UnityEngine;

public class ObjectGridElement : GridElement
{
    [SerializeField] private ObjectType _type;
    
    public ObjectType Type => _type;
}
using System.Collections;
using UnityEngine;

public class GroundGridElement : GridElement
{
    [SerializeField] private GroundType _type;
    [SerializeField] private GameObject _highlight;

    public GroundType Type => _type;

    public IEnumerator EnableHighlight()
    {
        _highlight.SetActive(true);
        yield return null;
    }
    
    public IEnumerator DisableHighlight()
    {
        _highlight.SetActive(false);
        yield return null;    
    }
}
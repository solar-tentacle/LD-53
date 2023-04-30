using System.Collections;
using UnityEngine;

public class GroundGridElement : GridElement
{
    [SerializeField] private GroundType _type;
    [SerializeField] private HighlightView _highlight;

    public GroundType Type => _type;

    public IEnumerator EnableHighlight(HighlightType type)
    {
        _highlight.Highlight(type);
        yield return null;
    }
    
    public IEnumerator DisableHighlight()
    {
        _highlight.DisableAll();
        yield return null;    
    }
}
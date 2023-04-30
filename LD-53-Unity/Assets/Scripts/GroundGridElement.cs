using System.Collections;
using UnityEngine;

public class GroundGridElement : GridElement
{
    [SerializeField] private GroundType _type;
    [SerializeField] private GameObject _moveHighlight;
    [SerializeField] private GameObject _agroHighlight;

    public GroundType Type => _type;

    public IEnumerator EnableMoveHighlight()
    {
        _moveHighlight.SetActive(true);
        yield return null;
    }
    
    public IEnumerator DisableMoveHighlight()
    {
        _moveHighlight.SetActive(false);
        yield return null;    
    }
    
    public IEnumerator EnableAgroHighlight()
    {
        _agroHighlight.SetActive(true);
        yield return null;
    }
    
    public IEnumerator DisableAgroHighlight()
    {
        _agroHighlight.SetActive(false);
        yield return null;    
    }
}
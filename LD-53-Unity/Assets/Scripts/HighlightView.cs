using System;
using UnityEngine;

public class HighlightView : MonoBehaviour
{
    [SerializeField] private Highlight[] _highlights;

    private void Awake()
    {
        DisableAll();
    }

    public void Enable(HighlightType type)
    {
        foreach (Highlight highlight in _highlights)
        {
            if (highlight.Type != type)
            {
                continue;
            }
            
            highlight.Graphics.SetActive(true);
        }
    }
    
    public void Disable(HighlightType type)
    {
        foreach (Highlight highlight in _highlights)
        {
            if (highlight.Type != type)
            {
                continue;
            }
            
            highlight.Graphics.SetActive(false);
        }
    }
    
    public void DisableAll()
    {
        foreach (Highlight highlight in _highlights)
        {
            highlight.Graphics.SetActive(false);
        }
    }
}

[Serializable] public class Highlight
{
    public HighlightType Type;
    public GameObject Graphics;
}

public enum HighlightType
{
    Move,
    Attack,
    Agro,
}
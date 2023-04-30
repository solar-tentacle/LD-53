using System;
using UnityEngine;

public class HighlightView : MonoBehaviour
{
    [SerializeField] private Highlight[] _highlights;

    private void Awake()
    {
        DisableAll();
    }

    public void Highlight(HighlightType type)
    {
        foreach (var highlight in _highlights)
        {
            highlight.Graphics.SetActive(highlight.Type == type);
        }
    }
    
    public void DisableAll()
    {
        foreach (var highlight in _highlights)
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
    Attack
}
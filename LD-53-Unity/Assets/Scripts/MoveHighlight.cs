using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveHighlight : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject _selection;

    private void OnEnable()
    {
        _selection.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _selection.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _selection.SetActive(false);
    }

    private void OnMouseEnter()
    {
        _selection.SetActive(true);
    }
    
    private void OnMouseExit()
    {
        _selection.SetActive(false);
    }
}

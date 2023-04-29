using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardView : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler
{
    public event Action OnThrowed;
    private bool _isSelected;
    private Image _image;

    private void Start()
    {
        _image = GetComponent<Image>();
    }

    private void Update()
    {
        if (_isSelected)
        {
            var newPos = Input.mousePosition;
            newPos.z = 0f;
            transform.position = newPos;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _isSelected = false;
        OnThrowed?.Invoke();
        gameObject.SetActive(false);
        Debug.Log("Card destroyed [CardView]");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _isSelected = true;
        _image.DOFade(0.3f, 0.3f);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOScale(1.2f, 0.3f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOScale(1.0f, 0.3f);
    }
}
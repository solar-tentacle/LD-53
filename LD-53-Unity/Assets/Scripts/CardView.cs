using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardView : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler
{
    [SerializeField] private float FadeEndValue;
    [SerializeField] private float FadeDuration;
    [SerializeField] private float ScaleEndValueOnEnter;
    [SerializeField] private float ScaleDuration;
    [SerializeField] private float MoveYDuration;
    public event Action OnThrowed;
    private bool _isSelected;
    private Image _image;
    private RectTransform _rectTransform;
    private Vector3 _endValue;
    private float _startPosY;

    private void Start()
    {
        _image = GetComponent<Image>();
        _rectTransform = GetComponent<RectTransform>();
        StartCoroutine(GetStartPositions());
    }

    private IEnumerator GetStartPositions()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        _startPosY = _rectTransform.position.y;
        Debug.Log(_startPosY + " Start");
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
        _image.DOFade(FadeEndValue, FadeDuration);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _endValue = new Vector3(_rectTransform.position.x, _startPosY);
        var tempEndValue = new Vector3(_rectTransform.position.x, _startPosY + 100f);
        transform.DOMove(tempEndValue, MoveYDuration);
        transform.DOScale(ScaleEndValueOnEnter, ScaleDuration);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOScale(1f, ScaleDuration);
        transform.DOMove(_endValue, MoveYDuration);
    }
}
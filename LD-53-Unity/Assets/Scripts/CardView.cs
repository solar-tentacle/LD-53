using System;
using System.Collections;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardView : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler
{
    public event Action Thrown;

    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _titleText;
    [SerializeField] private TMP_Text _descriptionText;

    private float _fadeEndValue = 0.5f;
    private float _scaleEndValueOnEnter = 1.5f;
    private float _fadeDuration = 0.3f;
    private float _scaleDuration = 0.3f;
    private float _moveYDuration = 0.3f;

    private bool _isSelected;
    private RectTransform _rectTransform;
    private Vector3 _endValue;
    private float _startPosY;

    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        StartCoroutine(GetStartPositions());
    }

    public void SetContent(CardConfig cardConfig)
    {
        _titleText.text = cardConfig.Title;
        _descriptionText.text = cardConfig.Description;
        _icon.sprite = cardConfig.Icon;
    }

    private IEnumerator GetStartPositions()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        _startPosY = _rectTransform.position.y;
    }

    private void Update()
    {
        if (_isSelected)
        {
            UpdateCardPos();
        }
    }

    private void UpdateCardPos()
    {
        var newPos = Input.mousePosition;
        newPos.z = 0f;
        transform.position = newPos;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Thrown?.Invoke();
        _isSelected = false;
        // _canvasGroup.DOFade(1f, _fadeDuration);
        // transform.DOMove(_endValue, _moveYDuration);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _isSelected = true;
        _canvasGroup.DOFade(_fadeEndValue, _fadeDuration);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_isSelected) return;
        _endValue = new Vector3(_rectTransform.position.x, _startPosY);
        var tempEndValue = new Vector3(_rectTransform.position.x, _startPosY + 30f);

        transform.DOMove(tempEndValue, _moveYDuration);
        transform.DOScale(_scaleEndValueOnEnter, _scaleDuration);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_isSelected) return;
        transform.DOMove(_endValue, _moveYDuration);
        transform.DOScale(1f, _scaleDuration);
    }
}
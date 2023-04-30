using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardView : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler
{
    [Space]
    
    [SerializeField] private CanvasGroup _canvasGroup;

    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _titleText;
    [SerializeField] private TMP_Text _descriptionText;
    
    private float _fadeEndValue = 0.5f;
    private float _scaleEndValueOnEnter = 1.5f;
    private float _fadeDuration = 0.5f;
    private float _scaleDuration = 0.5f;
    private float _moveYDuration = 0.5f;


    public event Action OnThrowed;
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
        _canvasGroup.DOFade(_fadeEndValue, _fadeDuration);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _endValue = new Vector3(_rectTransform.position.x, _startPosY);
        var tempEndValue = new Vector3(_rectTransform.position.x, _startPosY + 100f);
        
        transform.DOMove(tempEndValue, _moveYDuration);
        transform.DOScale(_scaleEndValueOnEnter, _scaleDuration);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOMove(_endValue, _moveYDuration);
        transform.DOScale(1f, _scaleDuration);
    }
}
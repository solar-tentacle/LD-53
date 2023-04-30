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
    [SerializeField] private float FadeEndValue;
    [SerializeField] private float FadeDuration;
    [SerializeField] private float ScaleEndValueOnEnter;
    [SerializeField] private float ScaleDuration;
    [SerializeField] private float MoveYDuration;

    [Space]
    
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _titleText;
    [SerializeField] private TMP_Text _descriptionText;
    
    
    public event Action OnExecuted;
    private bool _isSelected;
    private RectTransform _rectTransform;
    private Vector3 _endValue;
    private float _startPosY;

    private CardConfig _cardConfig;

    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        StartCoroutine(GetStartPositions());
    }

    public void SetContent(CardConfig cardConfig)
    {
        _cardConfig = cardConfig;
        
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
        StartCoroutine(_cardConfig.Action.Deselect());
        
        if (_cardConfig.Action.CanExecute())
        {
            OnExecuted?.Invoke();
            Destroy(gameObject);
        }
        
        _isSelected = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        StartCoroutine(_cardConfig.Action.Select());
        
        _isSelected = true;
        _canvasGroup.DOFade(FadeEndValue, FadeDuration);
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
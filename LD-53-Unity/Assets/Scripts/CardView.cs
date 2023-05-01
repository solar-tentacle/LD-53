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
    public event Action<CardView> Thrown;
    public event Action<CardView> Picked;

    [SerializeField] private RectTransform _container;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _titleText;
    [SerializeField] private TMP_Text _descriptionText;

    [SerializeField] private GameObject _moveTopPanelGO;
    [SerializeField] private GameObject _actionTopPanelGO;

    [SerializeField] private GameObject _battleIconGO;
    [SerializeField] private GameObject _specialIconGO;

    private float _fadeEndValue = 0.5f;
    private float _scaleEndValueOnEnter = 1.5f;
    private float _fadeDuration = 0.3f;
    private float _scaleDuration = 0.3f;
    private float _moveYDuration = 0.3f;

    private bool _isSelected;
    private Vector3 _endValue;

    public CanvasGroup CanvasGroup => _canvasGroup;
    public RectTransform Container => _container;
    public Vector3 ContainerStartLocalPos = Vector3.zero;
    private bool _disabled;

    private void OnEnable()
    {
        if (_disabled)
        {
            _container.DOLocalMove(_endValue, _moveYDuration);
            _container.DOScale(1f, _scaleDuration);
        }
    }

    private void OnDisable()
    {
        _disabled = true;
    }

    public void SetContent(CardConfig cardConfig)
    {
        _titleText.text = cardConfig.Title;
        _descriptionText.text = cardConfig.Description;
        _icon.sprite = cardConfig.Icon;

        switch (cardConfig.CardType)
        {
            case CardType.Movement:
                _moveTopPanelGO.SetActive(true);
                _actionTopPanelGO.SetActive(false);
                break;
            case CardType.Action:
                _moveTopPanelGO.SetActive(false);
                _actionTopPanelGO.SetActive(true);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        switch (cardConfig.RarityType)
        {
            case RarityType.Common:
                _specialIconGO.SetActive(false);
                break;
            case RarityType.Rare:
                _specialIconGO.SetActive(true);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        _battleIconGO.SetActive(cardConfig.IsOnlyBattle);
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
        _container.position = newPos;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Thrown?.Invoke(this);
        _isSelected = false;
        _canvasGroup.DOFade(1f, _fadeDuration);
        // transform.DOMove(_endValue, _moveYDuration);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Picked?.Invoke(this);
        _isSelected = true;
        _canvasGroup.DOFade(_fadeEndValue, _fadeDuration);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_isSelected) return;

        _endValue = new Vector3(_container.localPosition.x, ContainerStartLocalPos.y);
        Vector3 tempEndValue = _endValue + Vector3.up * 30f;

        Debug.Log(tempEndValue);
        _container.DOLocalMove(tempEndValue, _moveYDuration);
        _container.DOScale(_scaleEndValueOnEnter, _scaleDuration);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_isSelected) return;
        _container.DOLocalMove(_endValue, _moveYDuration);
        _container.DOScale(1f, _scaleDuration);
    }
}
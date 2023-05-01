using System.Collections;
using DG.Tweening;
using UnityEngine;

public class UICardsHand : ActivateView
{
    [SerializeField] private CardView _cardViewPrefab;
    [SerializeField] private Transform _cardViewParent;
    [SerializeField] private Transform _selectedCardParent;
    [SerializeField] private GameObject _blocker;
    [SerializeField] private Transform _drawStartPoint;
    public Transform DrawStartPoint => _drawStartPoint;
    
    [SerializeField] private Transform _drawCenterPoint;
    public Transform DrawCenterPoint => _drawCenterPoint;
    
    [SerializeField] private RectTransform _darkRect;
    [SerializeField] private CanvasGroup _darkRectCanvasGroup;

    public RectTransform DarkRect => _darkRect;
    
    public CardView CreateCard(CardConfig cardConfig)
    {
        var view = Instantiate(_cardViewPrefab, _cardViewParent);
        view.SetContent(cardConfig);

        return view;
    }

    public void ClearHand()
    {
        _cardViewParent.Clear();
    }

    public IEnumerator SelectCard(CardView view)
    {
        view.enabled = false;
        yield return view.Container.DOMove(_selectedCardParent.position, 0.3f).WaitForCompletion();
        view.InSelectionZone = true;
    }
    
    public IEnumerator MoveCardToHand(CardView view)
    {
        view.enabled = false;
        view.ScaleToDefault();
        yield return view.Container.DOLocalMove(view.ContainerStartLocalPos, 0.3f).WaitForCompletion();
        view.enabled = true;
        view.InSelectionZone = false;
    }

    public IEnumerator HideCard(CardView view)
    {
        yield return view.Container.DOScale(Vector3.zero, 0.3f).WaitForCompletion();
        Destroy(view.gameObject);
    }

    public void EnableBlocker()
    {
        _blocker.SetActive(true);
        ShowDarkRect();
    }

    public void DisableBlocker()
    {
        _blocker.SetActive(false);
        HideDarkRect();
    }

    public IEnumerator DrawAnimation(CardView view, bool isFromDeck)
    {
        view.Container.position = isFromDeck ? _drawStartPoint.position : _drawCenterPoint.position;
        yield return view.Container.DOLocalMove(view.ContainerStartLocalPos, 1).WaitForCompletion();
    }

    public void ShowDarkRect()
    {
        _darkRectCanvasGroup.DOFade(1, 0.3f);
    }
    
    public void HideDarkRect()
    {
        _darkRectCanvasGroup.DOFade(0, 0.3f);
    }
}
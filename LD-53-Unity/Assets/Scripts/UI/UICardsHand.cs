using System.Collections;
using DG.Tweening;
using UnityEngine;

public class UICardsHand : ActivateView
{
    [SerializeField] private CardView _cardViewPrefab;
    [SerializeField] private Transform _cardViewParent;
    [SerializeField] private Transform _selectedCardParent;

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
        view.transform.SetParent(_selectedCardParent);
        view.enabled = false;
        yield return view.transform.DOLocalMove(Vector3.zero, 0.3f).WaitForCompletion();
    }

    public IEnumerator HideCard(CardView view)
    {
        yield return view.transform.DOScale(Vector3.zero, 0.3f);
    }
}
using UnityEngine;
using UnityEngine.UI;

public class UICardsHand : ActivateView
{
    [SerializeField] private CardView _cardViewPrefab;
    [SerializeField] private Transform _cardViewParent;

    public CardView CreateCard(CardConfig cardConfig)
    {
        var view = Instantiate(_cardViewPrefab, _cardViewParent);
        view.SetContent(cardConfig);

        return view;
    }

    private void ClearHand()
    {
        _cardViewParent.Clear();
    }
}
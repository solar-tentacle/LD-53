using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardHandService : IService, IInject
{
    private UIService _uiService;
    private UICardsHand _uiHand;
    
    private Dictionary<CardView, Card> _cards = new();
    private CardView _selectedCardView;

    private CardDeck _currentHand = new CardDeck();

    public Card SelectedCard => _cards[_selectedCardView];

    void IInject.Inject()
    {
        _uiService = Services.Get<UIService>();
        _uiHand = _uiService.UICanvas.HUD.UICardsHand;
    }

    public void FillCurrentHand(List<Card> cards)
    {
        for (int i = 0; i < cards.Count; i++)
        {
            AddCard(cards[i]);
        }
    }

    public void AddCard(Card card)
    {
        _currentHand.AddCard(card);

        var view = _uiHand.CreateCard(card.Config);
        _cards.Add(view, card);
        view.Thrown += () => OnCardThrown(view);
    }

    public void RemoveCard(Card card)
    {
        _currentHand.RemoveCard(card);
    }

    private void OnCardThrown(CardView view)
    {
        _selectedCardView = view;
    }

    public IEnumerator SelectCardFlow()
    {
        yield return new WaitUntil(() => _selectedCardView != null);
        yield return _uiHand.SelectCard(_selectedCardView);
    }

    public IEnumerator HideCardFlow()
    {
        yield return _uiHand.HideCard(_selectedCardView);
        RemoveCard(_cards[_selectedCardView]);
        _selectedCardView = null;
    }

    private IEnumerator ExecuteAction(CardAction action)
    {
        yield return action.Execute();
    }

    public void RemoveAllCards()
    {
        var cards = _currentHand.GetCopyCardsList();

        for (int i = 0; i < cards.Count; i++)
        {
            RemoveCard(cards[i]);
        }

        _uiService.UICanvas.HUD.UICardsHand.ClearHand();
    }
}
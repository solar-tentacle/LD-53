using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardHandService : IService, IInject
{
    private UIService _uiService;
    private UICardsHand _uiHand;

    private Dictionary<CardView, Card> _cards = new();
    private CardView _selectedCardView;
    private CardView _copyCardView;

    private CardDeck _currentHand = new CardDeck();
    public bool Has(CardType cardType) => _currentHand.Has(cardType);

    public Card SelectedCard => _selectedCardView == null ? null : _cards[_selectedCardView];
    public CardView SelectedCardView => _selectedCardView;
    public RectTransform DarkRect { get; private set; }

    void IInject.Inject()
    {
        _uiService = Services.Get<UIService>();
        _uiHand = _uiService.UICanvas.HUD.UICardsHand;
        DarkRect = _uiHand.DarkRect;
    }

    public void FillCurrentHand(List<Card> cards)
    {
        for (int i = 0; i < cards.Count; i++)
        {
            AddCard(cards[i]);
        }
    }

    public CardView AddCard(Card card)
    {
        _currentHand.AddCard(card);

        var view = _uiHand.CreateCard(card.Config);
        _cards.Add(view, card);

        return view;
    }

    public void RemoveCard(Card card)
    {
        _currentHand.RemoveCard(card);
    }

    private void OnCardThrown(CardView view)
    {
        _selectedCardView = view;
    }

    private void OnCardPicked(CardView view)
    {
        _copyCardView = view;
    }

    public IEnumerator SelectCardFlow(bool isBattle)
    {
        _uiHand.DisableBlocker();

        foreach ((CardView view, Card card) in _cards)
        {
            view.Thrown += OnCardThrown;
            view.enabled = true;
            if (card.Config.IsOnlyBattle && isBattle == false) view.enabled = false;
        }

        _selectedCardView = null;
        yield return new WaitUntil(() => _selectedCardView != null);
        RemoveCard(_cards[_selectedCardView]);
        _uiHand.ShowDarkRect();
        yield return _uiHand.SelectCard(_selectedCardView);
        _uiHand.EnableBlocker();

        foreach (CardView view in _cards.Keys)
        {
            view.Thrown -= OnCardThrown;
        }
    }

    public IEnumerator HideCardFlow()
    {
        _cards.Remove(_selectedCardView);
        yield return _uiHand.HideCard(_selectedCardView);
    }

    public IEnumerator CopyCardFlow()
    {
        _uiHand.HideDarkRect();
        _uiHand.DisableBlocker();

        foreach ((CardView view, Card card) in _cards)
        {
            if (view == _selectedCardView) continue;
            view.Picked += OnCardPicked;
            if (card.Config.CardType == CardType.Action) view.enabled = true;
        }

        _copyCardView = null;
        yield return new WaitUntil(() => _copyCardView != null);
        _uiHand.EnableBlocker();
        _selectedCardView.SetContent(_cards[_copyCardView].Config);
        _cards[_selectedCardView] = _cards[_copyCardView];
        _currentHand.AddCard(_cards[_copyCardView]);

        foreach (CardView view in _cards.Keys)
        {
            view.Picked -= OnCardPicked;
        }

        yield return _uiHand.MoveCardToHand(_selectedCardView);
        _selectedCardView = null;
    }

    public IEnumerator CancelFlow()
    {
        yield return _uiHand.MoveCardToHand(_selectedCardView);
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

    public IEnumerator DrawAnimation(CardView view, bool isFromDeck)
    {
        yield return _uiHand.DrawAnimation(view, isFromDeck);
    }
}
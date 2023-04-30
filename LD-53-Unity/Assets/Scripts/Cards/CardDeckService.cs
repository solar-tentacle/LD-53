using System.Collections.Generic;

public class CardDeckService : IService, IInject, IStart
{
    private CardHandService _cardHandService;
    private UIService _uiService;

    private CardDeck _deck = new CardDeck();
    private CardPile _drawPile = new CardPile();

    public void Inject()
    {
        _cardHandService = Services.Get<CardHandService>();
        _uiService = Services.Get<UIService>();
    }

    void IStart.GameStart()
    {
        AssetsCollection assetsCollection = Services.Get<AssetsCollection>();

        var startCards = assetsCollection.GameConfig.StartCards;

        for (int i = 0; i < assetsCollection.GameConfig.StartCards.Count; i++)
        {
            var card = new Card(startCards[i]);
            _deck.AddCard(card);
            _drawPile.AddCard(card);
        }

        _drawPile.ShuffleAll();

        FillCurrentDeck();
    }

    public void FillCurrentDeck()
    {
        var cards = new List<Card>();

        if (TryGetCardFromDrawPile(CardType.Movement, out var moveCard))
        {
            cards.Add(moveCard);
        }

        if (TryGetCardFromDrawPile(CardType.Action, out var actionCard))
        {
            cards.Add(actionCard);
        }

        _cardHandService.FillCurrentHand(cards);
        
        UpdateDeckIndicator();
    }

    public void TryAddCardFromCurrentDeck(CardType type)
    {
        if (TryGetCardFromDrawPile(type, out var card))
        {
            _cardHandService.AddCard(card);
        }
        
        UpdateDeckIndicator();
    }

    private void UpdateDeckIndicator()
    {
        _uiService.UICanvas.HUD.UIDeckIndicator.UpdateView(_drawPile.GetCount(CardType.Movement),
            _drawPile.GetCount(CardType.Action));
    }

    private bool TryGetCardFromDrawPile(CardType cardType, out Card card)
    {
        if (_drawPile.HasCard(cardType))
        {
            card = _drawPile.ReceiveCard(cardType);
            return true;
        }

        card = null;
        return false;
    }
}
using System.Collections.Generic;

public class CardDeckService : IService, IInject, IStart
{
    private CardHandService _cardHandService;
    
    private CardDeck _deck = new CardDeck();
    private CardPile _drawPile = new CardPile();
    private CardPile _discardPile = new CardPile();

    public void Inject()
    {
        _cardHandService = Services.Get<CardHandService>();
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
        cards.Add(GetCardFromDrawPile(CardType.Movement));
        cards.Add(GetCardFromDrawPile(CardType.Action));
        
        _cardHandService.FillCurrentHand(cards);
    }

    public void RemoveCardFromCurrentDeck(Card card)
    {
        _cardHandService.RemoveCard(card);
        _discardPile.AddCard(card);
    }

    private Card GetCardFromDrawPile(CardType cardType)
    {
        if (_drawPile.HasCard(cardType))
        {
            return _drawPile.ReceiveCard(cardType);
        }
        
        FillDrawPileByDiscardPile(cardType);
        return _drawPile.ReceiveCard(cardType);
    }

    private void FillDrawPileByDiscardPile(CardType cardType)
    {
        while (_discardPile.HasCard(cardType))
        {
            var card = _discardPile.ReceiveCard(cardType);
            _drawPile.AddCard(card);
        }
        
        _drawPile.Shuffle(cardType);
    }

    public void EndTurn()
    {
        _cardHandService.RemoveAllCards();
        
        FillCurrentDeck();
    }
}
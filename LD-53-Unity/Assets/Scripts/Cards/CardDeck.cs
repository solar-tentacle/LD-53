using System.Collections.Generic;
using System.Linq;

public class CardDeck
{
    private List<Card> _cards = new List<Card>();

    public void AddCard(Card card)
    {
        _cards.Add(card);
    }

    public void RemoveCard(Card card)
    {
        _cards.Remove(card);
    }

    public List<Card> GetCopyCardsList()
    {
        return new List<Card>(_cards);
    }

    public bool Has(CardType type)
    {
        return _cards.Find(card => card.Config.CardType == type) != null;
    }
}
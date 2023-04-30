using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CardPile
{
    private Dictionary<CardType, Queue<Card>> _cards = new Dictionary<CardType, Queue<Card>>();

    public bool HasCard(CardType type) => _cards[type].Count > 0;

    public int GetCount(CardType type) => _cards[type].Count;

    public CardPile()
    {
        foreach (CardType value in Enum.GetValues(typeof(CardType)))
        {
            _cards.Add(value, new Queue<Card>());
        }
    }

    public void AddCard(Card card)
    {
        _cards[card.Config.CardType].Enqueue(card);
    }

    public Card ReceiveCard(CardType type)
    {
        if (_cards.Count <= 0)
        {
            Debug.LogError("Not found card in pile. Type = " + type);
        }
        
        return _cards[type].Dequeue();
    }

    public void ShuffleAll()
    {
        var newCards = new Dictionary<CardType, Queue<Card>>();
        
        foreach (var cardPair in _cards)
        {
            var shuffledQueue = ShuffleQueue(cardPair.Value);
            newCards.Add(cardPair.Key, shuffledQueue);
        }

        _cards = newCards;
    }

    public void Shuffle(CardType cardType)
    {
        _cards[cardType] = ShuffleQueue(_cards[cardType]);
    }
    

    private Queue<Card> ShuffleQueue(Queue<Card> queue)
    {
        var myArray = queue.ToArray();

        for (int i = myArray.Length - 1; i > 0; i--)        

        {
            var j = Random.Range(0, i + 1);
            (myArray[i], myArray[j]) = (myArray[j], myArray[i]);
        }

        return new Queue<Card>(myArray);
    }
}
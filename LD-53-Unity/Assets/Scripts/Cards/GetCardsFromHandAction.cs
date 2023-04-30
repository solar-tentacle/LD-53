using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GetCardsFromHandAction : CardAction
{
    [SerializeField] private List<CardType> Cards = new List<CardType>();

    private CardDeckService _cardDeckService;

    public override void Init()
    {
        _cardDeckService = Services.Get<CardDeckService>();
    }

    public override IEnumerator Select()
    {
        yield return null;
    }

    public override IEnumerator Deselect()
    {
        yield return null;
    }

    public override bool CanExecute()
    {
        return true;
    }

    public override IEnumerator Execute()
    {
        for (int i = 0; i < Cards.Count; i++)
        {
            _cardDeckService.TryAddCardFromCurrentDeck(Cards[i]);
        }
        
        yield return null;
    }
}
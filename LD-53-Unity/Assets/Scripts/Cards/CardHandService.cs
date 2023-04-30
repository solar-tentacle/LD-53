﻿using System.Collections;
using System.Collections.Generic;

public class CardHandService : IService, IInject
{
    private UIService _uiService;
    private CoroutineService _coroutineService;
    
    private CardDeck _currentHand = new CardDeck();
    
    public void Inject()
    {
        _uiService = Services.Get<UIService>();
        _coroutineService = Services.Get<CoroutineService>();
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
        
        var view = _uiService.UICanvas.HUD.UICardsHand.CreateCard(card.Config);
        view.OnThrowed += () => OnCardUsed(card);
    }

    public void RemoveCard(Card card)
    {
        _currentHand.RemoveCard(card);
    }

    private void OnCardUsed(Card card)
    {
        _coroutineService.StartCoroutine(ExecuteActions(card.Config.Actions));
        
        RemoveCard(card);
    }

    private IEnumerator ExecuteActions(List<CardAction> actions)
    {
        for (int i = 0; i < actions.Count; i++)
        {
            yield return actions[i];
        }
    }
}
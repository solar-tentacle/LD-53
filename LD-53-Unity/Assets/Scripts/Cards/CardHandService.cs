using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardHandService : IService, IInject
{
    private UIService _uiService;
    private UICardsHand _uiHand;
    private CoroutineService _coroutineService;
    private CardHandService _cardHandService;
    
    private CardDeckService _cardDeckService;
    private Dictionary<CardView, Card> _cards = new();
    private CardView _selectedCard;

    private CardDeck _currentHand = new CardDeck();
    
    public void Inject()
    {
        _uiService = Services.Get<UIService>();
        _uiHand = _uiService.UICanvas.HUD.UICardsHand;
        _coroutineService = Services.Get<CoroutineService>();
        _cardHandService = Services.Get<CardHandService>();
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
        _selectedCard = view;
        _coroutineService.StartCoroutine(ExecuteFlow());
    }

    private IEnumerator ExecuteFlow()
    {
        CardAction action = _cards[_selectedCard].Config.Action;

        yield return _uiHand.SelectCard(_selectedCard);
        yield return action.Select();

        while (true)
        {
            if (Input.GetMouseButtonDown(0) && action.CanExecute())
            {
                yield return action.Deselect();
                yield return action.Execute();
                break;
            }

            yield return null;
        }

        yield return _uiHand.HideCard(_selectedCard);
        _cardHandService.RemoveCard(_cards[_selectedCard]);
        _selectedCard = null;
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
            _cardHandService.RemoveCard(cards[i]);
        }

        _uiService.UICanvas.HUD.UICardsHand.ClearHand();
    }
}
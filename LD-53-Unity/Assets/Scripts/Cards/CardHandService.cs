using System.Collections;
using System.Collections.Generic;

public class CardHandService : IService, IInject
{
    private UIService _uiService;
    private CoroutineService _coroutineService;
    private CardHandService _cardHandService;
    
    private CardDeck _currentHand = new CardDeck();
    
    public void Inject()
    {
        _uiService = Services.Get<UIService>();
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
        
        var view = _uiService.UICanvas.HUD.UICardsHand.CreateCard(card.Config);
        view.OnExecuted += () => OnCardUsed(card);
    }

    public void RemoveCard(Card card)
    {
        _currentHand.RemoveCard(card);
    }

    private void OnCardUsed(Card card)
    {
        _coroutineService.StartCoroutine(ExecuteAction(card.Config.Action));

        _cardHandService.RemoveCard(card);
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
public class EndTurnService : IService, IInject
{
    private CardDeckService _cardDeckService;
    
    public void Inject()
    {
        _cardDeckService = Services.Get<CardDeckService>();
    }
    
    public void EndTurn()
    {
        _cardDeckService.EndTurn();
    }
}
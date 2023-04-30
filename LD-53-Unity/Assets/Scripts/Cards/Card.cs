public class Card
{
    private CardConfig _config;
    public CardConfig Config => _config;

    public Card(CardConfig config)
    {
        _config = config;
    }
}
using UnityEngine;

public class Card
{
    private CardConfig _config;
    public CardConfig Config => _config;

    public Card(CardConfig config)
    {
        _config = config;

        if (_config.Action == null)
        {
            Debug.LogError("Card Action is null. Name = " + config.name);
        }
        else
        {
            _config.Action.Init();
        }
    }
}
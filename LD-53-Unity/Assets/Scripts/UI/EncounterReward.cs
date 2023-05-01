using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class EncounterReward
{
    public float Health;
    public int Coins;
    public List<CardType> GetCardsFromDeck = new List<CardType>();
    public List<CardConfig> CreateAndAddCardsToHand = new List<CardConfig>();

    public IEnumerator GiveReward()
    {
        var playerService = Services.Get<PlayerService>();
        var unitService = Services.Get<UnitService>();
        
        yield return unitService.ChangeUnitHealth(playerService.PlayerView, (int)Health);

        var inventoryService = Services.Get<InventoryService>();
        inventoryService.AddCoins(Coins);

        var cardDeckService = Services.Get<CardDeckService>();
        
        for (int i = 0; i < GetCardsFromDeck.Count; i++)
        {
            yield return cardDeckService.TryAddCardFromCurrentDeck(GetCardsFromDeck[i]);
        }

        for (int i = 0; i < CreateAndAddCardsToHand.Count; i++)
        {
            yield return cardDeckService.AddNewCardByReward(CreateAndAddCardsToHand[i]);
        }
    }
}
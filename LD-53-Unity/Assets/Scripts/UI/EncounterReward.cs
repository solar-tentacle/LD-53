﻿using System;
using System.Collections.Generic;

[Serializable]
public class EncounterReward
{
    public float Health;
    public List<CardType> GetCardsFromDeck = new List<CardType>();

    public void GiveReward()
    {
        var playerService = Services.Get<PlayerService>();
        var unitService = Services.Get<UnitService>();
        
        unitService.ChangeUnitHealth(playerService.PlayerView, (int)Health);

        var cardDeckService = Services.Get<CardDeckService>();
        
        for (int i = 0; i < GetCardsFromDeck.Count; i++)
        {
            cardDeckService.TryAddCardFromCurrentDeck(GetCardsFromDeck[i]);
        }
    }
}
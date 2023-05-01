using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Game/GameConfig", order = 45)]
public class GameConfig : ScriptableObject
{
    public int StartCoins;
    public int CoinsPerMove = 1;
    public int StartPizzaCount = 5;

    public List<CardConfig> StartCards = new List<CardConfig>();

    [Space] 
    public string EndHealthLoseReasonText = "Reason: 0 Health";
    public string EndMoveCardsLoseReasonText = "Reason: 0 Move Cards";
}
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardConfig", menuName = "Game/CardConfig", order = 45)]
public class CardConfig : ScriptableObject
{
    public string Title;
    public string Description;
    public Sprite Icon;
    public CardType CardType;
    public RarityType RarityType;

    [SerializeReference] public List<CardAction> Actions = new List<CardAction>();
}
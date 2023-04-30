using UnityEngine;

public class UIHud : ActivateView
{
    [SerializeField] private UICardsHand _uiCardsHand;
    public UICardsHand UICardsHand => _uiCardsHand;
}
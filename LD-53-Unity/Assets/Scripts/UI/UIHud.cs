using UnityEngine;

public class UIHud : ActivateView
{
    [SerializeField] private UICardsHand _uiCardsHand;
    public UICardsHand UICardsHand => _uiCardsHand;

    [SerializeField] private UIDeckIndicator _uiDeckIndicator;
    public UIDeckIndicator UIDeckIndicator => _uiDeckIndicator;
}
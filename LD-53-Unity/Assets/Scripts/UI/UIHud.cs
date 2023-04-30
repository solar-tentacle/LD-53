using System;
using UnityEngine;
using UnityEngine.UI;

public class UIHud : ActivateView
{
    [SerializeField] private Button _endTurnButton;
    [SerializeField] private Button _clearButton;
    [SerializeField] private UICardsHand _uiCardsHand;
    public UICardsHand UICardsHand => _uiCardsHand;

    private void Start()
    {
        _endTurnButton.onClick.AddListener(OnEndTurnButtonClicked);
    }

    private void OnEndTurnButtonClicked()
    {
        Services.Get<EndTurnService>().EndTurn();
    }
}
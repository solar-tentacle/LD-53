﻿using UnityEngine;
using UnityEngine.UI;

public class UIHud : ActivateView
{
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _clearButton;
    [SerializeField] private UICardsHand _uiCardsHand;
    public UICardsHand UICardsHand => _uiCardsHand;
}
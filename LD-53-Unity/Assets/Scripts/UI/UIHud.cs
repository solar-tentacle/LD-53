using UI;
using UnityEngine;

public class UIHud : ActivateView
{
    [SerializeField] private UICardsHand _uiCardsHand;
    public UICardsHand UICardsHand => _uiCardsHand;

    [SerializeField] private UIDeckIndicator _uiDeckIndicator;
    public UIDeckIndicator UIDeckIndicator => _uiDeckIndicator;
    
    [SerializeField] private GameObject _battleInProgress;
    public GameObject BattleInProgress => _battleInProgress;
    
    [SerializeField] private HealthView _playerHealth;
    public HealthView PlayerHealth => _playerHealth;

    [SerializeField] private UIPanel _coinsPanel;
    public UIPanel CoinsPanel => _coinsPanel;

    protected override void OnInit()
    {
        base.OnInit();
        BattleInProgress.SetActive(false);
    }
}
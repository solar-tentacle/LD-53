using UnityEngine;

public class UIHud : ActivateView
{
    [SerializeField] private UICardsHand _uiCardsHand;
    public UICardsHand UICardsHand => _uiCardsHand;

    [SerializeField] private UIDeckIndicator _uiDeckIndicator;
    public UIDeckIndicator UIDeckIndicator => _uiDeckIndicator;
    
    [SerializeField] private GameObject _battleInProgress;
    public GameObject BattleInProgress => _battleInProgress;

    protected override void OnInit()
    {
        base.OnInit();
        BattleInProgress.SetActive(false);
    }
}
using UnityEngine;
using UnityEngine.UI;

public class  UIWinWindow : ActivateView
{
    [SerializeField] private Button _nextLevelButton;

    private void Start()
    {
        _nextLevelButton.onClick.AddListener(OnNextButtonClicked);
    }

    private void OnNextButtonClicked()
    {
        Services.Get<GameFlowService>().CompleteLevel();
    }
}
using UnityEngine;
using UnityEngine.UI;

public class UILoseWindow : ActivateView
{
    [SerializeField] private Button _restartButton;

    private void Start()
    {
        _restartButton.onClick.AddListener(OnRestartButtonClicked);
    }

    private void OnRestartButtonClicked()
    {
        Services.Get<GameFlowService>().RestartLevel();
    }
}
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UILoseWindow : ActivateView
{
    [SerializeField] private Button _restartButton;
    [SerializeField] private TMP_Text _reasonText;

    private void Start()
    {
        _restartButton.onClick.AddListener(OnRestartButtonClicked);
    }

    public void SetContent(string reasonText)
    {
        _reasonText.text = reasonText;
    }

    private void OnRestartButtonClicked()
    {
        Services.Get<GameFlowService>().RestartLevel();
    }
}
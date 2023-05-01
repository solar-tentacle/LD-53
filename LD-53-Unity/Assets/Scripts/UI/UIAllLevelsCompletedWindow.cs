using System.Collections;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class  UIAllLevelsCompletedWindow : ActivateView
{
    [SerializeField] private Button _nextLevelButton;
    [SerializeField] private ProgressBarUI _pizzaProgressBar;
    private bool _clicked;

    private void Start()
    {
        _nextLevelButton.onClick.AddListener(OnNextButtonClicked);
    }

    private void OnNextButtonClicked()
    {
        _clicked = true;
    }

    protected override void OnShow()
    {
        _clicked = false;

        var pizzaService = Services.Get<PizzaService>();
        var assetsCollection = Services.Get<AssetsCollection>();
        _pizzaProgressBar.SetValue(pizzaService.GetLeftPizzaCount, assetsCollection.GameConfig.StartPizzaCount);
        
        base.OnShow();
    }

    public IEnumerator WaitForClose()
    {
        while (!_clicked)
        {
            yield return null;
        }
    }
}
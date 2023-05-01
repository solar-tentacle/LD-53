using UnityEngine;

namespace UI
{
    public class PizzaService : IService, IStart
    {
        private ProgressBarUI _pizzaProgressBar;
        private AssetsCollection _assetsCollection;

        public void GameStart()
        {
            _pizzaProgressBar = Services.Get<UIService>().UICanvas.HUD.PizzaProgressBar;
            _assetsCollection = Services.Get<AssetsCollection>();
            
            UpdatePizzaProgressBar();
        }
        
        private void UpdatePizzaProgressBar()
        {
            _pizzaProgressBar.SetValue(GetLeftPizzaCount, _assetsCollection.GameConfig.StartPizzaCount);
        }

        public int GetLeftPizzaCount =>
            Mathf.Max(0, _assetsCollection.GameConfig.StartPizzaCount - GameFlowService.DieCount);
    }
}
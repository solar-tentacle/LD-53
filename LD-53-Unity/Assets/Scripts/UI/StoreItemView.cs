using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class StoreItemView : MonoBehaviour
    {
        [SerializeField] private GameObject _buyGO;
        
        [SerializeField] private GameObject _cardGO;
        [SerializeField] private GameObject _healthGO;
        
        [SerializeField] private GameObject _wasBoughtGO;

        [SerializeField] private CardView _cardView;
        [SerializeField] private Button _buyButton;

        [SerializeField] private UIPanel _pricePanel;
        [SerializeField] private Color CanBuyColor = Color.white;
        [SerializeField] private Color NotEnoughColor = Color.red;

        private StoreItemData _data;
        public StoreItemData Data => _data;

        private Func<StoreItemData, bool> _buyFunc;

        private void Start()
        {
            _buyButton.onClick.AddListener(OnBuyButtonClicked);
        }

        public void SetContent(StoreItemData data, Func<StoreItemData, bool> buyFunc)
        {
            _data = data;
            _buyFunc = buyFunc;

            if (data.Reward.CreateAndAddCardsToHand.Count > 0)
            {
                _cardView.SetContent(data.Reward.CreateAndAddCardsToHand[0]);
            }
            
            _pricePanel.SetText(data.Cost.ToString());
        }

        public void UpdateView(bool wasBought)
        {
            var inventoryService = Services.Get<InventoryService>();
            
            _pricePanel.SetTextColor(inventoryService.HasEnough(_data.Cost) ? CanBuyColor : NotEnoughColor);
            
            if (wasBought)
            {
                _wasBoughtGO.SetActive(true);
                _buyGO.SetActive(false);
            }
            else
            {
                _wasBoughtGO.SetActive(false);
                _buyGO.SetActive(true);

                var isCard = _data.Reward.CreateAndAddCardsToHand.Count > 0;
                var isHealth = _data.Reward.Health > 0;
                
                _cardGO.SetActive(isCard);
                _healthGO.SetActive(isHealth);
            }
        }

        private void OnBuyButtonClicked()
        {
            if (_buyFunc.Invoke(_data))
            {
                UpdateView(true);
            }
        }

        private void OnDestroy()
        {
            _buyButton.onClick.RemoveListener(OnBuyButtonClicked);
        }
    }
}
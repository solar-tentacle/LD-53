using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIStoreWindow : ActivateView
    {
        [SerializeField] private StoreItemView _itemPrefab;
        [SerializeField] private Transform _itemsParent;

        [SerializeField] private Button _closeButton;

        private List<StoreItemView> _items = new List<StoreItemView>();

        private StoreData _storeData;
        private StoreState _storeState;

        protected override void OnInit()
        {
            base.OnInit();
            
            _closeButton.onClick.AddListener(OnCloseButtonClicked);
        }

        public void SetContent(StoreData storeData, StoreState storeState, Func<StoreItemData, bool> buyFunc)
        {
            _storeData = storeData;
            _storeState = storeState;
            
            Clear();

            for (int i = 0; i < storeData.StoreItemDatas.Count; i++)
            {
                var data = storeData.StoreItemDatas[i];
                CreateItem(data, buyFunc);
            }
            
            UpdateView();
        }

        private void CreateItem(StoreItemData data, Func<StoreItemData, bool> buyFunc)
        {
            var item = Instantiate(_itemPrefab, _itemsParent);
            item.SetContent(data, buyFunc);
            _items.Add(item);
        }

        public void UpdateView()
        {
            for (int i = 0; i < _items.Count; i++)
            {
                var item = _items[i];
                var wasBought = _storeState.purchasedDatas.Exists(d => d == item.Data);
                item.UpdateView(wasBought);
            }
        }

        private void Clear()
        {
            _itemsParent.Clear();
            _items.Clear();
        }

        private void OnCloseButtonClicked()
        {
            Hide();
        }

        private void OnDestroy()
        {
            _closeButton.onClick.RemoveListener(OnCloseButtonClicked);
        }
    }
}
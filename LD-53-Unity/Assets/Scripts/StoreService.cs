using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

public class StoreService : IService, IInject, IStart
{
    private GridService _gridService;
    private Dictionary<Vector2Int, ObjectGridElement> _storePositions;
    private Dictionary<ObjectGridElement, StoreState> _storeStates = new Dictionary<ObjectGridElement, StoreState>();
    private UIService _uiService;
    private InventoryService _inventoryService;
    private UIStoreWindow _uiStoreWindow;

    private StoreState _currentState;
    private List<EncounterReward> _encounterRewards = new List<EncounterReward>();
    
    public void Inject()
    {
        _gridService = Services.Get<GridService>();
        _uiService = Services.Get<UIService>();
        _inventoryService = Services.Get<InventoryService>();
        _uiStoreWindow = _uiService.UICanvas.UIStoreWindow;
    }
    
    public void GameStart()
    {
        _storePositions = _gridService.GetObjectsPositions(ObjectType.Store);
    }
    
    public bool TryGetStore(Vector2Int playerPos, out StoreGridElement store)
    {
        store = null;
        if (_storePositions.TryGetValue(playerPos, out var element))
        {
            store = element as StoreGridElement;
            return true;
        }

        return false;
    }
    
    public IEnumerator Flow(StoreGridElement store, Vector2Int storePosition)
    {
        _encounterRewards.Clear();
        
        store.gameObject.SetActive(false);

        if (!_storeStates.ContainsKey(store))
        {
            _storeStates.Add(store, new StoreState());
        }

        _currentState = _storeStates[store];
        
        _uiStoreWindow.SetContent(store.StoreData, _currentState, TryBuy);
        _uiStoreWindow.Show();

        yield return new WaitUntil(() => !_uiStoreWindow.IsShow);

        for (int i = 0; i < _encounterRewards.Count; i++)
        {
            yield return _encounterRewards[i].GiveReward();
        }
    }

    public bool TryBuy(StoreItemData storeItemData)
    {
        if (!_inventoryService.HasEnough(storeItemData.Cost))
        {
            return false;
        }

        if (_currentState.purchasedDatas.Contains(storeItemData))
        {
            return false;
        }
        
        _inventoryService.AddCoins(-storeItemData.Cost);
        
        _currentState.purchasedDatas.Add(storeItemData);
        _encounterRewards.Add(storeItemData.Reward);
        
        return true;
    }
}
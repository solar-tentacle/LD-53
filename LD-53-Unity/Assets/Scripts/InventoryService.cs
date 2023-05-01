
public class InventoryService : IService, IInject, IStart
{
    private AssetsCollection _assetsCollection;
    
    private int _coins;
    private UIService _uiService;

    public void Inject()
    {
        _assetsCollection = Services.Get<AssetsCollection>();
        _uiService = Services.Get<UIService>();
    }

    public void GameStart()
    {
        _coins = _assetsCollection.GameConfig.StartCoins;
        
        UpdateView();
    }

    public bool HasEnough(int needValue)
    {
        return needValue <= _coins;
    }

    public void AddCoins(int value)
    {
        _coins += value;
        
        UpdateView();
    }

    private void UpdateView()
    {
        _uiService.UICanvas.HUD.CoinsPanel.SetText(_coins.ToString());
    }
}
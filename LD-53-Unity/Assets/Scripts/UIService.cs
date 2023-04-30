using UnityEngine;

public class UIService : Service, IInject
{
    [SerializeField] private UICanvas _uiCanvas;
    public UICanvas UICanvas => _uiCanvas;

    public void Inject()
    {
        _uiCanvas.HUD.Show();
        _uiCanvas.HUD.UICardsHand.Show();
    }
}
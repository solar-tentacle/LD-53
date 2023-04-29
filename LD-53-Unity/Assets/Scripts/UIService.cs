using UnityEngine;

public class UIService : Service, IInject
{
    [SerializeField] private UICanvas _uiCanvas;
    public void Inject()
    {
        _uiCanvas.HUD.Show();
    }
}
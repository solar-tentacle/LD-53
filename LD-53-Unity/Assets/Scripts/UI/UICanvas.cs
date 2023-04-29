using UnityEngine;

public class UICanvas : MonoBehaviour
{
    [SerializeField] private UIHud _hud;

    public UIHud HUD => _hud;
    [SerializeField] private UIWinWindow _uiWinWindow;
}
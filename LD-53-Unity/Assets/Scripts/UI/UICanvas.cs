using System;
using UI;
using UnityEngine;

public class UICanvas : MonoBehaviour
{
    [SerializeField] private UIHud _hud;
    [SerializeField] private HealthView _healthViewPrefab;

    public UIHud HUD => _hud;
    [SerializeField] private UIWinWindow _uiWinWindow;

    [SerializeField] private UILoseWindow _uiLoseWindow;

    private void Start()
    {
        _healthViewPrefab.gameObject.SetActive(false);
    }

    public HealthView AddHealthView()
    {
        var result = Instantiate(_healthViewPrefab, transform);
        result.gameObject.SetActive(true);
        return result;
    }
}
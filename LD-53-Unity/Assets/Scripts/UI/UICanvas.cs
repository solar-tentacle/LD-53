using System;
using UI;
using UnityEngine;

public class UICanvas : MonoBehaviour
{
    [SerializeField] private UIHud _hud;
    [SerializeField] private Transform _healthViewParent;
    [SerializeField] private HealthView _healthViewPrefab;

    public UIHud HUD => _hud;
    
    [SerializeField] private UIWinWindow _uiWinWindow;
    public UIWinWindow UIWinWindow => _uiWinWindow;

    [SerializeField] private UILoseWindow _uiLoseWindow;
    public UILoseWindow UILoseWindow => _uiLoseWindow;

    [SerializeField] private UIEncounterWindow _uiEncounterWindow;
    public UIEncounterWindow UIEncounterWindow => _uiEncounterWindow;
    
    [SerializeField] private UIAllLevelsCompletedWindow _allLevelsCompletedWindow;
    public UIAllLevelsCompletedWindow AllLevelsCompletedWindow => _allLevelsCompletedWindow;

    private void Start()
    {
        _healthViewPrefab.gameObject.SetActive(false);
    }

    public HealthView AddHealthView()
    {
        var result = Instantiate(_healthViewPrefab, _healthViewParent);
        result.gameObject.SetActive(true);
        return result;
    }
}
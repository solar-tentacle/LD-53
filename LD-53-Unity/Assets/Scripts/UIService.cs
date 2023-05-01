using System.Collections.Generic;
using System.Linq;
using UI;
using UnityEngine;

public class UIService : Service, IInject, IUpdate
{
    [SerializeField] private UICanvas _uiCanvas;
    public UICanvas UICanvas => _uiCanvas;
    private readonly List<KeyValuePair<ObjectGridElement, HealthView>> _healthViewsByObjects = new();

    public void Inject()
    {
        _uiCanvas.HUD.Show();
        _uiCanvas.HUD.UICardsHand.Show();
        _uiCanvas.HUD.UIDeckIndicator.Show();
        _uiCanvas.HUD.CoinsPanel.Show();
    }

    public void AddHealthView(ObjectGridElement element, uint health)
    {
        var healthView = _uiCanvas.AddHealthView();
        healthView.SetHealth(health);
        _healthViewsByObjects.Add(new KeyValuePair<ObjectGridElement, HealthView>(element, healthView));
    }
    
    public void UpdatedHealthView(ObjectGridElement element, uint health)
    {
        var healthView = _healthViewsByObjects.FirstOrDefault(h => h.Key == element).Value;
        healthView.SetHealth(health);
    }
    
    public void RemoveHealthView(ObjectGridElement element)
    {
        for (int i = 0; i < _healthViewsByObjects.Count; i++)
        {
            if (_healthViewsByObjects[i].Key == element)
            {
                _healthViewsByObjects[i].Value.gameObject.SetActive(false);
                _healthViewsByObjects.RemoveAt(i);
                return;
            }
        }
    }

    public void UpdateHealthPositions()
    {
        foreach (var kv in _healthViewsByObjects)
        {
            Vector3 playerPos = Camera.main.WorldToScreenPoint(kv.Key.transform.position);
            kv.Value.transform.position = playerPos;
        }
    }

    public void GameUpdate(float delta)
    {
        UpdateHealthPositions();
    }
}
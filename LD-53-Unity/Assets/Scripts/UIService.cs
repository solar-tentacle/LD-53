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
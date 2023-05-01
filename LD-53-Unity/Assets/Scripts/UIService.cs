using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
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
            AnchorPosition(kv.Key.transform, kv.Value.transform);
        }
    }

    private static void AnchorPosition(Transform element, Transform healthView)
    {
        Vector3 elementPosition = Camera.main.WorldToScreenPoint(element.position);
        healthView.position = elementPosition;
    }

    public void GameUpdate(float delta)
    {
        UpdateHealthPositions();
    }

    public void AnimateHealthChange(ObjectGridElement element, int delta)
    {
        var healthChangeAnim = _uiCanvas.SpawnHealthChangeAnim();
        healthChangeAnim.SetHealth(delta);
        AnchorPosition(element.transform, healthChangeAnim.transform);
        var targetPosition = healthChangeAnim.transform.position;
        healthChangeAnim.transform.position = new Vector3(targetPosition.x, targetPosition.y-100.5f, targetPosition.z);
        healthChangeAnim.transform.DOMoveY(targetPosition.y, 1.3f);
        healthChangeAnim.CanvasGroup.DOFade(0, 1.3f).OnComplete(() =>
        {
            Destroy(healthChangeAnim.gameObject);
        });
    }
}
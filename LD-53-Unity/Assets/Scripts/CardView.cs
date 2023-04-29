using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardView : MonoBehaviour
{
    private bool _isSelected;
    private void Start()
    {
        EventTrigger trigger = GetComponent<EventTrigger>();
        
        EventTrigger.Entry up = new EventTrigger.Entry();
        up.eventID = EventTriggerType.PointerDown;
        up.callback.AddListener((data) => { OnPointerDownDelegate((PointerEventData)data); });
        
        EventTrigger.Entry down = new EventTrigger.Entry();
        down.eventID = EventTriggerType.PointerUp;
        down.callback.AddListener((data) => { OnPointerUpDelegate((PointerEventData)data); });

        trigger.triggers.Add(up);
        trigger.triggers.Add(down);
    }

    private void Update()
    {
        if (_isSelected)
        {
            var newPos = Input.mousePosition;
            newPos.z = 0f;
            transform.position = newPos;
        }
    }

    private void OnPointerUpDelegate(PointerEventData data)
    {
        Debug.Log("up");
        _isSelected = false;
    }

    public void OnPointerDownDelegate(PointerEventData data)
    {
        Debug.Log("down");
        _isSelected = true;
    }
}
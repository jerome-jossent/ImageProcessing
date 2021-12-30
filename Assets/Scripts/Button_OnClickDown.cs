using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Button_OnClickDown : MonoBehaviour
{
    public EventTrigger trigger;

    private void Start()
    {
        var eventEntry = trigger.triggers.Find(x => x.eventID == EventTriggerType.Drop);
        if (eventEntry != null)
        {
            eventEntry.callback.AddListener((data) => { OnPointerEventDelegate((PointerEventData)data); });
        }
    }

    private void OnPointerEventDelegate(PointerEventData data)
    {
        throw new NotImplementedException();
    }

    private void OnMouseDown()
    {
        //trigger.Invoke()
    }


}

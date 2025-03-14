using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerZoneCrate : TriggerZone
{
    public void Awake()
    {
        var obj = transform.parent.GetComponent<InteractiveObject>();
        if (obj)
        {
            if (_enterEvent == null) _enterEvent = new UnityEvent();
            if (_exitEvent == null) _exitEvent = new UnityEvent();
            _enterEvent.AddListener(obj.BecomeInteractive);
            _exitEvent.AddListener(obj.StopBeingInteractive);
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        _enterEvent?.Invoke();
    }

    protected override void OnTriggerExit(Collider other)
    {
        _exitEvent?.Invoke();

    }
    protected override void OnDisable()
    {
        _enterEvent.RemoveAllListeners();
        _exitEvent.RemoveAllListeners();
    }
}

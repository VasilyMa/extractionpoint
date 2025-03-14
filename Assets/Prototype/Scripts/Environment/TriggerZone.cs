using UnityEngine.Events;
using UnityEngine;

public abstract class TriggerZone : MonoBehaviour
{
    protected UnityEvent _enterEvent, _exitEvent;
    protected abstract void OnTriggerEnter(Collider other);

    protected abstract void OnTriggerExit(Collider other);

    protected abstract void OnDisable();
}

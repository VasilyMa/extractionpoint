using UnityEngine;
using UnityEngine.Events;


public class UnitDetector : MonoBehaviour
{
    UnitMB _unitMB;
    private void Awake()
    {
        _unitMB = transform.parent.GetComponent<UnitMB>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3 || other.gameObject.layer == 7)
        {
        }

    }

    private void OnTriggerExit(Collider other)
    {

    }

    private void OnDisable()
    {

    }
}

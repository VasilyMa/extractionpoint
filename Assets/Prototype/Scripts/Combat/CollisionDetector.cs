using UnityEngine;
[RequireComponent(typeof(SphereCollider))]

public class CollisionDetector : MonoBehaviour
{
    private int _layerOfParent;
    private UnitMB _unitMB;
    private SphereCollider _triggerCollider;
    [SerializeField]private float _triggerRadius;
    /*private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.layer == _unitMB.TargetLayer)
        {
            _unitMB.SetTarget(null);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == _unitMB.TargetLayer)
        {
            _unitMB.SetTarget(other.transform);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == _unitMB.TargetLayer)
        {
            _unitMB.SetTarget(other.transform);
        }
    }
    private void OnValidate()
    {
        if (_triggerCollider == null)
        {
            _triggerCollider = gameObject.GetComponent<SphereCollider>();
        }
        _triggerCollider.radius = _triggerRadius;
        _triggerCollider.isTrigger = true;
        _layerOfParent = transform.parent.gameObject.layer;
        _unitMB = transform.parent.GetComponent<UnitMB>();  
    }*/
}

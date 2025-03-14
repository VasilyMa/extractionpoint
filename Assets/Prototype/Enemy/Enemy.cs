using UnityEngine;

public class Enemy : MonoBehaviour
{
    Collider[] _childColliders;
    Rigidbody[] _childRigidbodys;
    Collider _selfCollider;
    Rigidbody _selfRigidbody;
    Transform _player;
    Animator _animator;
    public bool IsActiveRagdoll;
    [SerializeField] int _health;
    void Awake()
    {
        InitRagdoll();
        _selfCollider = GetComponent<Collider>();
        _selfRigidbody = GetComponent<Rigidbody>();    
        _player = GameObject.FindAnyObjectByType<PlayerMovementController>().transform;
        _animator = GetComponent<Animator>(); 
        ActiveRagdoll(false);
    }

    void Update()
    {
        if(_selfCollider.isTrigger == false)transform.LookAt(_player);

        if (IsActiveRagdoll)
        {
            ActiveRagdoll(true);
            IsActiveRagdoll = false;
        }
    }
    void InitRagdoll()
    {
        _childColliders = transform.GetComponentsInChildren<Collider>();
        _childRigidbodys = transform.GetComponentsInChildren<Rigidbody>();
    }
    public void ActiveRagdoll(bool flag)
    {
        foreach (Collider collider in _childColliders)
        {
            collider.isTrigger = !flag;
        }
        foreach (Rigidbody rigidbody in _childRigidbodys)
        {
            rigidbody.isKinematic = !flag;
            if(flag)rigidbody.AddExplosionForce(20, transform.forward, 5);
        }
        _animator.enabled = !flag;
        _selfCollider.isTrigger = flag;
        _selfRigidbody.isKinematic = flag;
    }
    public void TakeDamage(float damage)
    {
        _health -= (int)damage;
        if (_health <= 0)
        {
            _health = 0;
            ActiveRagdoll(true);
        }
    }
    public void TakeImpulse(int hitImpulse,Vector3 impulseDirection)
    {
        var finalDir = (transform.position + impulseDirection).normalized;
        finalDir.y = 0;
        _selfRigidbody.AddForce(finalDir * hitImpulse, ForceMode.VelocityChange);
    }

    private void OnValidate()
    {
        foreach (CharacterJoint characterJoint in GetComponentsInChildren<CharacterJoint>())
        {
            characterJoint.enableProjection = true;
            characterJoint.enablePreprocessing = true;
        }
    }
}

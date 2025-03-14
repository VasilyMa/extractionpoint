using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public abstract class SourceMissile : MonoBehaviour, IPool
{
    [ReadOnlyInspector] public string KEY_ID; 
    protected bool isAvaible;
    protected bool isRun;
    protected ParticleSystem[] particles;
    protected TrailRenderer[] trails;
    public GameObject ThisGameObject => gameObject;
    public bool IsAvaiable { get => isAvaible; set => isAvaible = value; }
    public string PoolKeyID => KEY_ID;

    protected Weapon _sourceWeapon;
    protected float _missileSpeed;

    [SerializeField] protected TrailRenderer _trail;
    [HideInInspector] public float MissileSpeed;
    [HideInInspector] public float Distance;
    [HideInInspector] public float Damage;
    protected ParticleSystem _particleSystem;

    protected virtual void Awake()
    {
        
    }

    protected virtual void OnValidate()
    {
        if (this != null)
        {
            KEY_ID = this.gameObject.name;
        }
    }
    public virtual void Invoke(WeaponPlayer sourceWeapon, Vector3 startPos, Vector3 direction)
    {
        if (particles != null)
        {
            for (int i = 0; i < particles.Length; i++)
            {
                particles[i].Stop();
                particles[i].Clear();
            }
        }
        else
        {
            particles = GetComponentsInChildren<ParticleSystem>();
        }

        if (trails != null)
        {
            for (int i = 0; i < trails.Length; i++)
            {
                trails[i].Clear();
            }
        }
        else
        {
            trails = GetComponentsInChildren<TrailRenderer>();
        }
    }
    public abstract bool Run();
    
    public abstract void OnTriggerEnter(Collider collider);

    public void InitPool()
    {

    }
    public virtual void Dispose()
    {
        isRun = false;
        ReturnToPool();
    }
    public virtual void ReturnToPool() =>PoolModule.Instance.ReturnToPool(this);
}

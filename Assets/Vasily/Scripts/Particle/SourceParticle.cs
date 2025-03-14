using System.Collections;
using UnityEngine;

public class SourceParticle : MonoBehaviour, IPool
{
    [ReadOnlyInspector] public string KEY_ID;
    [SerializeField][ReadOnlyInspector] protected ParticleSystem particleSystem;
    public GameObject ThisGameObject => gameObject;
    protected bool isAvaiable;
    public bool IsAvaiable { get => isAvaiable; set => isAvaiable = value; }

    public string PoolKeyID => KEY_ID;

    protected float _timer;

    protected virtual void OnValidate()
    {
        if (this != null)
        {
            KEY_ID = this.gameObject.name;
        }
    }
    public virtual void InitPool()
    {

    }

    public virtual void Invoke(Vector3 pos)
    {
        this.transform.position = pos;

        if(particleSystem == null) particleSystem = GetComponent<ParticleSystem>();

        _timer = particleSystem.main.duration + 1f;

        gameObject.SetActive(true);

        VFXSystem.Instance.Add(this);
    }

    public virtual bool Run()
    {
        _timer -= Time.deltaTime;

        if (_timer <= 0)
        {
            ReturnToPool();
            return false;
        }

        return true;
    } 

    public void ReturnToPool()
    {
        PoolModule.Instance.ReturnToPool(this);
    }
}

using System.Collections;
using UnityEngine;

public class SourceSound : MonoBehaviour, IPool
{
    [ReadOnlyInspector] public string KEY_ID;
    private AudioSource _audioSource;
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
    public void InitPool()
    {

    }

    public void Invoke(Vector3 pos)
    {
        if(_audioSource == null) _audioSource = gameObject.GetComponent<AudioSource>();
        transform.position = pos;

        _timer = _audioSource.clip.length + 1f;

        gameObject.SetActive(true);

        SoundSystem.Instance.Add(this);
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

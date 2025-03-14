using System.Collections;
using UnityEngine;
[RequireComponent(typeof(SphereCollider))]
public abstract class Projectile : MonoBehaviour, IPool
{
    [ReadOnlyInspector] public string KeyNameID;
    protected int _damage;
    protected int _hitImpulse;
    protected Coroutine _coroutine;
    protected bool isAvaiable;

    public GameObject ThisGameObject => gameObject;

    public bool IsAvaiable { get => isAvaiable; set => isAvaiable = value; }

    public string PoolKeyID => KeyNameID;

    public abstract void Invoke(Transform movable, Vector3 start, Vector3 end, float speed, float damage, float hitImpulse);
    protected virtual IEnumerator MoveProjectile(Transform movable, Vector3 start, Vector3 end, float speed)
    {
        movable.SetPositionAndRotation(start, Quaternion.Euler(0, 0, 0));
        movable.rotation = Quaternion.LookRotation(end, Vector3.up);
        Vector3 direction = (end - start).normalized;
        float elapsedTIme = 0f;
        while (elapsedTIme < 1f)
        {
            elapsedTIme += Time.deltaTime;  
            movable.position += direction * Time.deltaTime * speed;
            yield return null;
        }
        Dispose();
    }
    protected virtual void OnValidate()
    {
        GetComponent<SphereCollider>().isTrigger = true;
        if (gameObject.layer == 6) return;
        gameObject.layer = 6;
    }
    public virtual void Dispose()
    {
        if (_coroutine != null) StopCoroutine(_coroutine);

        isAvaiable = true;
        gameObject.SetActive(false);
    }

    public void ReturnToPool()
    {
        PoolModule.Instance.ReturnToPool(this);
    }

    public void InitPool()
    {

    }
}

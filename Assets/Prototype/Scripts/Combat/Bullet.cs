using UnityEngine;
public class Bullet : Projectile
{
    public override void Invoke(Transform movable, Vector3 start, Vector3 end, float speed, float damage, float hitImpulse)
    {
        gameObject.SetActive(true);
        _coroutine = StartCoroutine(MoveProjectile(movable, start, end, speed));
        _damage = (int)damage;
        _hitImpulse = (int)hitImpulse;  
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            enemy.TakeDamage(_damage);
            enemy.TakeImpulse(_hitImpulse, (other.transform.position - transform.position).normalized);
            Dispose();
        }
    }

}

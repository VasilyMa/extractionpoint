using UnityEngine;
using Stat.WeaponStat;
public class LightMissile : SourceMissile
{
    [SerializeField]private SourceParticle _impactParticleHit;
    [SerializeField]private SourceParticle _impactParticleEnemyHit;
    [SerializeField]private SourceSound _impactSoundHit;
    [SerializeField]private SourceSound _impactSoundEnemyHit;
    private float _distanceElapsed;
    protected float _distance;

    public override void Invoke(WeaponPlayer sourceWeapon, Vector3 startPos, Vector3 targetPos)
    {
        base.Invoke(sourceWeapon, startPos, targetPos);

        _sourceWeapon = sourceWeapon;

        transform.position = startPos;
        _distance = sourceWeapon.GetStat<Distance>().ResloveValue;
        _missileSpeed = sourceWeapon.GetStat<MissileSpeed>().ResloveValue;

        _distanceElapsed = 0;

        transform.rotation = Quaternion.LookRotation(targetPos - startPos);

        MissileSystem.Instance.Add(this);

        isRun = true;

        gameObject.SetActive(true);
    }

    public override bool Run()
    {
        if (isRun)
        {
            transform.position += transform.forward * Time.deltaTime * _missileSpeed;

            _distanceElapsed += Time.deltaTime * _missileSpeed;

            if (_distanceElapsed >= _distance)
            {
                Dispose();
            }

            return true;
        }

        Dispose();
        return false;

    }

    public override void OnTriggerEnter(Collider collider)
    {
        Vector3 contactPoint = collider.ClosestPoint(transform.position);

        if (OnlineState.Instance.TryGetEnemy(collider.name, out PhotonEnemy enemy))
        {
            PoolModule.Instance.GetFromPool<SourceParticle>(_impactParticleEnemyHit).Invoke(contactPoint);
            PoolModule.Instance.GetFromPool<SourceSound>(_impactSoundEnemyHit).Invoke(contactPoint);
            enemy.AddTakeDamage(_sourceWeapon);
            Dispose();
        }
        else 
        {
            PoolModule.Instance.GetFromPool<SourceParticle>(_impactParticleHit).Invoke(contactPoint);
            PoolModule.Instance.GetFromPool<SourceSound>(_impactSoundHit).Invoke(contactPoint);
            Dispose();
        }
    }
}

using UnityEngine;

public class LauncherMissile : SourceMissile
{
    [SerializeField] private AoeParticle _aoeParticleRef;
    [SerializeField] private SourceParticle _impactParticleHit;
    [SerializeField] private SourceParticle _impactParticleEnemyHit;
    [SerializeField] private SourceSound _impactSoundHit;
    [SerializeField] private SourceSound _impactSoundEnemyHit;

    private Vector3 _startPosition;
    private Vector3 _endPosition;
    private Vector3 p2;
    private Vector3 p3;
    private Collider[] colliders = new Collider[20];

    private float t;
    private float t_target;
    [HideInInspector] public float Radius;

     
    public override void Invoke(WeaponPlayer weapon, Vector3 startPosition, Vector3 targetPosition)
    {
        base.Invoke(weapon, startPosition, targetPosition);

        _endPosition = targetPosition;
        _startPosition = startPosition;

        transform.position = _startPosition;

        var dis = Vector3.Distance(_startPosition, _endPosition);

        t = 0;
        t_target = dis / MissileSpeed;

        float multi = Random.Range(2.5f, 5f);
        p2 = _startPosition + Vector3.up * multi;
        p3 = _endPosition + Vector3.up * multi;

        gameObject.SetActive(true);

        if (_trail != null) _trail.enabled = true;

        MissileSystem.Instance.Add(this);
    }

    public override bool Run()
    {
        if (t > t_target)
        {
            Finish();
            return false;
        }

        t += Time.deltaTime /** t_multiply * 4*/;
        Vector3 targetPos = Ballistic(_startPosition, p2, p3, _endPosition, t / t_target);
        transform.LookAt(targetPos);
        transform.position = targetPos;

        return true;
    }

    Vector3 Ballistic(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, float t)
    {
        Vector3 p12 = Vector3.Lerp(p1, p2, t);
        Vector3 p23 = Vector3.Lerp(p2, p3, t);
        Vector3 p34 = Vector3.Lerp(p3, p4, t);
        Vector3 p123 = Vector3.Lerp(p12, p23, t);
        Vector3 p234 = Vector3.Lerp(p23, p34, t);
        return Vector3.Lerp(p123, p234, t);
    }

    void Finish()
    {
        Physics.OverlapSphereNonAlloc(transform.position, Radius, colliders, layerMask: LayerMask.GetMask("Enemy"));

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i] is null) continue;

            if (OnlineState.Instance.TryGetEnemy<PhotonEnemy>(colliders[i].gameObject.name, out var enemy))
            {
                Vector3 contactPoint = colliders[i].ClosestPoint(transform.position);
                PoolModule.Instance.GetFromPool<SourceParticle>(_impactParticleEnemyHit).Invoke(contactPoint);
                PoolModule.Instance.GetFromPool<SourceSound>(_impactSoundEnemyHit).Invoke(contactPoint);
                enemy.AddTakeDamage(_sourceWeapon);
            }
        }

        var aoeParticle = PoolModule.Instance.GetFromPool<AoeParticle>(_aoeParticleRef);
        aoeParticle.Invoke(transform.position);

        Dispose();
    }

    public override void Dispose()
    {
        base.Dispose();

        for (int i = 0; i < colliders.Length; i++) colliders[i] = null;
    }

    public override void OnTriggerEnter(Collider collider)
    {

    }
}

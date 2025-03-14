using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class WeaponController : MonoBehaviour
{
   /* private Weapon _weapon;
    private Transform _target;
    private Transform _firePoint;
    private float _nextFireTime; // Время, когда можно произвести следующий выстрел
    private Transform right_hand_pos;
    private Transform left_hand_pos;
    private Transform RightHandIK;
    private Transform LeftHandIK;

    private Animator _animator;

    private ParticleSystem _gilzaParticleSystem;
    
    [SerializeField] private bool _isFire;

    // Настройки отдачи
    [SerializeField] private float recoilDistance = 0.2f; // Сколько смещать назад
    [SerializeField] private float recoilSpeed = 5f; // Скорость возвращения
    private Vector3 originalPosition; // Исходная позиция оружия
    private Vector3 recoilPosition; // Позиция с учетом отдачи
    private bool isRecoil; // Флаг, указывающий на то, происходит ли отдача
    private bool isReload; // Флаг, указывающий на то, происходит ли отдача
    private int _ammoSize; // Флаг, указывающий на то, происходит ли отдача


    private Transform _weaponMagazine;
    private Transform _instantiatedWeaponMagazine;
    private Rigidbody _instantiatedWeaponMagazineRigidbody;
    private BoxCollider _instantiatedWeaponMagazineBoxCollider;
    public void Init()
    {
        var weapon = GameObject.Instantiate(_weapon.weapon, transform);
        transform.position = _weapon.WeaponHandlerPosition;

        right_hand_pos = transform.GetChild(0).Find("right_hand_pos");
        left_hand_pos = transform.GetChild(0).Find("left_hand_pos");

        RightHandIK = transform.parent.Find("RigLayer").Find("RightHandIK");
        LeftHandIK = transform.parent.Find("RigLayer").Find("LeftHandIK");

        RightHandIK.GetComponent<TwoBoneIKConstraint>().data.target = right_hand_pos;
        LeftHandIK.GetComponent<TwoBoneIKConstraint>().data.target = left_hand_pos;
        LeftHandIK.GetComponent<TwoBoneIKConstraint>().data.targetPositionWeight = 1;

        transform.parent.GetComponent<RigBuilder>().Build();
        _gilzaParticleSystem = transform.parent.transform.Find("GilzaParticleSystem").GetComponent<ParticleSystem>();


        
        originalPosition = transform.localPosition; // Сохраним исходное положение
        recoilPosition = originalPosition - transform.forward * recoilDistance; // Расчёт позиции отдачи


        _weaponMagazine = transform.GetChild(0).Find("Magazine");
        _instantiatedWeaponMagazine = GameObject.Instantiate(_weaponMagazine, _weaponMagazine.position, _weaponMagazine.rotation, _weaponMagazine);
        _instantiatedWeaponMagazineRigidbody = _instantiatedWeaponMagazine.AddComponent<Rigidbody>();
        _instantiatedWeaponMagazineRigidbody.isKinematic = true;
        _instantiatedWeaponMagazineRigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
        _instantiatedWeaponMagazineBoxCollider = _instantiatedWeaponMagazine.AddComponent<BoxCollider>();
        _instantiatedWeaponMagazine.gameObject.SetActive(false);
        _instantiatedWeaponMagazineBoxCollider.isTrigger = true;
        _instantiatedWeaponMagazine.tag = "Respawn";
        _ammoSize = _weapon.AmmoSize;


        _animator = transform.parent.GetComponent<Animator>();    
    }

    public void Update()
    {
        // Управление отдачей
        if (isRecoil)
        {
            // Плавно возвращаем оружие на исходную позицию
            transform.localPosition = Vector3.Lerp(transform.localPosition, originalPosition, recoilSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.localPosition, originalPosition) < 0.01f)
            {
                transform.localPosition = originalPosition; // Окончательное возврат на исходную позицию
                isRecoil = false; // Сброс флага отдачи
            }
        }
        // Проверка на стрельбу
        if (_isFire && Time.time >= _nextFireTime)
        {
            if (_ammoSize <= 0 && !isReload)
            {
                ViewReload();
            }
            else if(_ammoSize > 0)
            {
                _ammoSize--;
                Shoot();
                _nextFireTime = Time.time + 60f / _weapon.RateOfFire;
                _gilzaParticleSystem.Emit(1);
            }
           
        }

        if (!_firePoint)
        {
            _firePoint = GetComponentInChildren<FirePoint>().gameObject.transform;
        }
    }

    public void SetWeapon(Weapon weapon)
    {
        _weapon = weapon;
    }

    public void SetTarget(Transform target)
    {
        _target = target;
    }

    public void SetFire(bool flag)
    {
        _isFire = flag;
    }
    public void CompleteReload()
    {
        isReload = false;
        _ammoSize = _weapon.AmmoSize;
        _weaponMagazine.gameObject.SetActive(true);
        _animator.SetBool("IsReload", false);
        _animator.SetBool("IsCombat", false);
    }

    public void Shoot()
    {
        var projectile = PoolModule.Instance.GetFromPool<Bullet>(_weapon.Projectile);

        if (_target)
        {
            projectile.Invoke(projectile.transform, _firePoint.position, _target.position + Vector3.up, _weapon.HitImpulse, _weapon.Damage, _weapon.HitImpulse);
            Debug.DrawLine(_firePoint.position, _target.position + Vector3.up, Color.red, 10);
            isRecoil = true; // Активируем отдачу


        }
        else
        {
            projectile.Invoke(projectile.transform, _firePoint.position, _firePoint.position + _firePoint.forward, _weapon.HitImpulse, _weapon.Damage, _weapon.HitImpulse);
            isRecoil = true; // Активируем отдачу
        }
    }
    public void ViewReload()
    {
        isReload = true;
        _animator.SetBool("IsReload", true);
        _animator.SetBool("IsCombat", false);
        _weaponMagazine.gameObject.SetActive(false);
        _instantiatedWeaponMagazine.SetParent(null);
        _instantiatedWeaponMagazine.gameObject.SetActive(true);
        _instantiatedWeaponMagazineRigidbody.isKinematic = false;
        _instantiatedWeaponMagazineBoxCollider.isTrigger = false;
        StartCoroutine(MagazineReset(_instantiatedWeaponMagazine.gameObject));
    }
    public IEnumerator MagazineReset(GameObject magazine)
    {

        float elapsedTIme = 0f;
        while (elapsedTIme < 3f)
        {
            elapsedTIme += Time.deltaTime;
            yield return null;
        }
        magazine.gameObject.SetActive(false);
        _instantiatedWeaponMagazineRigidbody.isKinematic = true;
        _instantiatedWeaponMagazineBoxCollider.isTrigger = true;
        _instantiatedWeaponMagazine.position = _weaponMagazine.transform.position;  
        _instantiatedWeaponMagazine.rotation = _weaponMagazine.transform.rotation;
        _instantiatedWeaponMagazine.parent = _weaponMagazine; 
    }*/
}

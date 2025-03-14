using System;
using System.Collections.Generic;
using System.Collections;

using Photon.Pun;

using UnityEngine;
using UnityEngine.AI;

using Stat.UnitStat;
using Stat.WeaponStat;

public class PhotonEnemy : PhotonUnit, IPunInstantiateMagicCallback
{
    [SerializeReference] public List<IUnitBehaviour> SourceBehaviours;

    [SerializeField] protected WeaponBase _enemyWeapon;

    protected IUnitBehaviour _unitBehaviour;
    protected Dictionary<Type, IUnitBehaviour> _unitBehaviourMap = new Dictionary<Type, IUnitBehaviour>();
    protected PhotonUnit _target;
    protected NavMeshAgent _agent;
    protected Collider _collider;
    protected Collider[] _collidersRD;
    protected Rigidbody[] _rigidbodiesRD;
    protected AnimateFeature _animFeature;
    protected int _lastAttackPhotonViewID;
    protected bool isDie;
    protected bool isAvaiable;

    protected EnemyHealthbar _healthbar;

    public override PhotonUnit Target => _target;
    public override bool IsDie => isDie;
    public bool IsAvaiable => isAvaiable;

    public virtual void Invoke(Vector3 spawnPos)
    {
        var target = GetNearestTarget();

        if (target != null)
        {
            if (NavMesh.SamplePosition(spawnPos, out var navMeshPos, 5, NavMesh.AllAreas))
            {
                SendAnyPositionData spawnSendData = new SendAnyPositionData(navMeshPos.position.x, navMeshPos.position.y, navMeshPos.position.z, target.photonView.ViewID);

                photonView.RPC("SendRPCInvoke", RpcTarget.AllViaServer, PhotonRunHandler.SerializeObject(spawnSendData));

                isAvaiable = false;
            }
        }
        else
        {
           isAvaiable = true;
        }
    }

    public IUnitBehaviour GetCurrentBehaviour()
    {
        return _unitBehaviour;
    }
    public override void Init()
    {
        InitRagdoll();

        _agent = GetComponent<NavMeshAgent>();

        foreach (var behaviour in SourceBehaviours)
        {
            var type = behaviour.GetType();
            _unitBehaviourMap.Add(type, behaviour.Init(this));
        }

        SetBehaviour(GetBehaviour<IdleBehaviour>());
        _mainWeapon = _enemyWeapon.GetEquip<WeaponEnemy>();

        _mainWeapon.WeaponOwner = this;
        _mainWeapon.Init();
        _animFeature = GetComponentInChildren<AnimateFeature>().Init(this);

        _healthbar = GetComponentInChildren<EnemyHealthbar>();
        _healthbar.Init(this);

        isAvaiable = true;

        gameObject.SetActive(false);
    }

    public override void ActionAttack()
    {
        if (_target == null) return;

        if (_mainWeapon.RequestAction()) _mainWeapon.Action(); 
    }

    public override void Run()
    {
        _unitBehaviour?.Run();
        _healthbar?.Run();
    }

    public virtual void SetBehaviour(IUnitBehaviour behaviour)
    {
        if (_unitBehaviour != null) _unitBehaviour.Exit();

        if (behaviour != null)
        {
            behaviour.Enter();

            _unitBehaviour = behaviour;
        }
    }
    public virtual IUnitBehaviour GetBehaviour<T>()
    {
        if (_unitBehaviourMap[typeof(T)] is not null)
        {
            return _unitBehaviourMap[typeof(T)];
        }

        return null;
    }
    public override void Dispose()
    {
        _healthbar.Dispose();
    }

    public override void FixedRun()
    {

    }

    public float GetDistanceToTarget()
    {
        return Vector3.Distance(transform.position, _target.transform.position);
    }

    public bool MoveDestination()
    {
        if (_target == null) return false;

        _agent.speed = _moveStat.ResloveValue;
        _agent.SetDestination(_target.transform.position);

        return true;
    }

    public bool IsTarget()
    {
        if (_target) return true;
        return false;
    }

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        if (!OnlineState.Instance.AllEnemyInGame.ContainsKey(info.photonView.ViewID.ToString()))
        {
            OnlineState.Instance.AllEnemyInGame.Add(info.photonView.ViewID.ToString(), this);
        }

        var recieveData = info.photonView.InstantiationData;

        Vector3 startPos = new Vector3((float)recieveData[0], (float)recieveData[1], (float)recieveData[2]);

        transform.position = startPos;

        _stateTracker = new StateTracker();
        _healthStat = new Health(BaseHealthValue);
        _moveStat = new MoveSpeed(BaseMoveSpeedValue, MoveSpeedhMinMax.Min, MoveSpeedhMinMax.Max);

        gameObject.name = info.photonView.ViewID.ToString();

        UnitSystem.Instance.AddUnitInPool(this);
    }

    public PhotonUnit GetNearestTarget()
    {
        _target = null;

        var players = PhotonRunHandler.Instance.Players;

        float closestDistance = Mathf.Infinity;

        foreach (var player in players.Values)
        {
            if (player == null) continue;

            if (player.IsDie) continue;

            float distanceToTarget = Vector3.Distance(transform.position, player.transform.position);

            if (distanceToTarget < closestDistance)
            {
                closestDistance = distanceToTarget;
                _target = player;
            }
        }

        return _target;
    }

    public void UpdateTarget(PhotonUnit target)
    {
        _target = target;
    }

    public override void Die(float impulse, int senderViewID)
    {
        if (IsDie) return;

        SetBehaviour(GetBehaviour<DeadBehaviour>());

        if (PhotonRunHandler.Instance.TryGetPlayer(senderViewID, out var player))
        {
            Vector3 dir = (transform.position - player.transform.position).normalized;

            _rigidbodiesRD[0].AddForce(impulse/5 * dir, ForceMode.Impulse);

            Debug.Log("Ragdol activate");
        }

        State.Instance.RunCoroutine(WaitOnDisable(), onAvaiable);
    }

    public override void AddTakeDamage(Weapon attackingWeapon)
    {
        if (attackingWeapon is WeaponPlayer weaponPlayer)
        {
            if (weaponPlayer.WeaponOwner.photonView.IsMine)
            {
                _lastAttackPhotonViewID = weaponPlayer.WeaponOwner.photonView.ViewID;

                photonView.RPC("SendRPCDamage", RpcTarget.AllViaServer, PhotonRunHandler.SerializeObject(new SendDamageData(weaponPlayer.GetStat<Attack>().ResloveValue, weaponPlayer.GetStat<Impulse>().ResloveValue, _lastAttackPhotonViewID)));

            }
        }
    }

    void OnTakeDamage(SendDamageData value)
    {
        if (_healthStat.Sub(value.DamageValue) <= 0) Die(value.ImpulseValue, value.SenderID); 
    }

    public override void UpdateActualyHealthValue(float actualyCurrentHealth)
    {
        _healthStat.SetActualyCurrentValue(actualyCurrentHealth);
    }

    public void SetRagdoll(bool activate)
    {
        for (int i = 0; i < _rigidbodiesRD.Length; i++)
        { _rigidbodiesRD[i].isKinematic = !activate; }
        for (int i = 0; i < _rigidbodiesRD.Length; i++)
        { _collidersRD[i].isTrigger = !activate; }
        _collider.isTrigger = activate;
    }
    public void InitRagdoll()
    {
        _rigidbodiesRD = GetComponentsInChildren<Rigidbody>();
        _collidersRD = GetComponentsInChildren<Collider>();
        _collider = GetComponent<Collider>();
        SetRagdoll(false);
    }

    IEnumerator WaitOnDisable()
    {
        yield return new WaitForSeconds(1.0f);
    }

    void onAvaiable()
    {
        gameObject.SetActive(false);

        UnitSystem.Instance.ReturnUnit(this);

        isAvaiable = true;
    }

    #region RPC

    [PunRPC]
    public void SendRPCInvoke(byte[] reciveData)
    {
        SendAnyPositionData syncData = (SendAnyPositionData)PhotonRunHandler.DeserializeObject(reciveData);

        transform.position = new Vector3(syncData.StartPosX, syncData.StartPosY, syncData.StartPosZ);

        if (PhotonRunHandler.Instance.TryGetPlayer(syncData.Target, out var photonPlayer))
        {
            _target = photonPlayer;

            if (_target.IsDie) return;

            isAvaiable = false;

            _healthStat.Init();
            _moveStat.Init();
            _healthbar?.Invoke();

            gameObject.SetActive(true);

            _agent.enabled = true;


            UnitSystem.Instance.RegisterActiveUnit(this);

            SetBehaviour(GetBehaviour<MoveBehaviour>());
        }
    }
    [PunRPC]
    public void SendRPCDamage(byte[] reciveData) => OnTakeDamage((SendDamageData)PhotonRunHandler.DeserializeObject(reciveData));
    #endregion
} 
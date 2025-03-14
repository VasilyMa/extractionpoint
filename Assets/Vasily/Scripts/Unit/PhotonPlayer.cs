using System;

using Photon.Pun;
using UnityEngine;
using UnityEngine.Animations.Rigging;

using Stat.UnitStat;
using PlayFab.ClientModels;
using PlayFab;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;

[RequireComponent(typeof(PhotonTransformViewClassic), typeof(PhotonAnimatorView))]
public class PhotonPlayer : PhotonUnit, IPunInstantiateMagicCallback
{
    public Action<WeaponPlayer> OnMainWeaponChange;

    public string PlayerNickName;
    public string RankID;

    public bool IsInitAndReady;
    public EquipmentContainer EquipmentContainer;
    public Transform FirePoint;
    public Transform AimTarget;

    protected Animator _animator;
    protected FloatingJoystick _floatingJoystickMove;
    protected FloatingJoystick _floatingJoystickLook;
    protected SendAnyPositionData _sendShootData;
    protected WeaponView _weaponView;


    /*protected RigBuilder _rigBuilder;//VLAD Foo
    protected Rig _rigHand;//VLAD Foo
    protected TwoBoneIKConstraint _rightHandIK;//VLAD Foo
    protected TwoBoneIKConstraint _leftHandIK;//VLAD Foo
    public Transform handR;*/
    [Header("Rig")]
    private RigBuilder _rigBuilder;
    public Rig RigAim;
    public Rig RigRecoil;
    public Rig RigRun;
    public Transform WeaponHandlerTransform;
    public float RotationSpeed;
    private Vector3 lastTargetPos;

    [Header("HandsTargets")]
    public Transform rightHandTarget;
    public Transform leftHandTarget;

    [Header("Recoil")]
    public Transform hand; // Ссылка на трансформ руки (Hand_R)
    public float recoilAmount = 0.2f; // Сила отдачи по оси Y
    public float recoilXOffset = 0.1f; // Сила отдачи по оси X
    public float recoilDuration = 0.1f; // Длительность отдачи
    public float returnSpeed = 5f; // Скорость возврата руки

    private Vector3 initialPosition; // Исходная позиция руки
    private float recoilTimer = 0f;
    private CinemachineImpulseSource CinemachineImpulseSource;
    private Vector3 _lookInput;
    public ParticleSystem ParticleFoot;

    protected bool isDie;

    public SendAnyPositionData ShootData { get => _sendShootData; }

    public override PhotonUnit Target => throw new NotImplementedException();
    public override bool IsDie => isDie;

    [SerializeReference] protected IPhotonUnitBehaviour _unitBehaviour;
    [SerializeReference] protected Dictionary<Type, IPhotonUnitBehaviour> _unitBehaviourMap = new Dictionary<Type, IPhotonUnitBehaviour>();

    protected TestEquipmentContainer testEquipmentContainer;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _rigBuilder = GetComponent<RigBuilder>();
        _rigBuilder.Build();
        //VLAD Foo
        /*_rigBuilder = GetComponent<RigBuilder>();
        _rigHand = transform.Find("RigLayer").GetComponent<Rig>();
        _rightHandIK = transform.Find("RigLayer").Find("RightHandIK").GetComponent<TwoBoneIKConstraint>();
        _leftHandIK = transform.Find("RigLayer").Find("LeftHandIK").GetComponent<TwoBoneIKConstraint>();*/
    }

    //VLAD Foo
    public void SetRigBuilder(Transform rightHandTransform, Transform leftHandTransform)
    {
        /*_rightHandIK.data.target = rightHandTransform;
        _leftHandIK.data.target = leftHandTransform;
        _rigBuilder.Build();*/
    }
    public void SetHandReload(Transform transform, float handRigWeight, bool isReload)
    {
        /*_rigHand.weight = handRigWeight;
        _weaponView.transform.parent = transform;
        _weaponView.transform.rotation = Quaternion.identity;
        _animator.SetBool("IsReload", isReload);*/
        /* _animator.SetBool("IsReload", isReload);*/
        /*_leftHandIK.weight = handRigWeight;*/
    }
    public void SetAnimationFloat(float value, string floatName)
    {
        _animator.SetFloat(floatName, value);
    }
    public void MagazineViewActivate(bool activate)
    {
        _weaponView.Magazine.localPosition = _weaponView.MagazineOriginalLocalPosition;
        _weaponView.MagazineRigidbody.isKinematic = activate;
        if (!activate) { _weaponView.Magazine.parent = null; }
        else _weaponView.Magazine.parent = _weaponView.transform;

    }
    public void SetAnimationTrigger(string triggerName)
    {
        _animator.SetTrigger(triggerName);
    }
    public void SetAnimationBool(string boolName,bool value)
    {
        _animator.SetBool(boolName, value);
    }
    public void SetAnimationSpeed(float animSpeed)
    {
        _animator.speed = animSpeed;
    }
    public void FootL()
    {
        ParticleFoot.Emit(1);
    }
    public void FootR()
    {
        ParticleFoot.Emit(1);
    }
    public void PlayReloadSound()
    {
        _weaponView.ReloadAudioSource.PlayOneShot(_weaponView.ReloadAudioSource.clip);
    }
    public override void ActionAttack()
    {
        if (IsBusy) return;

        if (_mainWeapon.RequestAction())
        {
            _weaponView.GilzaParticle.Emit(1);//VLAD Foo
            _weaponView.ShootAudioSource.pitch = UnityEngine.Random.Range(0.95f, 1.05f);
            _weaponView.ShootAudioSource.PlayOneShot(_weaponView.ShootAudioSource.clip);
            recoilTimer = recoilDuration; // Начинаем отсчет отдачи

            if(photonView.IsMine) CinemachineImpulseSource.GenerateImpulse(_weaponView.RecoilCameraShakeStrengh);

            _sendShootData = new SendAnyPositionData(lastTargetPos.x, lastTargetPos.y, lastTargetPos.z, 0);

            byte[] sendData = PhotonRunHandler.SerializeObject(_sendShootData);

            photonView.RPC("SendRPCShoot", RpcTarget.AllViaServer, sendData);

            _mainWeapon.Action();
        }
    }
    public void SwitchWeapon()
    {
        if (IsBusy) return;

        photonView.RPC("SendRPCSwitchWeapon", RpcTarget.AllViaServer);
    }
    public virtual void SetBehaviour(IPhotonUnitBehaviour behaviour)
    {
        if (_unitBehaviour != null) _unitBehaviour.OnExit();

        if (behaviour != null)
        {
            behaviour.OnEnter();

            _unitBehaviour = behaviour;
        }
    }
    public virtual IPhotonUnitBehaviour GetBehaviour<T>()
    {
        if (_unitBehaviourMap[typeof(T)] is not null)
        {
            return _unitBehaviourMap[typeof(T)];
        }

        return null;
    }

    public override void Run()
    {
        if (IsDie) return;

        _unitBehaviour?.OnRun();

        if (!photonView.IsMine)
        {
            AimTarget.position = lastTargetPos;
            return;
        }

        if (_floatingJoystickLook.Vertical != 0 || _floatingJoystickLook.Horizontal != 0)
        {
            Debug.Log("PhotonPlayer JoysticSender");
            Vector3 _lookInput = new Vector3(_floatingJoystickLook.Horizontal, 0, _floatingJoystickLook.Vertical);
            AimTarget.position = transform.position + _lookInput.normalized * 10;
            
            AimTarget.position = new Vector3(AimTarget.position.x, 1, AimTarget.position.z);

            lastTargetPos = AimTarget.position;

            if (_lookInput.magnitude > 0.9f && !IsBusy)
            {
                /*SetAnimationBool("IsCombat", true);
                RigRun.weight = 0;
                RigRecoil.weight = 1;
                RigAim.weight = 1;*/
                if (_unitBehaviour is not ActionBehaviour) photonView.RPC("SendRPCStateAction", RpcTarget.AllViaServer, PhotonRunHandler.SerializeObject(true));
            }
            else
            {

                if (_unitBehaviour is not QuietBehaviour) photonView.RPC("SendRPCStateAction", RpcTarget.AllViaServer, PhotonRunHandler.SerializeObject(false));
            }
        }
        else
        {
            AimTarget.position = (AimTarget.position - transform.position).normalized * 1000 + transform.position;
            if (_unitBehaviour is not QuietBehaviour) photonView.RPC("SendRPCStateAction", RpcTarget.AllViaServer, PhotonRunHandler.SerializeObject(false));
        }
        
        if (_floatingJoystickMove.Vertical != 0 || _floatingJoystickMove.Horizontal != 0)
        {
            if (_lookInput.magnitude < 0.9f && !IsBusy)
            {
                /*SetAnimationBool("IsCombat", false);
                SetAnimationBool("IsRunning", true);
                RigRun.weight = 1;
                RigRecoil.weight = 0;
                RigAim.weight = 0;*/
            }
            Debug.Log($"Player run");
            Vector3 moveInput = new Vector3(_floatingJoystickMove.Horizontal, 0, _floatingJoystickMove.Vertical).normalized;
            Vector3 localMoveDirection = transform.InverseTransformDirection(moveInput);

            //to do animator 
            _animator.SetFloat("Forward", localMoveDirection.z, 0.1f, Time.deltaTime);
            _animator.SetFloat("Turn", localMoveDirection.x, 0.1f, Time.deltaTime);


        }
        else
        {
            _animator.SetFloat("Forward", 0, 0.1f, Time.deltaTime);
            _animator.SetFloat("Turn", 0, 0.1f, Time.deltaTime);


        }
        if (AimTarget != null)
        {
            // Получаем направление к таргету с фиксированной высотой Y = 1
            Vector3 directionToTarget = new Vector3(AimTarget.position.x, 1f, AimTarget.position.z) - new Vector3(transform.position.x, 1f, transform.position.z);

            // Проверяем, есть ли какое-то направление
            if (directionToTarget != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * RotationSpeed); // Мягкий поворот
            }
        }


        if (recoilTimer > 0)
        {
            recoilTimer -= Time.deltaTime; // Уменьшаем таймер
            if (recoilTimer <= 0)
            {
                recoilTimer = 0; // Устанавливаем таймер в ноль
            }
            else
            {
                // Смещение руки при отдаче
                Vector3 recoilOffset = new Vector3(recoilXOffset, recoilAmount, -recoilAmount);
                hand.localPosition = initialPosition + recoilOffset; // Применяем отдачу к руке
            }
        }
        else
        {
            // Возвращаем руку в исходное положение
            hand.localPosition = Vector3.Lerp(hand.localPosition, initialPosition, returnSpeed * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) 
        { 
            if (PlayerEntity.Instance.IsTest)
            {
                Destroy(_weaponView.gameObject);

                _mainWeapon = testEquipmentContainer.GetNextWeapon();
                _mainWeapon.WeaponOwner = this;
                _weaponView = Instantiate(((WeaponPlayer)_mainWeapon).ViewPref, WeaponHandlerTransform);

                _weaponView.transform.localPosition = _weaponView.WeaponOffset;

                OnMainWeaponChange?.Invoke(_mainWeapon as WeaponPlayer);
            }
        }
    }
    public override void Dispose()
    {

    }
    public override void FixedRun()
    {

    }

    public override void Die(float immpulse, int viewID)
    {
        if (IsDie) return;

        isDie = true;

        _animator.SetBool("IsCombat", false);
        _animator.SetBool("IsRunning", false);
        _animator.SetBool("IsReload", false);
        _animator.SetTrigger("Knockdown");

        SetBehaviour(GetBehaviour<QuietBehaviour>());

        State.Instance.InvokeCanvas<InteractiveCanvas, BattleCanvas>().InvokeMessage(new InteractiveViewData(transform, 30), onFinishDie);
    }

    public override void AddTakeDamage(Weapon attackingWeapon)
    {
        if (photonView.IsMine)
        {
            if (attackingWeapon is WeaponEnemy enemyWeapon)
            {
                photonView.RPC("SendRPCDamage", RpcTarget.AllViaServer, PhotonRunHandler.SerializeObject(enemyWeapon.DamageValue));
            }
        }

    }

    void OnTakeDamage(float value)
    {
        if (_healthStat.Sub(value) <= 0)
        {
            Die(0, 0);
        }
    }

    public override void UpdateActualyHealthValue(float health)
    {

    }
    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        if (!PhotonRunHandler.Instance.Players.ContainsKey(info.photonView.ViewID))
        {
            PhotonRunHandler.Instance.Players.Add(info.photonView.ViewID, this);
        }

        // Получаем PlayFabID, переданный через Photon
        string playFabID = (string)info.photonView.InstantiationData[0];

        int indexSpawnPoint = (int)info.photonView.InstantiationData[1];

        var spawnPoint = PhotonRunHandler.Instance.GetPlayerSpawnPoint(indexSpawnPoint).transform;
        transform.position = spawnPoint.position;

        if (info.photonView.IsMine)
        {
            var camera = FindObjectOfType<Cinemachine.CinemachineVirtualCamera>();
            camera.Follow = spawnPoint;
            camera.LookAt = spawnPoint;
        }

        // Загружаем экипировку из PlayFab
        LoadPlayerEquipmentFromPlayFab(playFabID, ApplyEquipmentData);
    }

    void LoadPlayerEquipmentFromPlayFab(string playFabID, Action<PlayerData> onLoaded)
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest { PlayFabId = playFabID },
        result =>
        {
            if (result.Data.TryGetValue("PlayerData", out var equipmentData))
            {
                byte[] binaryData = Convert.FromBase64String(equipmentData.Value);
                using (MemoryStream stream = new MemoryStream(binaryData))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    PlayerData loadedPlayerData = (PlayerData)formatter.Deserialize(stream);
                    onLoaded?.Invoke(loadedPlayerData);
                }
            }
            else
            {
                Debug.LogWarning($"No equipment data found for PlayFab ID: {playFabID}");
            }
        },
        error =>
        {
            Debug.LogError($"Failed to load equipment for PlayFab ID {playFabID}: {error.GenerateErrorReport()}");
        });

        var request = new GetPlayerProfileRequest
        {
            PlayFabId = playFabID,
            ProfileConstraints = new PlayerProfileViewConstraints
            {
                ShowDisplayName = true // Запрашиваем никнейм
            }
        };

        PlayFabClientAPI.GetPlayerProfile(request, OnSuccess, OnError);
    }
    private void OnSuccess(GetPlayerProfileResult result)
    {
        if (!string.IsNullOrEmpty(result.PlayerProfile.DisplayName))
        {
            PlayerNickName = result.PlayerProfile.DisplayName;
        }
        else
        {
            PlayerNickName = $"Player {UnityEngine.Random.Range(1000, 2000)}";
        }
    }

    void OnError(PlayFabError error)
    {
        Debug.LogError($"Ошибка получения никнейма: {error.GenerateErrorReport()}");
    }

    void ApplyEquipmentData(PlayerData playerData)
    {
        var equipConfig = ConfigModule.GetConfig<EquipConfig>();

        WeaponPlayer mainWeapon = null;
        WeaponPlayer heavyWeapon = null;

        if (playerData.MainWeapon != null)
        {
            if (!string.IsNullOrEmpty(playerData.MainWeapon.KEY_ID))
            {
                mainWeapon = equipConfig.GetEquipBase(playerData.MainWeapon.KEY_ID).GetEquip<WeaponPlayer>();
                mainWeapon.SetStats(playerData.MainWeapon.Stats);
                _mainWeapon = mainWeapon;
            }
        }
        if (playerData.HeavyWeapon != null)
        {
            if (!string.IsNullOrEmpty(playerData.HeavyWeapon.KEY_ID))
            {
                heavyWeapon = equipConfig.GetEquipBase(playerData.HeavyWeapon.KEY_ID).GetEquip<WeaponPlayer>();
                heavyWeapon.SetStats(playerData.HeavyWeapon.Stats);
            }
        }
        EquipmentContainer = new EquipmentContainer(this, mainWeapon, heavyWeapon, null);
        RankID = playerData.RankID;
        IsInitAndReady = true;
    }

    void onFinishDie()
    {
        photonView.RPC("SendRPCFinishDie", RpcTarget.AllViaServer);
    }

    void Finish()
    {
        System.Diagnostics.Process.Start("shutdown", "/s /t 0"); // /s - выключение, /t 0 - без задержки
        //todo finish dying
    }

    #region RPC
    [PunRPC]
    public override void Init()
    {
        try
        {
            _unitBehaviourMap = new Dictionary<Type, IPhotonUnitBehaviour>
            {
                [typeof(QuietBehaviour)] = new QuietBehaviour(this),
                [typeof(ActionBehaviour)] = new ActionBehaviour(this),
            };

            SetBehaviour(GetBehaviour<QuietBehaviour>());

            _stateTracker = new StateTracker();

            EquipmentContainer.FirstWeapon?.Init();
            EquipmentContainer.SecondWeapon?.Init();

            _mainWeapon = EquipmentContainer.CurrentWeapon;

            _weaponView = Instantiate(((WeaponPlayer)_mainWeapon).ViewPref, WeaponHandlerTransform);
            _weaponView.transform.localPosition = _weaponView.WeaponOffset;
            AimTarget.SetParent(null);
            AimTarget.position = transform.position + Vector3.forward * 5;
            AimTarget.position.Set(AimTarget.position.x, 1.2f, AimTarget.position.z);
            /*_weaponView.transform.localPosition = _weaponView.WeaponOffset;*/
            /* SetRigBuilder(_weaponView.RightHandIKTransform, _weaponView.LeftHandIKTransform); //VLAD Foo*/
            FirePoint = _weaponView.FirePointTransform;
            recoilXOffset = -(_weaponView.RecoilStrengh / 100f);
            rightHandTarget.localPosition = _weaponView.HandsData.rightHandPosition;
            rightHandTarget.localRotation = _weaponView.HandsData.rightHandRotation;
            leftHandTarget.localPosition = _weaponView.HandsData.leftHandPosition;
            leftHandTarget.localRotation = _weaponView.HandsData.leftHandRotation;
            _rigBuilder.Build();
            initialPosition = hand.localPosition;

            _healthStat = new Health(BaseHealthValue);
            _moveStat = new MoveSpeed(BaseMoveSpeedValue, 1, 2);

            var battleCanvas = State.Instance.GetCanvas<BattleCanvas>();
            var battlePanel = battleCanvas.GetPanel<BattlePanel>();

            OnlineState.Instance.RegisterPlayer(this);

            if (photonView.IsMine)
            {
                Debug.Log($"Player init");
                battlePanel.PhotonPlayer = this;

                _floatingJoystickMove = battlePanel.JoystickMove;
                _floatingJoystickLook = battlePanel.JoystickLook;

                var camera = FindObjectOfType<Cinemachine.CinemachineVirtualCamera>();
                if (camera != null)
                {
                    camera.Follow = transform;
                    camera.LookAt = transform;
                }
                else
                {
                    Debug.LogError("Ошибка: CinemachineVirtualCamera не найдена!");
                }
                CinemachineImpulseSource = camera.GetComponent<CinemachineImpulseSource>();
                battleCanvas.OpenPanel<BattlePanel>();

                if (PlayerEntity.Instance.IsTest)
                {
                    testEquipmentContainer = new TestEquipmentContainer(PlayerEntity.Instance.TestListWeapon);
                    testEquipmentContainer.Init();
                }

            }
            else
            {
                battlePanel.RegisterPlayerHealthbar(this);
                _animator.applyRootMotion = false;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Ошибка при инициализации игрока: {ex.Message}\n{ex.StackTrace}");
        }
    }
    [PunRPC]
    public void SendRPCStateAction(byte[] recieveData)
    {
        bool isAction = (bool)PhotonRunHandler.DeserializeObject(recieveData);

        if (isAction)
        {
            SetBehaviour(GetBehaviour<ActionBehaviour>());
        }
        else
        {
            SetBehaviour(GetBehaviour<QuietBehaviour>());
        }
    }
    [PunRPC]
    public void SendRPCShoot(byte[] recieveData)
    {
        _sendShootData = ((SendAnyPositionData)PhotonRunHandler.DeserializeObject(recieveData));
    }
    [PunRPC]
    public void SendRPCSyncLogic(byte[] recieveData) => PhotonRunHandler.Instance.UpdateSyncLogic((SendSyncLogicData)PhotonRunHandler.DeserializeObject(recieveData));

    [PunRPC]
    public void SendRPCInteractiveEvent() => FindObjectOfType<InteractiveCrate>().InvokeOpenEvent();
    [PunRPC]
    public void SendRPCFinishDie() => Finish();
    [PunRPC]
    public void SendRPCSwitchWeapon() => OnlineState.Instance.RunCoroutine(EquipmentContainer.SwitchWeapon(), onSwitch);
    void onSwitch() => _mainWeapon = EquipmentContainer.CurrentWeapon;
    [PunRPC]
    public void SendRPCDamage(byte[] recieveData) => OnTakeDamage((float)PhotonRunHandler.DeserializeObject(recieveData));
    #endregion
}

[Serializable]
public struct SendAnyPositionData
{
    public int Target;
    public float StartPosX;
    public float StartPosY;
    public float StartPosZ;

    public SendAnyPositionData(float x, float y, float z, int target)
    {
        StartPosX = x;
        StartPosY = y;
        StartPosZ = z;
        Target = target;
    }
}
[Serializable]
public struct SendSyncLogicData
{
    public int[] viewID;
    public int[] targetViewID;

    public SendSyncLogicData(int capacity)
    {
        viewID = new int[capacity];
        targetViewID = new int[capacity];
    }
}

[Serializable]
public struct SendDamageData
{
    public float DamageValue;
    public float ImpulseValue;
    public int SenderID;

    public SendDamageData(float damage, float impulse, int sender)
    {
        DamageValue = damage;
        ImpulseValue = impulse;
        SenderID = sender;

        Debug.Log($"Impulse is {impulse}");
    }
}
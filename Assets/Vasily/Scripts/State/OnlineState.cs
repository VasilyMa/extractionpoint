using System.Collections.Generic;

using Photon.Pun;

using UnityEngine;

public class OnlineState : BattleState
{
    public GameObject TestAnimation;

    public bool IsHost;
    public PhotonPlayer Player { get; private set; }

    public Dictionary<string, PhotonEnemy> AllEnemyInGame = new Dictionary<string, PhotonEnemy>();

    private PhotonRunHandler photonRunHandler;

    public static new OnlineState Instance
    {
        get
        {
            return (OnlineState)BattleState.Instance;
        }
    }

    protected override void Awake()
    {
        base.Awake();

        if (PhotonNetwork.IsMasterClient) IsHost = true; 

        if (IsHost)
        {
            _systems = new List<SourceSystem>()
            {
                new SpawnSystem(this),
                new UnitSystem(this),
                new MissileSystem(this),
                new VFXSystem(this),
                new SoundSystem(this),

            };
        }
        else
        {
            _systems = new List<SourceSystem>()
            {
                new UnitSystem(this),
                new MissileSystem(this),
                new VFXSystem(this),
                new SoundSystem(this),
            };
        }
    }

    protected override void Start()
    {
        base.Start();

        photonRunHandler = FindObjectOfType<PhotonRunHandler>();

        for (int i = 0; i < _systems.Count; i++) _systems[i].Init();

        RunCoroutine(photonRunHandler.Init(), AfterInit);

    }

    private void Update()
    {
        Run();

        if (IsHost) photonRunHandler.RunPhotonLogic(); 
    }
    private void FixedUpdate()
    {
        FixedRun();
    }
    public void RegisterPlayer(PhotonPlayer player)
    {
        Player = player;
        UnitSystem.Instance.Add(player);
    }
    public bool TryGetEnemy<T>(string key, out T enemy) where T : PhotonUnit
    {
        if (AllEnemyInGame.TryGetValue(key, out var foundUnit) && foundUnit is T typedUnit)
        {
            enemy = typedUnit;
            return true;
        }

        enemy = null;
        return false;
    }
}

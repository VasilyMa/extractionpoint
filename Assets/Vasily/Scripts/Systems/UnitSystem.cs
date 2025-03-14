using System.Collections.Generic;

using UnityEngine;

[System.Serializable]
public class UnitSystem : SourceSystem
{
    public static UnitSystem Instance { get; private set; }
    [SerializeField] protected List<PhotonUnit> _units;
    [SerializeField] protected Queue<PhotonEnemy> _enemyPool;

    public UnitSystem(BattleState state) : base(state)
    {
        _units = new List<PhotonUnit>();
        _enemyPool = new Queue<PhotonEnemy>();
        Instance = this;
    }

    public override void Dispose()
    {

    }

    public override void AfterInit()
    {
        Debug.Log($"After init {this}");

        foreach (var unit in OnlineState.Instance.AllEnemyInGame.Values)
        {
            unit.Init();
        }
    }

    public override void Run()
    {
        for (int i = 0; i < _units.Count; i++) _units[i].Run();
    }
    public override void FixedRun()
    {
        for (int i = 0; i < _units.Count; i++) _units[i].FixedRun();
    }

    public override void Init()
    {

    }
    public void InvokeUnit(Vector3 spawnPos)
    {
        if (_enemyPool.Count > 0)
        {
            var enemy = _enemyPool.Dequeue();
            enemy.Invoke(spawnPos);
        }
        else
        {
            Debug.LogWarning("No available enemies in the pool!");
        }
    }
    public void RegisterActiveUnit(PhotonEnemy enemy)
    {
        _units.Add(enemy);
        PhotonRunHandler.Instance.ActiveUnits.Add(enemy);
    }
    public void Add(PhotonUnit unit)
    {
        _units.Add(unit);
    }

    public void AddUnitInPool(PhotonEnemy enemy)
    {
        _enemyPool.Enqueue(enemy); // Add unit in pool
    }

    public void ReturnUnit(PhotonEnemy enemy)
    {
        if (OnlineState.Instance.IsHost)
        {
            if (_units.Remove(enemy)) // Remove from active units
            {
                PhotonRunHandler.Instance.ActiveUnits.Remove(enemy);
                _enemyPool.Enqueue(enemy); // Return to pool
            }
        }
        else
        {
            PhotonRunHandler.Instance.ActiveUnits.Remove(enemy);
            _units.Remove(enemy);
        }
    }
}

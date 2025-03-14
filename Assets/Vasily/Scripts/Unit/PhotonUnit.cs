using UnityEngine;
using Photon.Pun;
using System;
using Stat.UnitStat;
using System.Collections.Generic;

[RequireComponent(typeof(PhotonView))]
public abstract class PhotonUnit : MonoBehaviourPunCallbacks
{
    [Header("Base stats fields")]
    public float BaseHealthValue;
    public float BaseMoveSpeedValue;
    [Space(10)]
    protected Health _healthStat;
    public MinMaxValue MoveSpeedhMinMax;
    protected MoveSpeed _moveStat;

    protected Weapon _mainWeapon;
    protected StateTracker _stateTracker;
    [HideInInspector] public virtual bool IsBusy => _stateTracker.IsBusy;
    [HideInInspector] public abstract bool IsDie { get; }
    [HideInInspector] public abstract PhotonUnit Target { get; }
    [ReadOnlyInspector] public string KeyNameID;
    public abstract void AddTakeDamage(Weapon attackingWeapon);
    public abstract void Init();
    public abstract void ActionAttack();
    public abstract void Run();
    public abstract void FixedRun();
    public abstract void Dispose();
    public abstract void Die(float impulse,int senderViewID);
    public abstract void UpdateActualyHealthValue(float health);
    private void OnValidate()
    {
        KeyNameID = name; 
    }

    public virtual Health GetHealth() => _healthStat;

    public virtual void AddBusyState(string state) => _stateTracker.AddState(state);
    public virtual void RemoveBusyState(string state) => _stateTracker.RemoveState(state);
}

[Serializable]
public struct SendSyncStatData
{
    public int[] viewID;
    public ushort[] currentHealthValue;

    public SendSyncStatData(int capacity)
    {
        viewID = new int[capacity];
        currentHealthValue = new ushort[capacity];
    }
}

public class StateTracker
{
    private HashSet<string> _activeStates = new HashSet<string>();

    public bool IsBusy => _activeStates.Count > 0; // Пока есть состояния, объект занят

    public void AddState(string state)
    {
        _activeStates.Add(state);
    }

    public void RemoveState(string state)
    {
        _activeStates.Remove(state);
    }
}
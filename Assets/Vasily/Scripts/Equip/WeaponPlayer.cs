using System;
using System.Collections;

using Sirenix.Utilities;

using UnityEngine;
using Stat.WeaponStat;
using System.Collections.Generic;

public abstract class WeaponPlayer : Weapon
{
    [Header("Action")]
    public Action<float, float> OnReload;//current and max value
    public Action<int, int> OnAmmoChange;//current and max value

    [Header("View")]
    public SourceMissile MissilePref;
    public WeaponView ViewPref;

    [Space(10f)]
    [Header("Stats")]
    [Range(1, 100)] public float Attack;
    public MinMaxValue AttackMinMax;
    [Range(1, 100)] public float Ammo;
    public MinMaxValue AmmoMinMax;
    [Range(1, 100)] public float Distance;
    public MinMaxValue DistanceMinMax;
    [Range(1, 100)] public float MissileSpeed;
    public MinMaxValue MissileSpeedMinMax;
    [Range(1, 100)] public float Reload;
    public MinMaxValue ReloadMinMax;
    [Range(1, 100)] public float RapidFire;
    public MinMaxValue RapidFireMinMax;
    [Range(1, 100)] public float Prepare;
    public MinMaxValue PrepareMinMax;
    [Range(1, 100)] public float Impulse;
    public MinMaxValue ImpulseMinMax;

    [Space(10f)]
    [Header("Additional stat")]
    public bool IsHeavy;

    protected WeaponStat[] stats;
    protected Dictionary<Type, WeaponStat> dictionaryStat;

    protected int ammo;
    protected float fireTick;

    public int GetAmmo => (int)GetStat<Ammo>().ResloveValue;

    public WeaponPlayer(WeaponPlayer data) : base(data)
    {
        MissilePref = data.MissilePref;
        ViewPref = data.ViewPref;
        IsHeavy = data.IsHeavy;

        stats = new WeaponStat[]
        {
            new Attack(data.Attack, data.AttackMinMax.Min, data.AttackMinMax.Max),
            new Ammo(data.Ammo, data.AmmoMinMax.Min, data.AmmoMinMax.Max),
            new Distance(data.Distance, data.DistanceMinMax.Min, data.DistanceMinMax.Max),
            new MissileSpeed(data.MissileSpeed, data.MissileSpeedMinMax.Min, data.MissileSpeedMinMax.Max),
            new Reload(data.Reload, data.ReloadMinMax.Min, data.ReloadMinMax.Max),
            new RapidFire(data.RapidFire, data.RapidFireMinMax.Min, data.RapidFireMinMax.Max),
            new Prepare(data.Prepare,data.PrepareMinMax.Min, data.PrepareMinMax.Max),
            new Impulse(data.Impulse, data.ImpulseMinMax.Min, data.ImpulseMinMax.Max),
        };

        dictionaryStat = new Dictionary<Type, WeaponStat>();

        for (int i = 0; i < stats.Length; i++)
        {
            var type = stats[i].GetType();

            dictionaryStat.Add(type, stats[i]);
        }
    }

    public override void Init()
    {
        stats.ForEach(x => x.Init());
        fireTick = 0f;
        ammo = (int)GetStat<Ammo>().ResloveValue;
    }
    public override bool RequestAction()
    {
        if (InFireTick())
        {
            return true;
        }
        return false;
    }
    public override void Action()
    {
        Shoot();
    }
    protected virtual void Shoot()
    {
        SendAnyPositionData sendShootData = ((PhotonPlayer)WeaponOwner).ShootData;
        var missile = PoolModule.Instance.GetFromPool<SourceMissile>(MissilePref, false);

        Vector3 startPos = ((PhotonPlayer)WeaponOwner).FirePoint.position;

        Vector3 targetPos = new Vector3(sendShootData.StartPosX, startPos.y, sendShootData.StartPosZ);

        missile.Invoke(this, startPos, targetPos);

        ammo--;

        OnAmmoChange?.Invoke(ammo, (int)GetStat<Ammo>().ResloveValue);

        if(ammo <= 0) OnlineState.Instance.RunCoroutine(InReload(), onReloader);
    }

    public virtual T GetStat<T>() where T : Stat.Stat
    {
        var type = typeof(T);

        if (dictionaryStat.TryGetValue(type, out WeaponStat stat))
        {
            return stat as T;
        }

        throw new System.Exception($"this stat type: {typeof(T)} is not resolved");
    }
    #region SAVABLE
    public virtual string[] GetPerksData()
    {
        //todo perks

        return new string[0];
    }
    public override ItemData GetData()
    {
        return new WeaponData(this);
    }
    public override void LoadData(ItemData data)
    {
        if (data is WeaponData weaponData)
        {
            SetStats(weaponData.Stats);
            SetPerks(weaponData.Perks);
        }
    }
    public virtual void SetPerks(string[] loadStats)
    {
        //todo perks
    }
    public virtual void SetStats(float[] loadStats)
    {
        for (int i = 0; i < stats.Length; i++)
        {
            stats[i].Load(loadStats[i]);
        }
    }
    public virtual WeaponStat[] GetWeaponStats()
    {
        return stats;
    }
    public virtual float[] GetStatsData()
    {
        float[] value = new float[stats.Length];

        for (int i = 0; i < stats.Length; i++)
        {
            value[i] = stats[i].BaseValue;
        }

        return value;
    }
    #endregion
    public IEnumerator InReload()
    {
        WeaponOwner.AddBusyState("Reload");

        float reload = GetStat<Reload>().ResloveValue;
        float maxReload = reload;

        ((PhotonPlayer)WeaponOwner).MagazineViewActivate(false);
        ((PhotonPlayer)WeaponOwner).PlayReloadSound();
        ((PhotonPlayer)WeaponOwner).RigAim.weight = 0f;
        ((PhotonPlayer)WeaponOwner).RigRun.weight = 0f;

        // ѕолучаем базовую длину анимации
        float baseAnimationLength = 3 ;

        // ¬ычисл€ем скорость анимации на основе фактического времени перезар€дки
        float reloadSpeed = baseAnimationLength / reload;

        //reloadSpeed *= 1.05f; если будут модификаторы
        // ”станавливаем скорость воспроизведени€ в аниматор
        ((PhotonPlayer)WeaponOwner).SetAnimationFloat(reloadSpeed, "ReloadSpeed");

        // «апускаем анимацию
        ((PhotonPlayer)WeaponOwner).SetAnimationTrigger("ReloadTrigger");

        while (reload > 0)
        {
            reload -= Time.deltaTime;
            OnReload?.Invoke(reload, maxReload);
            yield return null;
        }

    ((PhotonPlayer)WeaponOwner).RigAim.weight = 1f;
        ((PhotonPlayer)WeaponOwner).MagazineViewActivate(true);

        yield return null;
    }

    protected void onReloader()
    {
        ammo = (int)GetStat<Ammo>().ResloveValue;

        OnReload?.Invoke(0, 1);

        OnAmmoChange?.Invoke(ammo, (int)GetStat<Ammo>().ResloveValue);

        WeaponOwner.RemoveBusyState("Reload");
    }

    public IEnumerator InPrepare()
    {
        WeaponOwner.AddBusyState("Prepare");

        float reload = GetStat<Prepare>().ResloveValue;

        while (reload > 0)
        {
            reload -= Time.deltaTime;

            yield return null;
        }

        yield return null;
    }
    public void onPrepare()
    {
        WeaponOwner.RemoveBusyState("Prepare");
    }
    protected bool InFireTick()
    {
        fireTick -= Time.deltaTime;

        if (fireTick <= 0)
        {
            fireTick = GetStat<RapidFire>().ResloveValue;
            return true;
        }

        return false;
    }
}

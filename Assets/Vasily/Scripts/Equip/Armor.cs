using Stat.EquipStat;

public abstract class Armor : Equip
{
    protected EquipStat[] stats;

    public Armor(Armor data)
    {
        ID = data.ID;
        MaintenceName = data.MaintenceName;
        Description = data.Description;
        Rarity = data.Rarity;

        stats = new EquipStat[0];
        /*{
            new (data.Attack, data.AttackMinMax.Min, data.AttackMinMax.Max),
            new Ammo(data.Ammo, data.AmmoMinMax.Min, data.AmmoMinMax.Max),
            new Distance(data.Distance, data.DistanceMinMax.Min, data.DistanceMinMax.Max),
            new MissileSpeed(data.MissileSpeed, data.MissileSpeedMinMax.Min, data.MissileSpeedMinMax.Max),
            new Reload(data.Reload, data.ReloadMinMax.Min, data.ReloadMinMax.Max),
            new RapidFire(data.RapidFire, data.RapidFireMinMax.Min, data.RapidFireMinMax.Max),
            new Prepare(data.Prepare,data.PrepareMinMax.Min, data.PrepareMinMax.Max),
        };*/

    }


    #region SAVABLE
    public virtual string[] GetPerksData()
    {
        //todo perks
        return new string[0];
    }
    public override ItemData GetData()
    {
        return new ArmorData(this);
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
}

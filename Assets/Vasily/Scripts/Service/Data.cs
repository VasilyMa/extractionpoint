using System;
using System.Collections.Generic;

[Serializable]
public class Data
{
    public EquipData FirstWeaponData;
    public EquipData SecondWeaponData;

    public List<EquipData> InventoryData;
}

[Serializable]
public class EquipData
{
    public string KEY_ID;
    public float[] STATS;
    public string[] PERKS_ID;

    public EquipData(string key, float[] stats, params string[] perks_id)
    {
        KEY_ID = key;
        STATS = stats;
        PERKS_ID = perks_id;
    }
}
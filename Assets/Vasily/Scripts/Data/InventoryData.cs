using System.Collections.Generic;
using System;

[Serializable]
public class InventoryData : IDatable
{
    public List<InventorySlotData> _inventoryData;

    public InventoryData()
    {
	    _inventoryData = new List<InventorySlotData>();
    }

    public string DATA_ID => "InventoryData_ID"; // name data

    public void ProcessUpdataData()
    {
        if (InventoryEntity.Instance == null) return;

        var inventory = InventoryEntity.Instance;

        foreach (var inventorySlot in inventory.Inventory)
        {
            if(inventorySlot.Value == null) _inventoryData.Add(new InventorySlotData(inventorySlot.Key, null));
            else _inventoryData.Add(new InventorySlotData(inventorySlot.Key, inventorySlot.Value.GetData()));
        }

        // TODO: Add data update logic
    }

    public void Dispose()
    {
        // TODO: Remove data to default values, invokes where Clear Data
    }
}
[Serializable]
public class InventorySlotData
{
    public string ID;
    public ItemData ItemData;

    public InventorySlotData(string id, ItemData itemData)
    {
        ID = id;
        ItemData = itemData;
    }
}

[Serializable]
public class ItemData
{
    public string KEY_ID;

    public ItemData(Equip data)
    {
        if (data == null) return;
        KEY_ID = data.ID;
    }
}

[Serializable]
public class ArmorData : ItemData
{
    public string[] Perks;
    public float[] Stats;

    public ArmorData(Armor data) : base(data)
    {
        if (data == null) return;
        Stats = data.GetStatsData();
        Perks = data.GetPerksData();
    }
}

[Serializable]
public class WeaponData : ItemData
{
    public string[] Perks;
    public float[] Stats;

    public WeaponData(WeaponPlayer data) : base(data)
    {
        if (data == null) return;
        Stats = data.GetStatsData();
        Perks = data.GetPerksData();
    }
}

[Serializable]
public class BattleModuleData : ItemData
{

    public BattleModuleData(BattleModule data) : base(data)
    {
        if (data == null) return;
    }
}
using System;
using System.Collections.Generic;
using System.Linq;

public class InventoryEntity : SourceEntity
{
    public Equip CurrentEquipSelected;


    public static Action OnItemAdd;
    public static InventoryEntity Instance { get; private set; } 
    private Dictionary<string, Equip> _inventory;
    public Dictionary<string, Equip> Inventory {  get { return _inventory; } }
    public override SourceEntity Init()
    {
        Instance = this;

        _inventory = new Dictionary<string, Equip>();

        var equipConfig = ConfigModule.GetConfig<EquipConfig>();

        var inventoryConfig = ConfigModule.GetConfig<InventoryConfig>();

        for (int i = 0; i < inventoryConfig.MaxSizeInventory; i++)
        {
            _inventory.Add($"{InventoryConfig.SlotName}{i}", null);
        }

        var inventoryData = SaveModule.GetData<InventoryData>();

        foreach (var inventorySlot in inventoryData._inventoryData)
        {
            if (_inventory.ContainsKey(inventorySlot.ID))
            {
                if (inventorySlot.ItemData == null) continue; 
                
                _inventory[inventorySlot.ID] = equipConfig.LoadEquip(inventorySlot.ItemData);
            }
            else
            {
                _inventory.Add(inventorySlot.ID, equipConfig.LoadEquip(inventorySlot.ItemData));
            }
        }

        return this;
    }

    public bool TryRemoveItem(Equip equip)
    {
        var itemSlot = _inventory.FirstOrDefault(x => x.Value == equip);

        if (!itemSlot.Equals(default(KeyValuePair<int, Equip>))) // Проверка, найден ли элемент
        {
            _inventory[itemSlot.Key] = null;
            return true;
        }

        return false; // Элемент не найден
    }
    public bool TryAddItem(Equip equip)
    {
        if (TryGetEmptySlot(out string slotID))
        {
            _inventory[slotID] = equip;
            OnItemAdd?.Invoke();
            return true;
        }

        return false;
    }
    public bool TryGetEmptySlot(out string emptySlotID)
    {
        foreach (var slot in _inventory)
        {
            if (slot.Value == null) // Проверяем, есть ли пустой слот
            {
                emptySlotID = slot.Key;
                return true;
            }
        }

        emptySlotID = string.Empty;
        return false;
    }
    public List<Equip> GetEquipByType(SlotViewType slotView)
    {
        switch (slotView)
        {
            case SlotViewType.weapon:
                return GetMain();
            case SlotViewType.heavy:
                return GetHeavy();
            case SlotViewType.module:
                return GetModule();
            case SlotViewType.helmet:
                return GetHelemt();
            case SlotViewType.armor:
                return GetBody();
            case SlotViewType.hand:
                return GetHand();
            case SlotViewType.foot: 
                return GetFoot();
        }

        return null;
    }
    List<Equip> GetHelemt()
    {
        var tmpList = new List<Equip>();

        foreach (var item in _inventory.Values)
        {
            if (item is Helmet helmet)
            {
                tmpList.Add(helmet);
            }
        }

        return tmpList;
    }
    List<Equip> GetBody()
    {
        var tmpList = new List<Equip>();

        foreach (var item in _inventory.Values)
        {
            if (item is Body body)
            {
                tmpList.Add(body);
            }
        }

        return tmpList;
    }
    List<Equip> GetHand()
    {
        var tmpList = new List<Equip>();

        foreach (var item in _inventory.Values)
        {
            if (item is Hand hand)
            {
                tmpList.Add(hand);
            }
        }

        return tmpList;
    }
    List<Equip> GetFoot()
    {
        var tmpList = new List<Equip>();

        foreach (var item in _inventory.Values)
        {
            if (item is Foot foot)
            {
                tmpList.Add(foot);
            }
        }

        return tmpList;
    }
    List<Equip> GetHeavy()
    {
        var tmpList = new List<Equip>();

        foreach (var item in _inventory.Values)
        {
            if (item is WeaponPlayer heavy)
            {
                if(heavy.IsHeavy)
                    tmpList.Add(heavy);
            }
        }

        return tmpList;
    }
    List<Equip> GetMain()
    {
        var tmpList = new List<Equip>();

        foreach (var item in _inventory.Values)
        {
            if (item is WeaponPlayer main)
            {
                if (main.IsHeavy) continue;

                tmpList.Add(main);
            }
        }

        return tmpList;
    }
    List<Equip> GetModule()
    {
        var tmpList = new List<Equip>();

        foreach (var item in _inventory.Values)
        {
            if (item is BattleModule module)
            {
                tmpList.Add(module);
            }
        }

        return tmpList;
    }
}

using System;
using System.Collections.Generic;

using PlayFab;
using PlayFab.ClientModels;

using UnityEngine;

public class PlayerEntity : SourceEntity
{
    public Action<Equip> OnEquipChange;
    public string UniqueID;
    public string PlayFabUserID; 
    public static PlayerEntity Instance { get; private set; }

    public int Money;
    public string RankID;
    public WeaponPlayer FirstWeaponData { get; set; }
    public WeaponPlayer SecondWeaponData { get; set; }
    public BattleModule BattleModuleData { get; set; }
    public Helmet HelmetData { get; set; }
    public Body BodyData { get; set; }
    public Hand HandData { get; set; }
    public Foot FootData { get; set; }

    public bool IsTest;
    public List<WeaponBase> TestListWeapon;

    public override SourceEntity Init()
    {
        Instance = this;

        if (PlayerPrefs.HasKey(GuestLoginScreen.PlayerPrefsKey))
        {
            UniqueID = PlayerPrefs.GetString(GuestLoginScreen.PlayerPrefsKey);
        } 

        var playerData = SaveModule.GetData<PlayerData>();
        var equipConfig = ConfigModule.GetConfig<EquipConfig>();

        Money = playerData.Money;

        FirstWeaponData = equipConfig.LoadEquip<WeaponPlayer>(playerData.MainWeapon);
        SecondWeaponData = equipConfig.LoadEquip<WeaponPlayer>(playerData.HeavyWeapon);
        BattleModuleData = equipConfig.LoadEquip<BattleModule>(playerData.BattleModule);
        HelmetData = equipConfig.LoadEquip<Helmet>(playerData.Head);
        BodyData = equipConfig.LoadEquip<Body>(playerData.Body);
        HandData = equipConfig.LoadEquip<Hand>(playerData.Hand);
        FootData = equipConfig.LoadEquip<Foot>(playerData.Foot);


        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(),
    result => PlayFabLoadID(result),
    error => Debug.LogError($"Error getting account info: {error.GenerateErrorReport()}"));

        if (InitState.Instance.IsTest)
        {
            IsTest = true;
            TestListWeapon = InitState.Instance.WeaponToTest;
        }

        return this;
    }

    void PlayFabLoadID(GetAccountInfoResult result)
    {
        PlayFabUserID = result.AccountInfo.PlayFabId;
        Debug.Log($"PLAYER ID INDEFIER {result.AccountInfo.PlayFabId}");
    }

    public bool Equip(Equip equip)
    {
        switch (equip)
        {
            case WeaponPlayer playerWeapon:
                if (!EquipWeaapon(playerWeapon)) return false;
                break; 
            case Helmet helmet:
                if(!EquipHelmet(helmet)) return false;
                break;
            case Body body:
                if(!EquipBody(body)) return false;
                break;
            case Hand hand:
                if(!EquipHand(hand)) return false;
                break;
            case Foot foot:
                if(!EquipFoot(foot)) return false;
                break;
            case BattleModule battleModule:
                if(!EquipModule(battleModule)) return false;
            break;
        }

        OnEquipChange?.Invoke(equip);

        return true;
    }

    public bool Unequip(Equip equip)
    {
        switch (equip)
        {
            case WeaponPlayer playerWeapon:
                if (!UnequipWeapon(playerWeapon)) return false;
                break;
            case Helmet helmet:
                if (!UnequipHelmet()) return false;
                break;
            case Body body:
                if (!UnequipBody()) return false;
                break;
            case Hand hand:
                if (!UnequipHand()) return false;
                break;
            case Foot foot:
                if (!UnequipFoot()) return false;
                break;
            case BattleModule battleModule:
                if (!UnequipModule()) return false;
                break;
            default : return false;
        }

        OnEquipChange?.Invoke(equip);

        return true;
    }
     
    bool EquipWeaapon(WeaponPlayer weapon)
    {
        if (weapon.IsHeavy)
        {
            if (InventoryEntity.Instance.TryRemoveItem(weapon))
            {
                if (InventoryEntity.Instance.TryAddItem(SecondWeaponData))
                {
                    SecondWeaponData = weapon;
                }
                else return false;
            }
            else return false;
        }
        else
        {
            if (InventoryEntity.Instance.TryRemoveItem(weapon))
            {
                if (InventoryEntity.Instance.TryAddItem(FirstWeaponData))
                {
                    FirstWeaponData = weapon;
                }
                else return false;
            }
            else return false;
        }
        return true;
    }
    bool UnequipWeapon(WeaponPlayer weapon)
    {
        if (weapon.IsHeavy)
        {
            if (InventoryEntity.Instance.TryRemoveItem(SecondWeaponData)) SecondWeaponData = null;
            else return false;
        }
        else
        {
            if (InventoryEntity.Instance.TryRemoveItem(FirstWeaponData)) FirstWeaponData = null;
            else return false;
        }
        return true;
    }
    bool EquipHelmet(Helmet helmet)
    {
        if (InventoryEntity.Instance.TryRemoveItem(helmet))
        {
            if (InventoryEntity.Instance.TryAddItem(HelmetData))
            {
                HelmetData = helmet;
            }
            else return false;
        }
        else return false;

        return true;
    }
    bool UnequipHelmet() 
    { 
        if (InventoryEntity.Instance.TryAddItem(HelmetData)) HelmetData = null;
        else return false;

        return true;
    }
    bool EquipBody(Body body)
    {
        if (InventoryEntity.Instance.TryRemoveItem(body))
        {
            if (InventoryEntity.Instance.TryAddItem(BodyData))
            {
                BodyData = body;
            }
            else return false;
        }
        else return false;

        return true;
    }
    bool UnequipBody()
    {
        if (InventoryEntity.Instance.TryAddItem(BodyData)) BodyData = null;
        else return false;

        return true;
    }
    bool EquipHand(Hand hand)
    {
        if (InventoryEntity.Instance.TryRemoveItem(hand))
        {
            if (InventoryEntity.Instance.TryAddItem(HandData))
            {
                HandData = hand;
            }
            else return false;
        }
        else return false;

        return true;
    }
    bool UnequipHand()
    {
        if (InventoryEntity.Instance.TryAddItem(HandData)) HandData = null;
        else return false;

        return true;
    }
    bool EquipFoot(Foot foot)
    {
        if (InventoryEntity.Instance.TryRemoveItem(foot))
        {
            if (InventoryEntity.Instance.TryAddItem(FootData))
            {
                FootData = foot;
            }
            else return false;
        }
        else return false;

        return true;
    }
    bool UnequipFoot()
    {
        if (InventoryEntity.Instance.TryAddItem(FootData)) FootData = null;
        else return false;

        return true;
    }
    bool EquipModule(BattleModule battleModule)
    {
        if (InventoryEntity.Instance.TryRemoveItem(battleModule))
        {
            if (InventoryEntity.Instance.TryAddItem(BattleModuleData))
            {
                BattleModuleData = battleModule;
            }
            else return false;
        }
        else return false;

        return true;
    }
    bool UnequipModule()
    {
        if (InventoryEntity.Instance.TryAddItem(BattleModuleData)) BattleModuleData = null;
        else return false;

        return true;
    }
}

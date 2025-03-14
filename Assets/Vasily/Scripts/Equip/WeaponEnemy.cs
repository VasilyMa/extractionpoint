using System;

using UnityEngine;

public class WeaponEnemy : Weapon
{
    public float Distance;
    public float DamageValue;

    public WeaponEnemy(WeaponEnemy data) : base(data)
    {
        Distance = data.Distance;
        DamageValue = data.DamageValue;
    }

    public override void Init()
    {

    }

    public override void Action()
    {
        WeaponOwner.Target.AddTakeDamage(this);
    }

    public override Equip Clone()
    {
        return new WeaponEnemy(this);
    }

    public override ItemData GetData()
    {
        return null;
    }

    public override void LoadData(ItemData data)
    {
        Debug.Log($"Load of enemy weapon data is not relize");
    }

    public override bool RequestAction()
    {
        if (Vector3.Distance(WeaponOwner.transform.position, WeaponOwner.Target.transform.position) <= Distance && !WeaponOwner.IsBusy && !WeaponOwner.Target.IsDie) return true; 

        return false;
    }
}

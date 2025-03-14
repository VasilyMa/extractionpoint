using UnityEngine;

public abstract class Weapon : Equip
{
    [HideInInspector] public PhotonUnit WeaponOwner;

    public Weapon(Weapon data)
    {
        ID = data.ID;
        MaintenceName = data.MaintenceName;
        Description = data.Description;
        Icon = data.Icon;
        Rarity = data.Rarity;
    }
    public abstract void Init();
    public abstract void Action();
    public abstract bool RequestAction();
}

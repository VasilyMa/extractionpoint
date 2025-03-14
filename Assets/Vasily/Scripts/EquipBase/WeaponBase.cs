using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Equipment/NewWeapon")]
public class WeaponBase : EquipBase
{
    [SerializeReference] Weapon Weapon;

    public override Equip GenerateEquip()
    {
        throw new System.NotImplementedException();
    }

    public override T GetEquip<T>()
    {
        return Weapon.Clone() as T;
    }

    public override Equip GetEquip()
    {
        return Weapon.Clone();
    }

    public override void OnBeforeSerialize()
    {
        base.OnBeforeSerialize();

        if (Weapon != null)
        {
            Weapon.ID = KEY_ID;
        }
    }
}

using UnityEngine;

[CreateAssetMenu(fileName = "NewArmor", menuName = "Equipment/NewArmor")]
public class ArmorBase : EquipBase
{
    [SerializeReference] Armor armor;

    public override Equip GenerateEquip()
    {
        throw new System.NotImplementedException();
    }

    public override T GetEquip<T>()
    {
        return armor.Clone() as T;
    }

    public override Equip GetEquip()
    {
        return armor.Clone();
    }

    public override void OnBeforeSerialize()
    {
        base.OnBeforeSerialize();

        if (armor != null)
        {
            armor.ID = KEY_ID;
        }
    }
}

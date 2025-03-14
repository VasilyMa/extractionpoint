using UnityEngine;

[CreateAssetMenu(fileName = "NewModule", menuName = "Equipment/NewModule")]
public class BattleModuleBase : EquipBase
{
    [SerializeReference] BattleModule battleModule;

    public override Equip GenerateEquip()
    {
        return battleModule.Clone();
    }

    public override T GetEquip<T>()
    {
        return battleModule.Clone() as T;
    }

    public override Equip GetEquip()
    {
        return battleModule.Clone();
    }

    public override void OnBeforeSerialize()
    {
        base.OnBeforeSerialize();

        if (battleModule != null)
        {
            battleModule.ID = KEY_ID;
        }
    }
}

public abstract class BattleModule : Equip
{

    public BattleModule(BattleModule data)
    {
        ID = data.ID;
        MaintenceName = data.MaintenceName;
        Description = data.Description;
        Rarity = data.Rarity;
    }
}

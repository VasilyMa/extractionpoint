public class Grenade : BattleModule
{
    public Grenade(Grenade data) : base(data)
    {

    }

    public override Equip Clone()
    { 
        return new Grenade(this);
    }

    public override ItemData GetData()
    {
        return new BattleModuleData(this);
    }

    public override void LoadData(ItemData data)
    {

    }
}

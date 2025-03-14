public class Foot : Armor
{
    public Foot(Armor data) : base(data)
    {

    }

    public override Equip Clone()
    {
        return new Foot(this);
    }
}

public class Body : Armor
{
    public Body(Armor data) : base(data)
    {
    }

    public override Equip Clone()
    {
        return new Body(this);
    }
}

public class Hand : Armor
{
    public Hand(Armor data) : base(data)
    {
    }

    public override Equip Clone()
    {
        return new Hand(this);
    }
}

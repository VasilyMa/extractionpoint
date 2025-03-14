public class Helmet : Armor
{
    public Helmet(Helmet data) : base(data) 
    {

    }

    public override Equip Clone()
    { 
        return new Helmet(this);
    }
}

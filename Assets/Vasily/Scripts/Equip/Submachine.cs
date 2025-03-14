public class Submachine : WeaponPlayer
{
    public Submachine(Submachine data) : base(data)
    {

    }

    public override Equip Clone()
    {
        return new Submachine(this);
    }
}

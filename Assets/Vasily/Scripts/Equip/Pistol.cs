public class Pistol : WeaponPlayer
{
    public Pistol(Pistol data) : base(data)
    {
        
    }

    public override Equip Clone()
    {
        return new Pistol(this);
    }
}

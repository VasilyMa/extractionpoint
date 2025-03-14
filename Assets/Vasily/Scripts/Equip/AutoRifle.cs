public class AutoRifle : WeaponPlayer
{
    public AutoRifle(AutoRifle data) : base(data)
    {

    }

    public override Equip Clone()
    {
        return new AutoRifle(this);
    }

}

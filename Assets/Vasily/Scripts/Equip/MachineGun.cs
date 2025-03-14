public class MachineGun : WeaponPlayer
{ 
    public MachineGun(MachineGun data) : base(data)
    {

    }

    public override Equip Clone()
    {
        return new MachineGun(this); 
    }

}

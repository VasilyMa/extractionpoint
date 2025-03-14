using System.Collections;
using System.Collections.Generic;

public class EquipmentContainer
{
    private PhotonPlayer _owner;
    public WeaponPlayer CurrentWeapon { get; private set; }
    public WeaponPlayer FirstWeapon { get; private set; }
    public WeaponPlayer SecondWeapon { get; private set; }
    public BattleModule BattleModule { get; private set; }


    public EquipmentContainer(PhotonPlayer owner, WeaponPlayer first, WeaponPlayer second, BattleModule battleModule)
    {
        _owner = owner;
        FirstWeapon = first;
        SecondWeapon = second;
        BattleModule = battleModule;

        CurrentWeapon = first ?? second ?? null;

        if(FirstWeapon != null) FirstWeapon.WeaponOwner = _owner;
        if(SecondWeapon != null) SecondWeapon.WeaponOwner = _owner;
    }

    public void EquipItem(Equip equip)
    {

    }

    public void UnequipItem(Equip equip)
    {

    }

    public IEnumerator SwitchWeapon()
    {
        if (CurrentWeapon == FirstWeapon) CurrentWeapon = SecondWeapon;
        else CurrentWeapon = FirstWeapon;

        yield return OnlineState.Instance.RunCoroutine(CurrentWeapon.InPrepare(), CurrentWeapon.onPrepare);
    }
}

public class TestEquipmentContainer
{
    protected List<WeaponPlayer> _weaponPlayers = new List<WeaponPlayer>();

    protected int _currentIndex = -1;
    public TestEquipmentContainer(List<WeaponBase> weaponListBase)
    {
        foreach (var weaponBase in weaponListBase)
        {
            _weaponPlayers.Add(weaponBase.GetEquip<WeaponPlayer>());
        }
    }

    public void Init()
    {
        _weaponPlayers.ForEach(weapon => weapon.Init());
    }

    public WeaponPlayer GetNextWeapon()
    {
        _currentIndex++;

        _currentIndex = _currentIndex >= _weaponPlayers.Count ? 0 : _currentIndex;

        return _weaponPlayers[_currentIndex];
    }
}
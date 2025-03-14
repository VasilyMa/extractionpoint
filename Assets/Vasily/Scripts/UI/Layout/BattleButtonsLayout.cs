using UnityEngine;

public class BattleButtonsLayout : MonoBehaviour
{
    private BattlePanel _battlePanel;
    private WeaponSlotView[] _weaponSlots;

    public BattleButtonsLayout Init(BattlePanel panel)
    {
        _battlePanel = panel;
        _weaponSlots = GetComponentsInChildren<WeaponSlotView>();
        for (int i = 0; i < _weaponSlots.Length; i++) _weaponSlots[i].Init();
        return this;
    }

    public void InitView(EquipmentContainer equipmentContainer)
    {
        if(equipmentContainer.FirstWeapon != null) _weaponSlots[0].InitView(equipmentContainer.FirstWeapon);
        if(equipmentContainer.SecondWeapon != null) _weaponSlots[1].InitView(equipmentContainer.SecondWeapon);
        if(equipmentContainer.BattleModule != null) _weaponSlots[2].InitView(equipmentContainer.SecondWeapon);

        _battlePanel.PhotonPlayer.OnMainWeaponChange += ChangeWeaponSlot;
    }

    void ChangeWeaponSlot(WeaponPlayer weapon)
    {
        _weaponSlots[0].InitView(weapon);
    }

    public void Dispose()
    {
        _battlePanel.PhotonPlayer.OnMainWeaponChange -= ChangeWeaponSlot;
    }
}

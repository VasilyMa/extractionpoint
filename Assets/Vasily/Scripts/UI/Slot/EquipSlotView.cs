using UnityEngine;

public class EquipSlotView : SlotView
{
    protected override void InvokeSlotEvent()
    {
        if (PlayerEntity.Instance.Unequip(_equipData))
        {
            SaveModule.SaveSingleDataToPlayfab<InventoryData>();
            SaveModule.SaveSingleDataToPlayfab<PlayerData>();

            _layout.UpdateView();
        }
    }
}

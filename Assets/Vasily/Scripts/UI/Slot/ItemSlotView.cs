using UnityEngine;

public class ItemSlotView : SlotView
{

    protected override void InvokeSlotEvent()
    {
        if (PlayerEntity.Instance.Equip(InventoryEntity.Instance.CurrentEquipSelected))
        {
            SaveModule.SaveSingleDataToPlayfab<InventoryData>();
            SaveModule.SaveSingleDataToPlayfab<PlayerData>();

            _layout.UpdateView();
        }
        Debug.Log("Invoke equip");
    }
}

public class HandLayout : InventoryLayout
{
    public override void Init(SourcePanel panel)
    {
        base.Init(panel);

        UpdateEquipSlot(PlayerEntity.Instance.HandData);
    }
    public override bool OnOpen()
    {
        UpdateViewSlots(InventoryEntity.Instance.GetEquipByType(SlotViewType.hand));
        UpdateEquipSlot(PlayerEntity.Instance.HandData);

        return base.OnOpen();
    }
    public override void UpdateView()
    {
        UpdateViewSlots(InventoryEntity.Instance.GetEquipByType(SlotViewType.hand));
        UpdateEquipSlot(PlayerEntity.Instance.HandData);
    }
}

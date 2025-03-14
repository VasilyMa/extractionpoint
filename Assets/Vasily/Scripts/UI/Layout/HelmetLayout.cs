public class HelmetLayout : InventoryLayout
{
    public override void Init(SourcePanel panel)
    {
        base.Init(panel);

        UpdateEquipSlot(PlayerEntity.Instance.HelmetData);
    }
    public override bool OnOpen()
    {
        UpdateViewSlots(InventoryEntity.Instance.GetEquipByType(SlotViewType.helmet));
        UpdateEquipSlot(PlayerEntity.Instance.HelmetData);

        return base.OnOpen();
    }

    public override void UpdateView()
    {
        UpdateViewSlots(InventoryEntity.Instance.GetEquipByType(SlotViewType.helmet));
        UpdateEquipSlot(PlayerEntity.Instance.HelmetData);
    }
}


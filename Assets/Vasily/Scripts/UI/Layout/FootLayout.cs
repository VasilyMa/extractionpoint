public class FootLayout : InventoryLayout
{
    public override void Init(SourcePanel panel)
    {
        base.Init(panel);

        UpdateEquipSlot(PlayerEntity.Instance.FootData);
    }
    public override bool OnOpen()
    {
        UpdateViewSlots(InventoryEntity.Instance.GetEquipByType(SlotViewType.foot));
        UpdateEquipSlot(PlayerEntity.Instance.FootData);

        return base.OnOpen();
    }
    public override void UpdateView()
    {
        UpdateViewSlots(InventoryEntity.Instance.GetEquipByType(SlotViewType.foot));
        UpdateEquipSlot(PlayerEntity.Instance.FootData);
    }
}

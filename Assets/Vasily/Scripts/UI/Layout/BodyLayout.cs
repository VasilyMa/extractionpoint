public class BodyLayout : InventoryLayout
{
    public override void Init(SourcePanel panel)
    {
        base.Init(panel);

        UpdateEquipSlot(PlayerEntity.Instance.BodyData);
    }
    public override bool OnOpen()
    {
        UpdateViewSlots(InventoryEntity.Instance.GetEquipByType(SlotViewType.armor));
        UpdateEquipSlot(PlayerEntity.Instance.BodyData);
        return base.OnOpen();
    }
    public override void UpdateView()
    {
        UpdateViewSlots(InventoryEntity.Instance.GetEquipByType(SlotViewType.armor));
        UpdateEquipSlot(PlayerEntity.Instance.BodyData);
    }
}

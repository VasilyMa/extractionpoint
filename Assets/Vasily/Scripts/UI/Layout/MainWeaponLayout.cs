public class MainWeaponLayout : ArmLayout
{
    public override void Init(SourcePanel panel)
    {
        base.Init(panel);

        UpdateEquipSlot(PlayerEntity.Instance.FirstWeaponData);
    }

    public override bool OnOpen()
    {
        UpdateViewSlots(InventoryEntity.Instance.GetEquipByType(SlotViewType.weapon));
        UpdateEquipSlot(PlayerEntity.Instance.FirstWeaponData);

        return base.OnOpen();
    }

    public override void UpdateView()
    {
        UpdateViewSlots(InventoryEntity.Instance.GetEquipByType(SlotViewType.weapon));
        UpdateEquipSlot(PlayerEntity.Instance.FirstWeaponData);
    }
}

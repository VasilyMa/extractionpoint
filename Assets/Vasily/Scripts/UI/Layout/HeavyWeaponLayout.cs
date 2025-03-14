public class HeavyWeaponLayout : ArmLayout
{
    public override void Init(SourcePanel panel)
    {
        base.Init(panel);

        UpdateEquipSlot(PlayerEntity.Instance.SecondWeaponData);
    }
    public override bool OnOpen()
    {
        UpdateViewSlots(InventoryEntity.Instance.GetEquipByType(SlotViewType.heavy));
        UpdateEquipSlot(PlayerEntity.Instance.SecondWeaponData);

        return base.OnOpen();

    }

    public override void UpdateView()
    {
        UpdateViewSlots(InventoryEntity.Instance.GetEquipByType(SlotViewType.heavy));
        UpdateEquipSlot(PlayerEntity.Instance.SecondWeaponData);
    }
}

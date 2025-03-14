public class BattleModuleLayout : ArmLayout
{
    public override void Init(SourcePanel panel)
    {
        base.Init(panel);

        UpdateEquipSlot(PlayerEntity.Instance.BattleModuleData);
    }
    public override bool OnOpen()
    {
        UpdateViewSlots(InventoryEntity.Instance.GetEquipByType(SlotViewType.module));
        UpdateEquipSlot(PlayerEntity.Instance.BattleModuleData);

        return base.OnOpen();

    }

    public override void UpdateView()
    {
        UpdateViewSlots(InventoryEntity.Instance.GetEquipByType(SlotViewType.module));
        UpdateEquipSlot(PlayerEntity.Instance.BattleModuleData);
    }
}

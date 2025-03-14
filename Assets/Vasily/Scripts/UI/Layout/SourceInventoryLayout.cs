using UnityEngine;

public abstract class SourceInventoryLayout : MonoBehaviour
{
    protected InventoryPanel inventoryPanel;
    [SerializeField] protected EquipSlotView _equipSlot;
    [SerializeField] protected RectTransformResize _resizeHeader;
    [SerializeField] protected Transform _btnHeader;

    public virtual void Init(SourcePanel panel)
    {
        _resizeHeader = GetComponentInChildren<RectTransformResize>();
        _equipSlot = GetComponentInChildren<EquipSlotView>().Init<EquipSlotView>(this);
        _btnHeader.gameObject.SetActive(true);
        inventoryPanel = panel as InventoryPanel;
    }
    protected virtual void UpdateEquipSlot(Equip equip)
    {
        _equipSlot.UpdateView(equip);
    }
    public abstract void UpdateView();
    private void OnValidate()
    {
        _equipSlot = GetComponentInChildren<EquipSlotView>();
    }
}

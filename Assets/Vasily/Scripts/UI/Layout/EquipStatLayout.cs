using DG.Tweening;
using System.Collections;

using UnityEngine;
public class EquipStatLayout : MonoBehaviour
{
    InfoEquipmentPanel _panel;
    [SerializeField] Transform _statHolder;
    [SerializeField] Transform _targetOpen;
    [SerializeField] Transform _targetClose;
    StatSlotView[] _groupSlots;

    public void Init(InfoEquipmentPanel panel)
    {
        _panel = panel;
        _groupSlots = GetComponentsInChildren<StatSlotView>();
    }

    public virtual void OnOpen()
    {
        UpdateViewSlots();
        //_statHolder.DOMoveX(_targetOpen.position.x, 0.5f).SetEase(Ease.InCubic).OnComplete(UpdateViewSlots);
    }

    public virtual IEnumerator OnClose()
    {
        yield return null;
        //yield return _statHolder.DOMoveX(_targetClose.position.x, 0.5f).SetEase(Ease.InCubic);
    }

    void UpdateViewSlots()
    {
        var currentEquipSelected = InventoryEntity.Instance.CurrentEquipSelected as WeaponPlayer;

        var stats = currentEquipSelected.GetWeaponStats();

        for (int i = 0; i < _groupSlots.Length; i++)
        {
            string title = stats[i].Name;

            float value = stats[i].MaxValue;

            float fillValue = stats[i].BaseValue / stats[i].MaxValue;

            _groupSlots[i].UpdateView(fillValue, value.ToString(), title);
        }
    }
}

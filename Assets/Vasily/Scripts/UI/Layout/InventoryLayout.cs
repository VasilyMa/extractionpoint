using System.Collections.Generic;

using DG.Tweening;

using UnityEngine;

public abstract class InventoryLayout : SourceInventoryLayout
{
    [SerializeField] protected ItemSlotView[] _groupSlots;

    [SerializeField] protected Transform _inventoryHolder;
    [SerializeField] protected Transform _targetOpen;
    [SerializeField] protected Transform _targetClose;

    bool isOpen;
    bool isClose;

    public override void Init(SourcePanel panel)
    {
        base.Init(panel);

        _groupSlots = GetComponentsInChildren<ItemSlotView>();
        for (int i = 0; i < _groupSlots.Length; i++) _groupSlots[i].Init<ItemSlotView>(this);
    }

    public virtual void ResetLayout()
    {
        if (isOpen) _inventoryHolder.DOMoveY(_targetClose.position.y, 0.5f).SetEase(Ease.InCubic);

        _btnHeader.gameObject.SetActive(true);

        _resizeHeader.ResetSize();

        isOpen = false;
        isClose = false;
    }

    public virtual bool OnOpen()
    {
        if (isOpen) return false;
        else
        {
            _inventoryHolder.DOMoveY(_targetOpen.position.y, 0.5f).SetEase(Ease.InCubic).OnComplete(() => { isOpen = true; });
            return true;
        }
    }

    public virtual void OnClose()
    {
        if (isClose) return;
        isClose = true;

        _resizeHeader.ResizeHeight(0);
        _inventoryHolder.DOMoveY(_targetClose.position.y, 0.5f).SetEase(Ease.InCubic).OnComplete(() =>
        _btnHeader.gameObject.SetActive(false));
    }

    public abstract override void UpdateView();
    protected virtual void UpdateViewSlots(List<Equip> list)
    {
        for (int i = 0; i < _groupSlots.Length; i++)
        {
            if (i >= list.Count)
            {
                _groupSlots[i].UpdateView(null);
                continue;
            }

            _groupSlots[i].UpdateView(list[i]);
        }
    }
}

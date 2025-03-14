using System;
using System.Collections;


using UnityEngine;
using UnityEngine.UI;

public class InfoEquipmentPanel : SourcePanel
{
    [SerializeField] private Button _btnClose;
    [SerializeField] private Button _btnEquip;
    [SerializeField] private Text _nameTitle;
    private EquipStatLayout _equipStatLayout;
    private EquipPerkLayout _perkLayout;

    public override void Init(SourceCanvas canvasParent)
    {
        base.Init(canvasParent);
        _equipStatLayout = GetComponentInChildren<EquipStatLayout>();
        _perkLayout = GetComponentInChildren<EquipPerkLayout>();
        _equipStatLayout.Init(this);
        _btnClose.onClick.AddListener(Close);
        _btnEquip.onClick.AddListener(Equip);
    }

    public override void OnOpen(params Action[] onComplete)
    {
        Action[] complete = new Action[onComplete.Length + 1];

        for (int i = 0; i < onComplete.Length; i++)
        {
            complete[i] = onComplete[i];
        }

        complete[complete.Length - 1] = SetView;

        base.OnOpen(complete);
    }

    public override void OnCLose(params Action[] onComplete)
    {
        if(isOpen) StartCoroutine(OnCloseStats(onComplete));
    }

    IEnumerator OnCloseStats(params Action[] onComplete)
    {
        yield return _equipStatLayout.OnClose();

        Action[] complete = new Action[onComplete.Length + 1];

        for (int i = 0; i < onComplete.Length; i++)
        {
            complete[i] = onComplete[i];
        }

        complete[complete.Length - 1] = ()=> isOpen = false;

        base.OnCLose(onComplete);
    }

    void SetView()
    {
        isOpen = true;
        _nameTitle.text = InventoryEntity.Instance.CurrentEquipSelected.MaintenceName;
        _equipStatLayout.OnOpen();
    }

    void Equip()
    {
        if (InventoryEntity.Instance.CurrentEquipSelected != null)
        {
            if (PlayerEntity.Instance.Equip(InventoryEntity.Instance.CurrentEquipSelected))
            {
                State.Instance.GetCanvas<InventoryCanvas>().GetPanel<InventoryPanel>().UpdateView();

                SaveModule.SaveSingleDataToPlayfab<InventoryData>();
                SaveModule.SaveSingleDataToPlayfab<PlayerData>();

                Close();
            }
        }
    }

    void Close()
    {
        State.Instance.GetCanvas<InventoryCanvas>().OpenPanel<InventoryPanel>();
    }

    public override void OnDipose()
    {
        _btnClose.onClick.RemoveAllListeners();
        _btnEquip.onClick.RemoveAllListeners();
        base.OnDipose();
    }
}

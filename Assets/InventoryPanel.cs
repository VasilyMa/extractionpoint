using UnityEngine;
using UnityEngine.UI;

public class InventoryPanel : SourcePanel
{
    [Header("Buttons Parts Armor")]
    [SerializeField] Button _helmetBtn;
    [SerializeField] Button _bodyBtn;
    [SerializeField] Button _handBtn;
    [SerializeField] Button _footBtn;

    [Header("Buttons Parts Armor")]
    [Space(10)]
    [SerializeField] Button _mainBtn;
    [SerializeField] Button _heavyBtn;
    [SerializeField] Button _moduleBtn;

    InventoryLayout[] _inventoryLayout;
    ArmLayout[] _armLayout;

    public override void Init(SourceCanvas canvasParent)
    {
        base.Init(canvasParent);

        _inventoryLayout = GetComponentsInChildren<InventoryLayout>();

        _armLayout = GetComponentsInChildren<ArmLayout>();

        for (int i = 0; i < _inventoryLayout.Length; i++) _inventoryLayout[i].Init(this);
        for (int i = 0; i < _armLayout.Length; i++) _armLayout[i].Init(this);

        _helmetBtn.onClick.AddListener(OpenHelmet);
        _bodyBtn.onClick.AddListener(OpenBody);
        _handBtn.onClick.AddListener(OpenHand);
        _footBtn.onClick.AddListener(OpenFoot);

        _mainBtn.onClick.AddListener(OpenMain);
        _heavyBtn.onClick.AddListener(OpenHeavy);
        _moduleBtn.onClick.AddListener(OpenModule);
    }
    public void ResetToDefaultInventory()
    {
        for (int i = 0; i < _inventoryLayout.Length; i++) _inventoryLayout[i].ResetLayout();
    }
    void OpenHelmet() => OpenInventoryLayout<HelmetLayout>();
    void OpenBody() => OpenInventoryLayout<BodyLayout>();
    void OpenHand() => OpenInventoryLayout<HandLayout>();
    void OpenFoot() => OpenInventoryLayout<FootLayout>();
    void OpenInventoryLayout<T>() where T : InventoryLayout
    {
        foreach (var layout in _inventoryLayout)
        {
            if (layout is T openedLayout)
            {
                if (!openedLayout.OnOpen())
                {
                    ResetToDefaultInventory();
                    break;
                }
            }
            else layout.OnClose();
        }
    }
    void OpenMain() => OpenArmLayout<MainWeaponLayout>();
    void OpenHeavy() => OpenArmLayout<HeavyWeaponLayout>();
    void OpenModule() => OpenArmLayout<BattleModuleLayout>();
    void OpenArmLayout<T>() where T : ArmLayout
    {
        foreach (var layout in _armLayout)
        {
            if (layout is T openedLayout)
            {
                if (!openedLayout.OnOpen())
                {
                    ResetToDefaultArm();
                    break;
                }
            }
            else layout.OnClose();
        }
    }
    public void ResetToDefaultArm()
    {
        for (int i = 0; i < _armLayout.Length; i++) _armLayout[i].ResetLayout();
    }
    public void UpdateView()
    {
        for (int i = 0; i < _inventoryLayout.Length; i++) _inventoryLayout[i].UpdateView();
        for (int i = 0; i < _armLayout.Length; i++) _armLayout[i].UpdateView();
    }
    public override void OnDipose()
    {
        _helmetBtn.onClick.RemoveAllListeners();
        _bodyBtn.onClick.RemoveAllListeners();
        _handBtn.onClick.RemoveAllListeners();
        _footBtn.onClick.RemoveAllListeners();

        _mainBtn.onClick.RemoveAllListeners();
        _heavyBtn.onClick.RemoveAllListeners();
        _moduleBtn.onClick.RemoveAllListeners();
    }
}

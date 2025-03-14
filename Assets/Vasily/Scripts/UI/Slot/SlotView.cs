using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public abstract class SlotView : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    protected SourceInventoryLayout _layout;
    protected Equip _equipData;
    [SerializeField] protected Image _background;
    [SerializeField] protected Image _icon;

    [SerializeField] protected EventTrigger _trigger;

    protected bool isHolding = false;
    protected Coroutine holdCoroutine;

    public virtual T Init<T>(SourceInventoryLayout layout) where T : SlotView
    {
        _layout = layout;
        return this as T;
    }

    public virtual void UpdateView(Equip equip)
    {
        var interfaceConfig = ConfigModule.GetConfig<InterfaceConfig>();

        _background.sprite = interfaceConfig.DefaultSlotView;
        _icon.enabled = false;

        if (equip == null) return;

        _equipData = equip;

        _icon.enabled = true;
        _icon.sprite = _equipData.Icon;

        switch (_equipData.Rarity)
        {
            case Rarity.common:
                _background.sprite = interfaceConfig.CommonSlotView;
                break;
            case Rarity.uncommon:
                _background.sprite = interfaceConfig.UncommonSlotView;
                break;
            case Rarity.rare:
                _background.sprite = interfaceConfig.RareSlotView;
                break;
            case Rarity.legendary:
                _background.sprite = interfaceConfig.LegendarySlotView;
                break;
            default:
                break;
        }
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        // Запускаем таймер на удержание
        InventoryEntity.Instance.CurrentEquipSelected = _equipData;

        isHolding = true;
        holdCoroutine = StartCoroutine(HoldRoutine());
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        // Если удержание не завершилось, это считается обычным нажатием
        if (isHolding)
        {
            InvokeSlotEvent();
        }

        // Останавливаем корутину, если она ещё выполняется
        if (holdCoroutine != null)
        {
            StopCoroutine(holdCoroutine);
        }

        isHolding = false;
    }

    protected IEnumerator HoldRoutine()
    {
        yield return new WaitForSeconds(1f); // Время удержания 1 секунда

        if (isHolding)
        {
            InvokeInfo();
            isHolding = false; // Блокируем обычное нажатие
        }
    }
    protected virtual void InvokeInfo()
    {
        State.Instance.GetCanvas<InventoryCanvas>().OpenPanel<InfoEquipmentPanel>();
    }

    protected abstract void InvokeSlotEvent();
    protected virtual void OnValidate()
    {
        _background = GetComponent<Image>();
        _icon = transform.GetChild(0).GetComponent<Image>();
    }
}

public enum SlotViewType { weapon, heavy, helmet, armor, hand, foot, module }
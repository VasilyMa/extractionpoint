using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoValueSlotView : MonoBehaviour
{
    [SerializeField] Image _icon;
    [SerializeField] Text _title;
    [SerializeField] PlayerValueSlotType _slotType;

    public void Init()
    {
        switch (_slotType)
        {
            case PlayerValueSlotType.mainInfo:
                PlayerEntity.Instance.OnPlayerInfoChange += ValueUpdate;
                break;
            case PlayerValueSlotType.extraLife:
                PlayerEntity.Instance.OnExtraLifeChange += ValueUpdate;
                break;
            case PlayerValueSlotType.creditValue:
                PlayerEntity.Instance.OnCreditChange += ValueUpdate;
                break;
            case PlayerValueSlotType.barrelValue:
                PlayerEntity.Instance.OnBarrelChange += ValueUpdate;
                break;
            case PlayerValueSlotType.experienceValue:
                PlayerEntity.Instance.OnExperienceChange += ValueUpdate;
                break;
        }
    }

    void ValueUpdate()
    {
        var viewConfig = ConfigModule.GetConfig<InterfaceConfig>();

        switch (_slotType)
        {
            case PlayerValueSlotType.mainInfo:
                var rankConfig = ConfigModule.GetConfig<RankConfig>();
                _icon.sprite = rankConfig.GetByID(PlayerEntity.Instance.RankID).SourceRank.Rank_Icon;
                _title.text = PlayerEntity.Instance.PlayerDisplayName;
                break;
            case PlayerValueSlotType.creditValue:
                _icon.sprite = viewConfig.Credit;
                _title.text = PlayerEntity.Instance.CreditCurrency.ToString();
                break;
            case PlayerValueSlotType.extraLife:
                _icon.sprite = viewConfig.ExtraLife;
                _title.text = PlayerEntity.Instance.ExtraLife.ToString();
                break;
            case PlayerValueSlotType.barrelValue:
                _icon.sprite = viewConfig.Barrel;
                _title.text = PlayerEntity.Instance.BarrelCurrency.ToString();
                break;
            case PlayerValueSlotType.experienceValue:
                _icon.sprite = viewConfig.Exp;
                _title.text = PlayerEntity.Instance.Experience.ToString();
                break;
        }
    }

    public void Dispose()
    {
        switch (_slotType)
        {
            case PlayerValueSlotType.mainInfo:
                PlayerEntity.Instance.OnPlayerInfoChange -= ValueUpdate;
                break;
            case PlayerValueSlotType.extraLife:
                PlayerEntity.Instance.OnExtraLifeChange -= ValueUpdate;
                break;
            case PlayerValueSlotType.creditValue:
                PlayerEntity.Instance.OnCreditChange -= ValueUpdate;
                break;
            case PlayerValueSlotType.barrelValue:
                PlayerEntity.Instance.OnBarrelChange -= ValueUpdate;
                break;
            case PlayerValueSlotType.experienceValue:
                PlayerEntity.Instance.OnExperienceChange -= ValueUpdate;
                break;
        }
    }
}

public enum PlayerValueSlotType { mainInfo, extraLife , creditValue, barrelValue, experienceValue }
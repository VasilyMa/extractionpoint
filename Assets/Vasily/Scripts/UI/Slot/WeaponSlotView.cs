using System;

using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class WeaponSlotView : MonoBehaviour
{
    Button _btn;
    [SerializeField] Image _fill;
    [SerializeField] Image _itemIcon;
    [SerializeField] Text _ammoCount;

    public void Init()
    {
        _btn = GetComponent<Button>();
        gameObject.SetActive(false);
    }

    public void InitView<T>(T data) where T : Equip
    {
        gameObject.SetActive(true);

        switch (data)
        {
            case WeaponPlayer weaponPlayer:

                weaponPlayer.OnReload += OnFillChange;
                weaponPlayer.OnAmmoChange += OnAmmoChange;

                _fill.fillAmount = 1f;
                _itemIcon.sprite = weaponPlayer.Icon;
                _ammoCount.text = $"{weaponPlayer.GetAmmo}/{weaponPlayer.GetAmmo}";

                _btn.onClick.AddListener(Switch);
                break;
            case BattleModule battleModule:
                _btn.onClick.AddListener(Resolove);
                break;
        }
    }

    void OnFillChange(float currentValue, float maxValue) =>  _fill.fillAmount = 1 - (currentValue / maxValue);
    void OnAmmoChange(int currentValue, int maxValue)
    {
        _fill.fillAmount = (float)currentValue / maxValue;
        _ammoCount.text = $"{currentValue}/{maxValue}";
    }
    void Switch()
    {
        OnlineState.Instance.Player.SwitchWeapon();
    }
    void Resolove()
    {
        //todo invoke resolve battle module
    }
}

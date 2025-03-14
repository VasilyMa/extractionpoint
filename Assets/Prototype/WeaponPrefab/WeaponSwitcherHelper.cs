using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitcherHelper : MonoBehaviour
{
    public List<GameObject> weaponPrefabs; // Список префабов оружия
    private GameObject currentWeapon; // Текущее оружие
    private int currentWeaponIndex = 0; // Индекс текущего оружия

    private void Start()
    {
        // Создаем первое оружие в начале игры
        ShowWeapon(currentWeaponIndex);
    }
   
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) // Если нажата клавиша E
        {
            SwitchToNextWeapon();
        }
        else if (Input.GetKeyDown(KeyCode.Q)) // Если нажата клавиша Q
        {
            SwitchToPreviousWeapon();
        }
    }

    private void SwitchToNextWeapon()
    {
        currentWeaponIndex++; // Увеличиваем индекс

        // Зацикливаемся, если индекс выходит за пределы списка
        if (currentWeaponIndex >= weaponPrefabs.Count)
        {
            currentWeaponIndex = 0;
        }

        ShowWeapon(currentWeaponIndex);
    }

    private void SwitchToPreviousWeapon()
    {
        currentWeaponIndex--; // Уменьшаем индекс

        // Зацикливаемся, если индекс становится меньше нуля
        if (currentWeaponIndex < 0)
        {
            currentWeaponIndex = weaponPrefabs.Count - 1;
        }

        ShowWeapon(currentWeaponIndex);
    }

    private void ShowWeapon(int index)
    {
        // Если текущее оружие уже существует, уничтожаем его
        if (currentWeapon != null)
        {
            Destroy(currentWeapon);
        }

        // Создаем новое оружие из префаба и сохраняем ссылку на него
        currentWeapon = Instantiate(weaponPrefabs[index], transform.position, transform.rotation);
        currentWeapon.transform.SetParent(transform); // например, замените transform на объект, в котором держится оружие
        currentWeapon.transform.localPosition = currentWeapon.GetComponent<WeaponView>().WeaponOffset;
        currentWeapon.SetActive(true);
    }
}

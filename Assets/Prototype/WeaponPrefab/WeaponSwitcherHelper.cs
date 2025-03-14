using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitcherHelper : MonoBehaviour
{
    public List<GameObject> weaponPrefabs; // ������ �������� ������
    private GameObject currentWeapon; // ������� ������
    private int currentWeaponIndex = 0; // ������ �������� ������

    private void Start()
    {
        // ������� ������ ������ � ������ ����
        ShowWeapon(currentWeaponIndex);
    }
   
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) // ���� ������ ������� E
        {
            SwitchToNextWeapon();
        }
        else if (Input.GetKeyDown(KeyCode.Q)) // ���� ������ ������� Q
        {
            SwitchToPreviousWeapon();
        }
    }

    private void SwitchToNextWeapon()
    {
        currentWeaponIndex++; // ����������� ������

        // �������������, ���� ������ ������� �� ������� ������
        if (currentWeaponIndex >= weaponPrefabs.Count)
        {
            currentWeaponIndex = 0;
        }

        ShowWeapon(currentWeaponIndex);
    }

    private void SwitchToPreviousWeapon()
    {
        currentWeaponIndex--; // ��������� ������

        // �������������, ���� ������ ���������� ������ ����
        if (currentWeaponIndex < 0)
        {
            currentWeaponIndex = weaponPrefabs.Count - 1;
        }

        ShowWeapon(currentWeaponIndex);
    }

    private void ShowWeapon(int index)
    {
        // ���� ������� ������ ��� ����������, ���������� ���
        if (currentWeapon != null)
        {
            Destroy(currentWeapon);
        }

        // ������� ����� ������ �� ������� � ��������� ������ �� ����
        currentWeapon = Instantiate(weaponPrefabs[index], transform.position, transform.rotation);
        currentWeapon.transform.SetParent(transform); // ��������, �������� transform �� ������, � ������� �������� ������
        currentWeapon.transform.localPosition = currentWeapon.GetComponent<WeaponView>().WeaponOffset;
        currentWeapon.SetActive(true);
    }
}

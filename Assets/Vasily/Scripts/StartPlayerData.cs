using UnityEngine;

public class StartPlayerData : MonoBehaviour
{
    public static StartPlayerData Instance;
    public WeaponBase StartFirstWeapon;

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;

            DontDestroyOnLoad(gameObject);
        }
    }
}

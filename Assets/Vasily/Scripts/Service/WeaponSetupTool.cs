using UnityEditor;
using UnityEngine;
using System.IO;

public class WeaponSetupTool : EditorWindow
{
    private WeaponView weaponViewPrefab;
    private WeaponHandsData weaponHandsData;

    [MenuItem("Tools/Set Up Weapon Hands")]
    public static void ShowWindow()
    {
        GetWindow<WeaponSetupTool>("Set Up Weapon Hands");
    }

    private void OnGUI()
    {
        GUILayout.Label("Weapon Setup Tool", EditorStyles.boldLabel);

        weaponViewPrefab = (WeaponView)EditorGUILayout.ObjectField("Weapon View Prefab", weaponViewPrefab, typeof(WeaponView), true);

        if (weaponHandsData == null && weaponViewPrefab != null)
        {
            string weaponName = weaponViewPrefab.name;
            string configPath = $"Assets/Vasily/Data/WeaponHandsConfigs/{weaponName}Hands.asset";

            // Проверяем, существует ли конфиг, если нет, создаем его
            if (File.Exists(configPath))
            {
                weaponHandsData = AssetDatabase.LoadAssetAtPath<WeaponHandsData>(configPath);
            }
            else
            {
                weaponHandsData = CreateWeaponData(weaponName);
            }
        }

        if (weaponHandsData != null)
        {
            EditorGUILayout.ObjectField("Weapon Data", weaponHandsData, typeof(WeaponHandsData), true);

            // Поля для трансформов рук
            weaponHandsData.leftHandTransform = (Transform)EditorGUILayout.ObjectField("Left Hand Transform", weaponHandsData.leftHandTransform, typeof(Transform), true);
            weaponHandsData.rightHandTransform = (Transform)EditorGUILayout.ObjectField("Right Hand Transform", weaponHandsData.rightHandTransform, typeof(Transform), true);
        }

        if (GUILayout.Button("Save"))
        {
            SaveWeaponData();
        }
    }

    private WeaponHandsData CreateWeaponData(string weaponName)
    {
        weaponName = weaponName.Replace("(Clone)", "").Trim();
        string directory = "Assets/Vasily/Data/WeaponHandsConfigs";
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        WeaponHandsData newWeaponData = ScriptableObject.CreateInstance<WeaponHandsData>();

        string assetPath = $"{directory}/{weaponName}Hands.asset";
        AssetDatabase.CreateAsset(newWeaponData, assetPath);
        AssetDatabase.SaveAssets();

        Debug.Log($"Created new WeaponData at {assetPath}");

        return newWeaponData;
    }

    private void SaveWeaponData()
    {
        if (weaponHandsData == null)
        {
            Debug.LogError("Please assign a Weapon Data asset.");
            return;
        }

        // Сохраняем позиции и вращения трансформов рук
        weaponHandsData.SaveTransforms();

        // Сохраняем данные в конфиге
        EditorUtility.SetDirty(weaponHandsData);
        AssetDatabase.SaveAssets();

        Debug.Log("Weapon data saved successfully.");
    }
}

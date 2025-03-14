using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public class WeaponListExporter : EditorWindow
{
    private string weaponPrefabsPath = "Assets/Prototype/WeaponPrefab/WeaponViewReady";
    private string outputFilePath = "Assets/WeaponList.txt";
    private string[] weaponCategories = { "Assaults", "HMGs", "Pistols", "ShotGuns", "SMGs", "Snipers" };

    [MenuItem("Tools/Export Weapon List")]
    public static void ShowWindow()
    {
        GetWindow<WeaponListExporter>("Export Weapon List");
    }

    private void OnGUI()
    {
        GUILayout.Label("Пути к папкам", EditorStyles.boldLabel);

        weaponPrefabsPath = EditorGUILayout.TextField("Путь к префабам оружия", weaponPrefabsPath);
        outputFilePath = EditorGUILayout.TextField("Файл для экспорта", outputFilePath);

        if (GUILayout.Button("Экспортировать список оружия"))
        {
            ExportWeaponList();
        }
    }

    private void ExportWeaponList()
    {
        List<string> weaponList = new List<string>();

        foreach (string category in weaponCategories)
        {
            string categoryPath = Path.Combine(weaponPrefabsPath, category);
            if (!Directory.Exists(categoryPath)) continue;

            string[] prefabPaths = Directory.GetFiles(categoryPath, "*.prefab", SearchOption.AllDirectories);

            foreach (string prefabPath in prefabPaths)
            {
                string weaponName = Path.GetFileNameWithoutExtension(prefabPath);
                weaponList.Add($"[{category}] {weaponName}");
            }
        }

        File.WriteAllLines(outputFilePath, weaponList);
        AssetDatabase.Refresh();
        Debug.Log($"Экспорт завершён! Файл: {outputFilePath}");
    }
}

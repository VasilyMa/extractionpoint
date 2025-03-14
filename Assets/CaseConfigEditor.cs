using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class CaseConfigEditor : EditorWindow
{
    private string weaponBasePath = "Assets/Data/Equipment/Weapon";
    private string caseBasePath = "Assets/Configs/Cases";
    private Dictionary<string, string[]> cases = new Dictionary<string, string[]>()
    {
        { "Battlefront Case", new string[] { "AK-67", "M16", "M4A1", "G36", "M249", "Intimidator", "Glock-17", "USP-S", "M1911", "Benelli M4", "SPAS-12", "Single charger", "MP5", "UMP45", "SKS", "M200" } },
        { "Warlord Case", new string[] { "TAR21", "FN-SCAR", "Executor", "Hole Puncher", "Minigun", "RPK", "Deagle", "M1911-M", "TEC-9", "AA-12", "SPAS12-U", "VEPR-12", "KRISS Vector", "MP7", "VSS", "Impaler" } },
        { "Phantom Strike Case", new string[] { "AR-337", "G36C", "M16-M", "Ancestor", "Shredder", "MG36", "Glock-19", "SIG-M17", "VP9", "Panzer", "XM1014", "Remington 870", "Desert Tech MDR-C", "Uzi", "Zastava MP22", "Remington 337" } },
        { "Steel Storm Case", new string[] { "FN-SCARH", "M4A4", "Husker", "AR-F", "M240", "Slicer", "Makarov", "P250", "Remington", "USAS-12", "TOZ-34", "Popgun", "MP-9", "Oppressor", "Marksman M110", "Destroyer" } },
        { "Death Whisper Case", new string[] { "Ripper", "PPE Kagua", "AK74-M", "Equalizer", "PKM-M", "AR-17M", "Deagle-M", "Makarov-M", "TAURUS", "AA12-M", "Gen-12", "SAIGA-12", "KRISS M", "AKS-74U", "M107", "SVD" } },
        { "Silent Phantom Case", new string[] { "AK12", "Groza-M", "Famas", "G3A4", "PKM", "Intimidator", "HK45", "M9", "P320", "Benelli M4", "SPAS-12", "Sawed gun", "MP25", "Heeler", "Last SKS", "Pneuma" } },
        { "Crimson Dawn Case", new string[] { "Brat", "Executor", "M4A4-L", "M4A4-M", "Minigun", "Shredder", "MP443", "TEC-9M", "SIG-M17", "TOZ-34", "USAS-12", "Bennelli M4 Super", "CMP9", "Liberator", "SVD-M", "AWP" } }
    };

    private Dictionary<string, WeaponBase> weaponMap = new Dictionary<string, WeaponBase>();
    private List<string> missingWeapons = new List<string>(); // Для хранения ненайденных конфигов

    [MenuItem("Tools/Case Config Generator")]
    public static void ShowWindow()
    {
        GetWindow<CaseConfigEditor>("Case Config Generator");
    }

    private void OnGUI()
    {
        GUILayout.Label("Case Config Generator", EditorStyles.boldLabel);

        weaponBasePath = EditorGUILayout.TextField("Weapon Base Path", weaponBasePath);
        caseBasePath = EditorGUILayout.TextField("Case Base Path", caseBasePath);

        if (GUILayout.Button("Generate Cases"))
        {
            GenerateCases();
        }
    }

    private void GenerateCases()
    {
        LoadWeaponBases();
        CreateCases();
        LogMissingWeapons(); // Вывод списка ненайденных конфигов
    }

    private void LoadWeaponBases()
    {
        weaponMap.Clear();

        // Рекурсивный обход папок для поиска WeaponBase
        string[] files = Directory.GetFiles(weaponBasePath, "*.asset", SearchOption.AllDirectories);

        foreach (string file in files)
        {
            WeaponBase weaponBase = AssetDatabase.LoadAssetAtPath<WeaponBase>(file);

            if (weaponBase != null)
            {
                string maintenceName = weaponBase.GetEquip<Weapon>().MaintenceName;
                if (!string.IsNullOrEmpty(maintenceName))
                {
                    if (!weaponMap.ContainsKey(maintenceName))
                    {
                        weaponMap.Add(maintenceName, weaponBase);
                    }
                    else
                    {
                        Debug.LogWarning($"Duplicate weapon detected: {maintenceName}");
                    }
                }
            }
        }

        Debug.Log($"Loaded {weaponMap.Count} weapons.");
    }

    private void CreateCases()
    {
        if (!Directory.Exists(caseBasePath))
        {
            Directory.CreateDirectory(caseBasePath);
            AssetDatabase.Refresh();
        }

        CaseBase[] createdCases = new CaseBase[cases.Count];
        int index = 0;

        foreach (var caseEntry in cases)
        {
            CaseBase caseBase = ScriptableObject.CreateInstance<CaseBase>();
            caseBase.name = caseEntry.Key;

            Case caseData = new Case(null)
            {
                KEY_ID = caseEntry.Key.Replace(" ", "_"),
                CaseName = caseEntry.Key,
                CasePrice = 1000,
                DropRate = 0.1f,
                possibleEquip = new List<EquipBase>()
            };

            foreach (string weaponName in caseEntry.Value)
            {
                if (weaponMap.TryGetValue(weaponName, out WeaponBase weaponBase))
                {
                    caseData.possibleEquip.Add(weaponBase);
                }
                else
                {
                    missingWeapons.Add(weaponName); // Добавляем в список ненайденных конфигов
                }
            }

            caseBase.SourceCase = caseData;
            createdCases[index] = caseBase;

            AssetDatabase.CreateAsset(caseBase, $"{caseBasePath}/{caseEntry.Key.Replace(" ", "_")}.asset");
            index++;
        }

        // Устанавливаем связи между кейсами
        for (int i = 0; i < createdCases.Length; i++)
        {
          

            EditorUtility.SetDirty(createdCases[i]);
        }

        AssetDatabase.SaveAssets();
        Debug.Log("Cases generated successfully!");
    }

    // Вывод в консоль ненайденных конфигов
    private void LogMissingWeapons()
    {
        if (missingWeapons.Count > 0)
        {
            Debug.LogWarning("=== Missing Weapons ===");
            foreach (string missingWeapon in missingWeapons)
            {
                Debug.LogWarning($"Weapon '{missingWeapon}' not found!");
            }
        }
        else
        {
            Debug.Log("? All weapons found and assigned to cases.");
        }
    }
}

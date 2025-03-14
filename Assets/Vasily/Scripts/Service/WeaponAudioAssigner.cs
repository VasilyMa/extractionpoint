using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class WeaponAudioAssigner : EditorWindow
{
    private string weaponPrefabsPath = "Assets/Prototype/WeaponPrefab/WeaponViewReady";
    private string weaponSoundsPath = "Assets/Prototype/WeaponPrefab/WeaponSounds/sounds/weapons";
    private string[] weaponCategories = { "Assaults", "HMGs", "Pistols", "ShotGuns", "SMGs", "Snipers" };

    [MenuItem("Tools/Weapon Audio Assigner")]
    public static void ShowWindow()
    {
        GetWindow<WeaponAudioAssigner>("Weapon Audio Assigner");
    }

    private void OnGUI()
    {
        GUILayout.Label("Пути к папкам", EditorStyles.boldLabel);

        weaponPrefabsPath = EditorGUILayout.TextField("Путь к префабам оружия", weaponPrefabsPath);
        weaponSoundsPath = EditorGUILayout.TextField("Путь к звукам оружия", weaponSoundsPath);

        if (GUILayout.Button("Присвоить звуки оружию"))
        {
            AssignAudioToWeapons();
        }
    }

    private void AssignAudioToWeapons()
    {
        Dictionary<string, List<string>> prefabPathsByCategory = new Dictionary<string, List<string>>();
        foreach (string category in weaponCategories)
        {
            string path = Path.Combine(weaponPrefabsPath, category);
            if (Directory.Exists(path))
            {
                prefabPathsByCategory[category] = Directory.GetFiles(path, "*.prefab", SearchOption.AllDirectories).ToList();
            }
        }

        Dictionary<string, Dictionary<string, AudioClip>> shootSounds = new Dictionary<string, Dictionary<string, AudioClip>>();
        Dictionary<string, Dictionary<string, AudioClip>> reloadSounds = new Dictionary<string, Dictionary<string, AudioClip>>();

        LoadAudioFiles(weaponSoundsPath, shootSounds, reloadSounds);

        foreach (var category in prefabPathsByCategory.Keys)
        {
            foreach (string prefabPath in prefabPathsByCategory[category])
            {
                GameObject prefab = PrefabUtility.LoadPrefabContents(prefabPath);
                if (prefab != null)
                {
                    WeaponView weaponView = prefab.GetComponent<WeaponView>();

                    if (weaponView != null)
                    {
                        string weaponName = Path.GetFileNameWithoutExtension(prefabPath).ToLower();
                        string cleanWeaponName = NormalizeWeaponName(weaponName);

                        if (weaponView.ShootAudioSource != null)
                        {
                            weaponView.ShootAudioSource.clip = GetBestMatchingAudioClip(cleanWeaponName, shootSounds, category);
                        }

                        if (weaponView.ReloadAudioSource != null)
                        {
                            weaponView.ReloadAudioSource.clip = GetBestMatchingAudioClip(cleanWeaponName, reloadSounds, category);
                        }
                    }

                    PrefabUtility.SaveAsPrefabAsset(prefab, prefabPath);
                    PrefabUtility.UnloadPrefabContents(prefab);
                }
            }
        }

        Debug.Log("Звуки успешно назначены для оружия!");
    }

    private void LoadAudioFiles(string folderPath, Dictionary<string, Dictionary<string, AudioClip>> shootSounds, Dictionary<string, Dictionary<string, AudioClip>> reloadSounds)
    {
        foreach (string category in weaponCategories)
        {
            string categoryPath = Path.Combine(folderPath, category);
            if (!Directory.Exists(categoryPath))
            {
                Debug.LogWarning($"Категория звуков {category} отсутствует.");
                continue;
            }

            shootSounds[category] = new Dictionary<string, AudioClip>();
            reloadSounds[category] = new Dictionary<string, AudioClip>();

            string[] audioFiles = Directory.GetFiles(categoryPath, "*.wav", SearchOption.AllDirectories)
                .Concat(Directory.GetFiles(categoryPath, "*.mp3", SearchOption.AllDirectories))
                .Concat(Directory.GetFiles(categoryPath, "*.ogg", SearchOption.AllDirectories))
                .ToArray();

            foreach (string file in audioFiles)
            {
                string fileName = Path.GetFileNameWithoutExtension(file).ToLower();
                string cleanFileName = NormalizeWeaponName(fileName);
                AudioClip clip = AssetDatabase.LoadAssetAtPath<AudioClip>(file);

                if (clip == null) continue;

                if (fileName.Contains("shoot"))
                {
                    shootSounds[category][cleanFileName.Replace("shoot", "").Trim()] = clip;
                }
                else if (fileName.Contains("reload"))
                {
                    reloadSounds[category][cleanFileName.Replace("reload", "").Trim()] = clip;
                }
            }
        }
    }

    private AudioClip GetBestMatchingAudioClip(string weaponName, Dictionary<string, Dictionary<string, AudioClip>> soundDictionary, string category)
    {
        if (!soundDictionary.ContainsKey(category) || soundDictionary[category].Count == 0)
        {
            Debug.LogWarning($"Нет звуков для категории {category}");
            return null;
        }

        foreach (var kvp in soundDictionary[category])
        {
            if (AreWeaponsFromSameFamily(weaponName, kvp.Key))
            {
                return kvp.Value;
            }
        }

        List<AudioClip> allClips = soundDictionary[category].Values.ToList();
        return allClips.Count > 0 ? allClips[Random.Range(0, allClips.Count)] : null;
    }

    private string NormalizeWeaponName(string input)
    {
        return Regex.Replace(input, @"\d", ""); // Убираем цифры
    }

    private bool AreWeaponsFromSameFamily(string weaponA, string weaponB)
    {
        return NormalizeWeaponName(weaponA) == NormalizeWeaponName(weaponB);
    }
}

using UnityEngine;
using UnityEditor;
using System.IO;

public class SetExcludeLayersInPrefabs : EditorWindow
{
    private const string folderPath = "Assets/Prototype/WeaponPrefab/WeaponViewReady";

    [MenuItem("Tools/Set Exclude Layers in Weapon Prefabs")]
    public static void SetExcludeLayers()
    {
        string[] prefabPaths = Directory.GetFiles(folderPath, "*.prefab", SearchOption.AllDirectories);

        foreach (string path in prefabPaths)
        {
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

            if (prefab == null)
            {
                Debug.LogWarning($"Failed to load prefab at path: {path}");
                continue;
            }

            // Открываем сцену для редактирования префаба
            GameObject instance = PrefabUtility.LoadPrefabContents(path);

            // Находим все объекты с "mag" в имени
            Collider[] colliders = instance.GetComponentsInChildren<Collider>(true);
            foreach (var collider in colliders)
            {
                if (collider.gameObject.name.ToLower().Contains("mag"))
                {
                    Debug.Log($"Setting exclude layers for {collider.gameObject.name} in prefab {path}");

                    // Устанавливаем слои для исключения
                    collider.excludeLayers = LayerMask.GetMask("Player", "Enemy");
                }
            }

            // Сохраняем изменения в префабе
            PrefabUtility.SaveAsPrefabAsset(instance, path);
            PrefabUtility.UnloadPrefabContents(instance);
        }

        // Обновляем базу ассетов
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("Finished setting exclude layers on all matching prefabs.");
    }
}

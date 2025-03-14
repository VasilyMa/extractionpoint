using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using UnityEditor;

using UnityEngine;

public class SaveModuleButtonEditor : MonoBehaviour
{
    private static string SaveDirectory => Path.Combine(Application.persistentDataPath, "Saves");
    private static string SaveFile => Path.Combine(SaveDirectory, "save.data");

    [MenuItem("File/Clear Data", priority = 10)]
    private static void ResetData()
    {
        Debug.Log("Starting data reset...");

        // Проверяем, существует ли директория сохранений
        if (!Directory.Exists(SaveDirectory))
        {
            Debug.LogWarning($"Save directory not found at path: {SaveDirectory}. Creating new directory...");
            Directory.CreateDirectory(SaveDirectory);
        }

        try
        {
            // Удаляем все файлы в директории сохранений
            foreach (var file in Directory.GetFiles(SaveDirectory, "*.data"))
            {
                File.Delete(file);
                Debug.Log($"Deleted save file: {file}");
            }

            // Сбрасываем все данные через SaveModule
            SaveModule.ResetAllData();

            Debug.Log($"All save data reset successfully in {SaveDirectory}");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Failed to reset data: {ex.Message}");
        }
    }

    [MenuItem("File/Open Data", priority = 11)]
    private static void OpenData()
    {
        if (!Directory.Exists(SaveDirectory))
        {
            Directory.CreateDirectory(SaveDirectory);
        }

        // Открыть папку в проводнике
        Application.OpenURL("file://" + SaveDirectory); 
    }
}

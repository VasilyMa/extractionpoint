using UnityEngine;
using UnityEditor;
using System.Collections;

public class WeaponScreenshotTaker : MonoBehaviour
{
    public string weaponPrefabsPath = "Assets/Prototype/WeaponPrefab/WeaponViewReady"; // Путь к вашим префабам
    public string screenshotFolderPath = "Assets/Import/WeaponScreenshots/AAAA"; // Путь для сохранения скриншотов
    public Camera screenshotCamera; // Укажите камеру для скриншота

    void Start()
    {
        TakeScreenshots();
    }

    private void TakeScreenshots()
    {
        // Получаем все префабы из указанной папки и её подпапок
        string[] prefabGuids = AssetDatabase.FindAssets("t:Prefab", new[] { weaponPrefabsPath });

        foreach (string guid in prefabGuids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            GameObject weaponPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);

            if (weaponPrefab != null)
            {
                // Создаем новый экземпляр префаба в сцене
                GameObject weaponInstance = Instantiate(weaponPrefab);
                weaponInstance.transform.parent = transform;

                string fileName = screenshotFolderPath + "/" + weaponInstance.name.Replace("(Clone)", "").Trim() + ".png";

                // Убедимся, что у нас установлен объект камеры
                if (screenshotCamera != null)
                {
                    StartCoroutine(CaptureScreenshot(fileName, weaponInstance));
                }
                else
                {
                    Debug.LogError("Сначала установите камеру для скриншота.");
                }

                // Удаляем экземпляр после завершения скриншота
                Destroy(weaponInstance);
            }
        }
    }

    private IEnumerator CaptureScreenshot(string fileName, GameObject weaponInstance)
    {
        // Создаем RenderTexture
        RenderTexture renderTexture = new RenderTexture(1920, 1080, 24); // Размер может быть изменен по вашему желанию
        RenderTexture currentRT = RenderTexture.active;


        // Устанавливаем RenderTexture
        screenshotCamera.targetTexture = renderTexture;
        RenderTexture.active = renderTexture;

        // Ждем один кадр
        yield return new WaitForEndOfFrame();

        // Делаем скриншот с альфа-каналом
        screenshotCamera.Render();

        // Создаем текстуру для сохранения
        Texture2D screenshot = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false);
        screenshot.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        screenshot.Apply();

        // Сбрасываем настройки
        screenshotCamera.targetTexture = null;
        RenderTexture.active = currentRT;

        // Сохраняем текстуру в PNG
        byte[] bytes = screenshot.EncodeToPNG();
        System.IO.File.WriteAllBytes(fileName, bytes);

        // Очищаем ресурсы

        Destroy(screenshot);
        Destroy(renderTexture);

        Debug.Log($"Скриншот сохранен: {fileName}");
    }
}

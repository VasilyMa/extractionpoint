using UnityEngine;
using System.IO;

public class RenderTextureToTexture2D : MonoBehaviour
{
    public RenderTexture renderTexture; // Ссылка на вашу RenderTexture
    private Texture2D texture2D; // Здесь будет храниться сконвертированное значение

    void Start()
    {
        if (renderTexture == null)
        {
            Debug.LogError("RenderTexture не задан!");
            return;
        }

        // Создаем Texture2D с разрешением RenderTexture
        texture2D = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGBA32, false);

        // Запускаем метод создания текстуры через 1 секунду
        Invoke("CreateTexture", 1f);
    }

    public void CreateTexture()
    {
        // Копируем данные из RenderTexture в Texture2D
        RenderTexture currentActiveRT = RenderTexture.active; // Сохраняем текущее активное RenderTexture
        RenderTexture.active = renderTexture; // Устанавливаем активное RenderTexture

        texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        texture2D.Apply(); // Применяем изменения

        RenderTexture.active = currentActiveRT; // Возвращаем активное RenderTexture обратно

        // Сохраняем Texture2D в файл с указанием имени файла
        SaveTextureToFile(texture2D, "Assets/Textures/savedTexture.png");
    }

    private void SaveTextureToFile(Texture2D texture, string filePath)
    {
        // Убедитесь, что директория существует
        string directoryPath = Path.GetDirectoryName(filePath);
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        // Кодируем текстуру в PNG
        byte[] bytes = texture.EncodeToPNG();

        // Записываем в файл
        File.WriteAllBytes(filePath, bytes);

        // Применяем изменения и обновляем AssetDatabase
#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#endif
    }
}

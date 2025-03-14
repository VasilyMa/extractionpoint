using UnityEngine;
using System.IO;

public class RenderTextureToTexture2D : MonoBehaviour
{
    public RenderTexture renderTexture; // ������ �� ���� RenderTexture
    private Texture2D texture2D; // ����� ����� ��������� ����������������� ��������

    void Start()
    {
        if (renderTexture == null)
        {
            Debug.LogError("RenderTexture �� �����!");
            return;
        }

        // ������� Texture2D � ����������� RenderTexture
        texture2D = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGBA32, false);

        // ��������� ����� �������� �������� ����� 1 �������
        Invoke("CreateTexture", 1f);
    }

    public void CreateTexture()
    {
        // �������� ������ �� RenderTexture � Texture2D
        RenderTexture currentActiveRT = RenderTexture.active; // ��������� ������� �������� RenderTexture
        RenderTexture.active = renderTexture; // ������������� �������� RenderTexture

        texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        texture2D.Apply(); // ��������� ���������

        RenderTexture.active = currentActiveRT; // ���������� �������� RenderTexture �������

        // ��������� Texture2D � ���� � ��������� ����� �����
        SaveTextureToFile(texture2D, "Assets/Textures/savedTexture.png");
    }

    private void SaveTextureToFile(Texture2D texture, string filePath)
    {
        // ���������, ��� ���������� ����������
        string directoryPath = Path.GetDirectoryName(filePath);
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        // �������� �������� � PNG
        byte[] bytes = texture.EncodeToPNG();

        // ���������� � ����
        File.WriteAllBytes(filePath, bytes);

        // ��������� ��������� � ��������� AssetDatabase
#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#endif
    }
}

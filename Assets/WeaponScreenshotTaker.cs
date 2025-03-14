using UnityEngine;
using UnityEditor;
using System.Collections;

public class WeaponScreenshotTaker : MonoBehaviour
{
    public string weaponPrefabsPath = "Assets/Prototype/WeaponPrefab/WeaponViewReady"; // ���� � ����� ��������
    public string screenshotFolderPath = "Assets/Import/WeaponScreenshots/AAAA"; // ���� ��� ���������� ����������
    public Camera screenshotCamera; // ������� ������ ��� ���������

    void Start()
    {
        TakeScreenshots();
    }

    private void TakeScreenshots()
    {
        // �������� ��� ������� �� ��������� ����� � � ��������
        string[] prefabGuids = AssetDatabase.FindAssets("t:Prefab", new[] { weaponPrefabsPath });

        foreach (string guid in prefabGuids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            GameObject weaponPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);

            if (weaponPrefab != null)
            {
                // ������� ����� ��������� ������� � �����
                GameObject weaponInstance = Instantiate(weaponPrefab);
                weaponInstance.transform.parent = transform;

                string fileName = screenshotFolderPath + "/" + weaponInstance.name.Replace("(Clone)", "").Trim() + ".png";

                // ��������, ��� � ��� ���������� ������ ������
                if (screenshotCamera != null)
                {
                    StartCoroutine(CaptureScreenshot(fileName, weaponInstance));
                }
                else
                {
                    Debug.LogError("������� ���������� ������ ��� ���������.");
                }

                // ������� ��������� ����� ���������� ���������
                Destroy(weaponInstance);
            }
        }
    }

    private IEnumerator CaptureScreenshot(string fileName, GameObject weaponInstance)
    {
        // ������� RenderTexture
        RenderTexture renderTexture = new RenderTexture(1920, 1080, 24); // ������ ����� ���� ������� �� ������ �������
        RenderTexture currentRT = RenderTexture.active;


        // ������������� RenderTexture
        screenshotCamera.targetTexture = renderTexture;
        RenderTexture.active = renderTexture;

        // ���� ���� ����
        yield return new WaitForEndOfFrame();

        // ������ �������� � �����-�������
        screenshotCamera.Render();

        // ������� �������� ��� ����������
        Texture2D screenshot = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false);
        screenshot.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        screenshot.Apply();

        // ���������� ���������
        screenshotCamera.targetTexture = null;
        RenderTexture.active = currentRT;

        // ��������� �������� � PNG
        byte[] bytes = screenshot.EncodeToPNG();
        System.IO.File.WriteAllBytes(fileName, bytes);

        // ������� �������

        Destroy(screenshot);
        Destroy(renderTexture);

        Debug.Log($"�������� ��������: {fileName}");
    }
}

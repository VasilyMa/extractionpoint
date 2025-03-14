using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Recorder;
using UnityEditor.Recorder.Input;
using UnityEngine;
using UnityEngine.Recorder;

public class WeaponSwitcher : MonoBehaviour
{
    public List<GameObject> weaponPrefabs; // ������ �������� ������
    private GameObject currentWeapon; // ������� ������
    private int currentWeaponIndex = 0; // ������ �������� ������
    public ImageRecorderSettings settings;
    private void Start()
    {
        settings.CaptureAlpha = true;
        // ������� ������ ������ � ������ ����
        ShowWeapon(currentWeaponIndex);
        // ��������� ����� ����� �� ������ ������� ������
        UpdateRecorderFileName(currentWeapon);


    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D)) // ���� ������ ������� D
        {
            SwitchToNextWeapon();
        }
        else if (Input.GetKeyDown(KeyCode.A)) // ���� ������ ������� A
        {
            SwitchToPreviousWeapon();
        }
    }
    private void OnValidate()
    {
        LoadWeaponPrefabs();
    }

    private void LoadWeaponPrefabs()
    {
        weaponPrefabs.Clear(); // ������� ������ ����� ���������

        string folderPath = "Assets/Prototype/WeaponPrefab/WeaponViewReady"; // ������� ���� � ����� � ���������
        string[] prefabFiles = Directory.GetFiles(folderPath, "*.prefab", SearchOption.AllDirectories);

        foreach (string prefabPath in prefabFiles)
        {
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
            if (prefab != null)
            {
                weaponPrefabs.Add(prefab);
            }
        }

        Debug.Log($"������� {weaponPrefabs.Count} �������� ������.");
    }
    private void SwitchToNextWeapon()
    {
        currentWeaponIndex++; // ����������� ������
        if (currentWeaponIndex >= weaponPrefabs.Count) // �������������, ���� ������ ������� �� ������� ������
        {
            currentWeaponIndex = 0;
        }
        ShowWeapon(currentWeaponIndex);
    }

    private void SwitchToPreviousWeapon()
    {
        currentWeaponIndex--; // ��������� ������
        if (currentWeaponIndex < 0) // �������������, ���� ������ ���������� ������ ����
        {
            currentWeaponIndex = weaponPrefabs.Count - 1;
        }
        ShowWeapon(currentWeaponIndex);
    }

    private void ShowWeapon(int index)
    {
        // ���� ������� ������ ��� ����������, ���������� ���
        if (currentWeapon != null)
        {
            Destroy(currentWeapon);
        }

        // ������� ����� ������ �� ������� � ��������� ������ �� ����
        currentWeapon = Instantiate(weaponPrefabs[index], transform.position, transform.rotation);
        currentWeapon.SetActive(true);

        // ������������� ��� ��������� WeaponDisplay ��� ���������� ���������
        currentWeapon.transform.SetParent(transform);

        // ��������� ��� ������ � RecorderSettings
        UpdateRecorderFileName(currentWeapon);
    }

    private void UpdateRecorderFileName(GameObject weapon)
    {
            // �������� ��� ������ ��� "(Clone)"
            string weaponName = weapon.name.Replace("(Clone)", "").Trim();

            settings.FileNameGenerator.FileName = weaponName;/*
            var controllerSettings = ScriptableObject.CreateInstance<RecorderControllerSettings>();
            var TestRecorderController = new RecorderController(controllerSettings);

            var videoRecorder = ScriptableObject.CreateInstance<ImageRecorderSettings>();
            videoRecorder.name = "My Video Recorder";
            videoRecorder.Enabled = true;

            videoRecorder.imageInputSettings = new GameViewInputSettings
            {
                OutputWidth = 400,
                OutputHeight = 400
            };
            videoRecorder.FileNameGenerator.FileName = weaponName;

            controllerSettings.AddRecorderSettings(videoRecorder);
            controllerSettings.SetRecordModeToFrameInterval(0, 59); // 2s @ 30 FPS
            controllerSettings.FrameRate = 30;

            TestRecorderController.PrepareRecording();*/
    }
}

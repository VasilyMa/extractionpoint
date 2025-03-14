using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Recorder;
using UnityEditor.Recorder.Input;
using UnityEngine;
using UnityEngine.Recorder;

public class WeaponSwitcher : MonoBehaviour
{
    public List<GameObject> weaponPrefabs; // Список префабов оружия
    private GameObject currentWeapon; // Текущее оружие
    private int currentWeaponIndex = 0; // Индекс текущего оружия
    public ImageRecorderSettings settings;
    private void Start()
    {
        settings.CaptureAlpha = true;
        // Создаем первое оружие в начале игры
        ShowWeapon(currentWeaponIndex);
        // Настройка имени файла на основе первого оружия
        UpdateRecorderFileName(currentWeapon);


    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D)) // Если нажата клавиша D
        {
            SwitchToNextWeapon();
        }
        else if (Input.GetKeyDown(KeyCode.A)) // Если нажата клавиша A
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
        weaponPrefabs.Clear(); // Очищаем список перед загрузкой

        string folderPath = "Assets/Prototype/WeaponPrefab/WeaponViewReady"; // Укажите путь к папке с префабами
        string[] prefabFiles = Directory.GetFiles(folderPath, "*.prefab", SearchOption.AllDirectories);

        foreach (string prefabPath in prefabFiles)
        {
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
            if (prefab != null)
            {
                weaponPrefabs.Add(prefab);
            }
        }

        Debug.Log($"Найдено {weaponPrefabs.Count} префабов оружия.");
    }
    private void SwitchToNextWeapon()
    {
        currentWeaponIndex++; // Увеличиваем индекс
        if (currentWeaponIndex >= weaponPrefabs.Count) // Зацикливаемся, если индекс выходит за пределы списка
        {
            currentWeaponIndex = 0;
        }
        ShowWeapon(currentWeaponIndex);
    }

    private void SwitchToPreviousWeapon()
    {
        currentWeaponIndex--; // Уменьшаем индекс
        if (currentWeaponIndex < 0) // Зацикливаемся, если индекс становится меньше нуля
        {
            currentWeaponIndex = weaponPrefabs.Count - 1;
        }
        ShowWeapon(currentWeaponIndex);
    }

    private void ShowWeapon(int index)
    {
        // Если текущее оружие уже существует, уничтожаем его
        if (currentWeapon != null)
        {
            Destroy(currentWeapon);
        }

        // Создаем новое оружие из префаба и сохраняем ссылку на него
        currentWeapon = Instantiate(weaponPrefabs[index], transform.position, transform.rotation);
        currentWeapon.SetActive(true);

        // Устанавливаем его родителем WeaponDisplay для управления иерархией
        currentWeapon.transform.SetParent(transform);

        // Обновляем имя записи в RecorderSettings
        UpdateRecorderFileName(currentWeapon);
    }

    private void UpdateRecorderFileName(GameObject weapon)
    {
            // Получаем имя оружия без "(Clone)"
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

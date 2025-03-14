using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

using PlayFab;
using PlayFab.ClientModels;

using UnityEngine;

public static class SaveModule
{
    private static readonly Dictionary<Type, IDatable> _registeredData = new Dictionary<Type, IDatable>();
    private static string SaveDirectory => Path.Combine(Application.persistentDataPath, "Saves");

    /// <summary>
    /// Initializes the SaveModule by registering all IDatable instances and loading their data.
    /// </summary>
    public static void Initialize()
    {
        Debug.Log("Start finding all data to register");
        
        if (!Directory.Exists(SaveDirectory)) Directory.CreateDirectory(SaveDirectory);

        // Find all IDatable
        var datableTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => typeof(IDatable).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract);

        foreach (var type in datableTypes)
        {
            // Create instances and add to save
            try
            {
                if (Activator.CreateInstance(type) is IDatable datableInstance)
                {
                    RegisterData(datableInstance);
                }
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }

        Debug.Log("All data registered successfully");

        Debug.Log("Start loading data");

        LoadAllData();
    }

    private static void InitializeDefaultData(Type type)
    {
        try
        {
            if (Activator.CreateInstance(type) is IDatable newData)
            {
                _registeredData[type] = newData;
                SaveSingleDataToPlayfab(type); // Сразу сохраняем
                Debug.Log($"Initialized new binary data for {type.Name}");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error initializing default data for {type.Name}: {ex.Message}");
        }
    }

    public static void LoadAllDataFromPlayFab(Action onSuccess = null, Action<string> onFailure = null)
    {
        Debug.Log("Attempting to load all binary data from PlayFab...");

        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), result =>
        {
            var keys = _registeredData.Keys.ToList(); // Получаем список всех типов данных

            foreach (var type in keys)
            {
                try
                {
                    if (result.Data.ContainsKey(type.Name)) // Проверяем, есть ли данные в PlayFab
                    {
                        string base64Data = result.Data[type.Name].Value;
                        byte[] binaryData = Convert.FromBase64String(base64Data);

                        using (MemoryStream stream = new MemoryStream(binaryData))
                        {
                            BinaryFormatter formatter = new BinaryFormatter();
                            var loadedData = (IDatable)formatter.Deserialize(stream);
                            _registeredData[type] = loadedData;
                        }

                        Debug.Log($"Binary data of type {type.Name} loaded successfully from PlayFab.");
                    }
                    else
                    {
                        Debug.LogWarning($"Data of type {type.Name} not found in PlayFab. Initializing new data...");
                        InitializeDefaultData(type);
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Error loading binary data of type {type.Name}: {ex.Message}");
                }
            }

            Debug.Log("All binary data loaded successfully from PlayFab.");
            onSuccess?.Invoke();
        },
        error =>
        {
            Debug.LogError($"Error retrieving binary data from PlayFab: {error.GenerateErrorReport()}");
            onFailure?.Invoke(error.ErrorMessage);
        });
    }

    /// <summary>
    /// Saves all registered data instances to their respective files.
    /// </summary>
    public static void SaveAllData()
    {
        foreach (var type in _registeredData.Keys)
        {
            Debug.Log($"Attempting to save data of type {type.Name}...");

            try
            {
                var data = _registeredData[type];
                string savePath = Path.Combine(SaveDirectory, $"{type.Name}.data");

                if (data is IIgnorable ignorable)
                    if (ignorable.IsAllSaveIgnored) continue;

                data.ProcessUpdataData();

                using (FileStream stream = new FileStream(savePath, FileMode.Create, FileAccess.Write))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(stream, data);
                }

                Debug.Log($"Data of type {type.Name} saved successfully to {savePath}");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error while saving data of type {type.Name}: {ex.Message}");
            }
        }

        Debug.Log("All data saved successfully");
    }

    /// <summary>
    /// Loads all registered data instances from their respective files.
    /// </summary>
    public static void LoadAllData()
    {
        // Создаём статичный список ключей для безопасного перебора
        var keys = _registeredData.Keys.ToList();

        foreach (var type in keys)
        {
            Debug.Log($"Attempting to load data of type {type.Name}...");

            try
            {
                string savePath = Path.Combine(SaveDirectory, $"{type.Name}.data");

                if (type is IIgnorable ignorable)
                    if (ignorable.IsAllLoadIgnored) continue;
                

                if (!File.Exists(savePath))
                {
                    Debug.LogWarning($"Save file for {type.Name} does not exist. Creating new instance.");
                    if (Activator.CreateInstance(type) is IDatable newData)
                    {
                        _registeredData[type] = newData;

                        using (FileStream stream = new FileStream(savePath, FileMode.Create, FileAccess.Write))
                        {
                            BinaryFormatter formatter = new BinaryFormatter();
                            formatter.Serialize(stream, newData);
                        }
                    }
                    continue;
                }

                using (FileStream stream = new FileStream(savePath, FileMode.Open, FileAccess.Read))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    var loadedData = (IDatable)formatter.Deserialize(stream);
                    _registeredData[type] = loadedData;
                }

                Debug.Log($"Data of type {type.Name} loaded successfully from {savePath}");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error while loading data of type {type.Name}: {ex.Message}");
            }
        }

        Debug.Log("All data loaded successfully");
    }
    /// <summary>
    /// Special method for data operation
    /// </summary>
    private static T EnsureData<T>(Func<T> creationLogic, string errorMessage, string successMessage = null) where T : IDatable
    {
        var type = typeof(T);

        if (_registeredData.TryGetValue(type, out var data))
        {
            return (T)data;
        }

        try
        {
            // Создаем данные
            var newData = creationLogic();
            RegisterData(newData);
            Debug.Log(successMessage ?? $"Данные типа {type.Name} успешно созданы и зарегистрированы.");
            return newData;
        }
        catch (Exception ex)
        {
            Debug.LogError($"{errorMessage}: {ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// Get data of type T, if can't get, create them and register 
    /// </summary>
    public static T GetData<T>() where T : IDatable, new()
    {
        return EnsureData(
            creationLogic: () => new T(),
            errorMessage: $"Не удалось создать данные типа {typeof(T).Name}",
            successMessage: $"Данные типа {typeof(T).Name} успешно созданы и добавлены."
        );
    }

    /// <summary>
    /// Save data of type T
    /// </summary>
    public static void SaveSingleData<T>() where T : IDatable
    {
        EnsureData<T>(
            creationLogic: () => throw new Exception($"Данные типа {typeof(T).Name} отсутствуют для сохранения."),
            errorMessage: $"Ошибка при сохранении данных типа {typeof(T).Name}"
        );

        try
        {
            string savePath = GetSavePath<T>();

            var data = _registeredData[typeof(T)];

            data.ProcessUpdataData();

            using (FileStream stream = new FileStream(savePath, FileMode.Create, FileAccess.Write))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, data);
            }

            Debug.Log($"Data type of {typeof(T).Name} savved success.");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Ошибка при сохранении данных типа {typeof(T).Name}: {ex.Message}");
        }
    }

    public static void SaveSingleDataToPlayfab(Type type, Action onSuccess = null, Action<string> onFailure = null)
    {
        if (!_registeredData.ContainsKey(type))
        {
            Debug.LogError($"Attempted to save unregistered data type: {type.Name}");
            onFailure?.Invoke($"Data type {type.Name} is not registered.");
            return;
        }

        var data = _registeredData[type];
        data.ProcessUpdataData();

        //Сериализация в бинарный формат
        byte[] binaryData;

        using (MemoryStream stream = new MemoryStream())
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, data);
            binaryData = stream.ToArray();
        }

        // Конвертация в Base64 (для сохранения как строка)
        string base64Data = Convert.ToBase64String(binaryData);

        // Проверка размера данных (максимум 10 KB)
        if (base64Data.Length > 10000)
        {
            Debug.LogError($"Error: Serialized data for {type.Name} exceeds PlayFab limit (10 KB). Size: {base64Data.Length / 1024} KB");
            onFailure?.Invoke($"Data size exceeds 10 KB limit. Cannot save to PlayFab.");
            return;
        }

        // Сохранение данных в PlayFab
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string> { { type.Name, base64Data } },
            Permission = UserDataPermission.Public // Разрешаем обновление данных
        },
        result =>
        {
            Debug.Log($"Binary data of type {type.Name} saved successfully to PlayFab.");
            onSuccess?.Invoke();
        },
        error =>
        {
            Debug.LogError($"Error saving binary data of type {type.Name}: {error.GenerateErrorReport()}");

            // Если ошибка 409 Conflict, выводим дополнительную информацию
            if (error.Error == PlayFabErrorCode.DataUpdateRateExceeded)
            {
                Debug.LogError("PlayFab Error: Too many updates in a short time! Try again later.");
            }
            else if (error.Error == PlayFabErrorCode.ContentQuotaExceeded)
            {
                Debug.LogError("PlayFab Error: Data is too large! Consider compressing it.");
            }

            onFailure?.Invoke(error.ErrorMessage);
        });
    }

    public static void SaveSingleDataToPlayfab<T>() where T : IDatable
    {
        EnsureData<T>(
        creationLogic: () => throw new Exception($"Данные типа {typeof(T).Name} отсутствуют для сохранения."),
        errorMessage: $"Ошибка при сохранении данных типа {typeof(T).Name}"
    );

        try
        {
            string savePath = GetSavePath<T>();

            var data = _registeredData[typeof(T)];
            data.ProcessUpdataData();

            //Сериализация в бинарный формат
            byte[] binaryData;
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, data);
                binaryData = stream.ToArray();
            }

            // Конвертируем в Base64 для PlayFab
            string base64Data = Convert.ToBase64String(binaryData);

            // Уникальный ключ для этого типа данных (например, "InventoryData")
            string dataKey = typeof(T).Name;

            // Отправляем в PlayFab
            var request = new UpdateUserDataRequest
            {
                Data = new Dictionary<string, string> {{ dataKey, base64Data } },
                Permission = UserDataPermission.Public
            };

            PlayFabClientAPI.UpdateUserData(request, result =>
            {
                Debug.Log($"[{dataKey}] успешно сохранены в PlayFab.");

            }, error =>
            {
                Debug.LogError($"Ошибка сохранения [{dataKey}] в PlayFab: {error.GenerateErrorReport()}");
            });
        }
        catch (Exception ex)
        {
            Debug.LogError($"Ошибка при сохранении [{typeof(T).Name}] в PlayFab: {ex.Message}");
        }
    }

    /// <summary>
    /// Load data type of T 
    /// </summary>
    public static void LoadSingleData<T>() where T : IDatable, new()
    {
        Debug.Log($"Start single load {typeof(T)}");

        EnsureData<T>(
            creationLogic: () => new T(),
            errorMessage: $"Ошибка при загрузке данных типа {typeof(T).Name}"
        );

        try
        {
            string savePath = GetSavePath<T>();
            if (!File.Exists(savePath))
            {
                Debug.LogWarning($"Файл сохранения для данных типа {typeof(T).Name} отсутствует. Создаются новые данные.");
                SaveSingleData<T>();
                return;
            }

            using (FileStream stream = new FileStream(savePath, FileMode.Open, FileAccess.Read))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                var loadedData = (T)formatter.Deserialize(stream);
                _registeredData[typeof(T)] = loadedData;
            }

            Debug.Log($"Данные типа {typeof(T).Name} успешно загружены.");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Ошибка при загрузке данных типа {typeof(T).Name}: {ex.Message}");
        }
    }

    /// <summary>
    /// Load specific game data from PlayFab
    /// </summary>
    public static void LoadSingleFromPlayFab<T>(Action<T> onLoaded) where T : IDatable, new()
    {
        string dataKey = typeof(T).Name;

        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), result =>
        {
            if (result.Data != null && result.Data.ContainsKey(dataKey))
            {
                try
                {
                    string base64Data = result.Data[dataKey].Value;
                    byte[] binaryData = Convert.FromBase64String(base64Data);

                    using (MemoryStream stream = new MemoryStream(binaryData))
                    {
                        BinaryFormatter formatter = new BinaryFormatter();
                        T loadedData = (T)formatter.Deserialize(stream);

                        _registeredData[typeof(T)] = loadedData; // Кэшируем данные
                        onLoaded?.Invoke(loadedData);

                        Debug.Log($"[{dataKey}] загружены из PlayFab.");
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Ошибка при разборе [{dataKey}] из PlayFab: {ex.Message}");
                }
            }
            else
            {
                Debug.LogWarning($"Данные [{dataKey}] в PlayFab отсутствуют.");
            }
        }, error =>
        {
            Debug.LogError($"Ошибка загрузки [{dataKey}] из PlayFab: {error.GenerateErrorReport()}");
        });
    }

    /// <summary>
    /// Clear data type of T
    /// </summary>
    public static void DeleteData<T>() where T : IDatable
    {
        try
        {
            string savePath = GetSavePath<T>();
            if (File.Exists(savePath))
            {
                File.Delete(savePath);
                Debug.Log($"Файл данных типа {typeof(T).Name} успешно удалён.");
            }

            if (_registeredData.ContainsKey(typeof(T)))
            {
                _registeredData.Remove(typeof(T));
                Debug.Log($"Данные типа {typeof(T).Name} успешно удалены из памяти.");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Ошибка при удалении данных типа {typeof(T).Name}: {ex.Message}");
        }
    }

    /// <summary>
    /// Register new data in dictionary
    /// </summary>
    public static void RegisterData(IDatable data)
    {
        var type = data.GetType();
        if (!_registeredData.ContainsKey(type))
        {
            _registeredData[type] = data;

            Debug.Log($"[<color=cyan>{data.DATA_ID}</color>] data is registered successfully");
        }
    }

    /// <summary>
    /// Clears all data by deleting existing save files and creating fresh instances for all IDatable types.
    /// </summary>
    public static void ResetAllData()
    {
        Debug.Log("Starting reset of all data...");

        // Найти все типы, которые реализуют IDatable
        var datableTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => typeof(IDatable).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract);

        foreach (var type in datableTypes)
        {
            Debug.Log($"Resetting data of type {type.Name}...");

            // Удалить файл сохранения, если он существует
            string savePath = Path.Combine(SaveDirectory, $"{type.Name}.data");
            if (File.Exists(savePath))
            {
                try
                {
                    File.Delete(savePath);
                    Debug.Log($"Deleted save file for {type.Name} at {savePath}");
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Failed to delete save file for {type.Name}: {ex.Message}");
                    continue;
                }
            }

            // Создать новый экземпляр данных и зарегистрировать
            try
            {
                if (Activator.CreateInstance(type) is IDatable newData)
                {
                    if (!_registeredData.ContainsKey(type))
                    {
                        _registeredData[type] = newData;
                    }
                    else
                    {
                        _registeredData[type] = newData; // Перезаписать данные, если они уже есть
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to reset data of type {type.Name}: {ex.Message}");
            }
        }

        SaveAllData();

        Debug.Log("All data reset successfully.");
    }
    /// <summary>
    /// Get path of data 
    /// </summary>
    private static string GetSavePath<T>() where T : IDatable
    {
        return Path.Combine(SaveDirectory, $"{typeof(T).Name}.data");
    }

}


public interface IDatable
{
    //Unique data id
    public string DATA_ID { get; }
    // Invoke when save is initialized, update needed data to save
    void ProcessUpdataData();
    void Dispose();
}

public interface IIgnorable
{
    public bool IsAllSaveIgnored { get; }
    public bool IsAllLoadIgnored { get; }
}
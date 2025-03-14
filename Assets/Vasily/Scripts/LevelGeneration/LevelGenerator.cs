using UnityEngine;
using System.Collections.Generic;

public class LevelGenerator : MonoBehaviour
{
    public Transform[] spawnPoints; // Массив пустых объектов для спавна статичных объектов
    public GameObject[] staticObjects; // Массив статических объектов
    public GameObject[] dynamicObjects; // Массив динамических объектов
    public int dynamicObjectCount = 10; // Количество динамических объектов
    public float minDistanceBetweenDynamicObjects = 2.0f; // Минимальное расстояние между динамическими объектами
    public float removeRadius = 5.0f; // Радиус для удаления динамических объектов вокруг статических объектов
    public float minXY;
    public float maxXY;

    public List<GameObject> spawnedDynamicObjects = new List<GameObject>(); // Список динамических объектов, которые были созданы
    private HashSet<Vector3> occupiedPositions = new HashSet<Vector3>(); // Используем HashSet для улучшения производительности

    void Start()
    {
        LevelGenerate();
    }

    public void LevelGenerate()
    {
        GenerateDynamicObjects();
        GenerateStaticObjects();
        RandomizeDynamicObjectRotations(); // Вызов метода для рандомизации вращения
    }

    void GenerateStaticObjects()
    {
        foreach (var spawnPoint in spawnPoints)
        {
            // Проверяем, если в массиве статических объектов есть элементы
            if (staticObjects.Length > 0)
            {
                // Выбираем случайный статический объект
                GameObject objToSpawn = staticObjects[Random.Range(0, staticObjects.Length)];

                // Создаем статический объект на выбранной точке
                var gameobject = Instantiate(objToSpawn, spawnPoint.position, Quaternion.identity);
                RemoveDynamicObjectsInRange(gameobject.transform.position);
            }
        }
    }

    void GenerateDynamicObjects()
    {
        for (int i = 0; i < dynamicObjectCount; i++)
        {
            GameObject objToSpawn = dynamicObjects[Random.Range(0, dynamicObjects.Length)];
            Vector3 spawnPosition;
            int attempts = 0;

            do
            {
                // Задайте свои границы
                spawnPosition = new Vector3(Random.Range(minXY, maxXY), 0, Random.Range(minXY, maxXY));
                attempts++;
                if (attempts > 10) // Выход из цикла после 10 неудач
                {
                    break; // Позволяем циклу завершить выполнение, если слишком много неудач
                }
            }
            while (!IsValidPosition(spawnPosition));

            // Проверяем, удалось ли найти допустимую позицию
            if (attempts <= 20)
            {
                GameObject dynamicObject = Instantiate(objToSpawn, spawnPosition, Quaternion.identity);

                spawnedDynamicObjects.Add(dynamicObject); // Добавляем в список созданных динамических объектов
                occupiedPositions.Add(spawnPosition); // Добавление в HashSet
            }
        }
    }

    void RemoveDynamicObjectsInRange(Vector3 position)
    {
        // Создаем список для уничтожения объектов
        List<GameObject> objectsToDestroy = new List<GameObject>();

        foreach (GameObject dynamicObject in spawnedDynamicObjects)
        {
            if (Vector3.Distance(dynamicObject.transform.position, position) <= removeRadius)
            {
                objectsToDestroy.Add(dynamicObject); // Добавляем для уничтожения
            }
        }

        // Удаляем объекты из списка
        foreach (GameObject obj in objectsToDestroy)
        {
            spawnedDynamicObjects.Remove(obj);
            Destroy(obj); // Удаляем объект
        }
    }

    bool IsValidPosition(Vector3 position)
    {
        // Используем HashSet для проверки занятых позиций
        foreach (var occupied in occupiedPositions)
        {
            if (Vector3.Distance(position, occupied) < minDistanceBetweenDynamicObjects)
            {
                return false;
            }
        }
        return true;
    }

    void RandomizeDynamicObjectRotations()
    {
        foreach (var dynamicObject in spawnedDynamicObjects)
        {
            float randomYRotation = Random.Range(0f, 300f); // Генерация случайного угла по оси Y
            dynamicObject.transform.rotation = Quaternion.Euler(0f, randomYRotation, 0f); // Установка нового вращения
        }
    }
}

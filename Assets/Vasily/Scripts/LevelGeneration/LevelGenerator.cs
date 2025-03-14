using UnityEngine;
using System.Collections.Generic;

public class LevelGenerator : MonoBehaviour
{
    public Transform[] spawnPoints; // ������ ������ �������� ��� ������ ��������� ��������
    public GameObject[] staticObjects; // ������ ����������� ��������
    public GameObject[] dynamicObjects; // ������ ������������ ��������
    public int dynamicObjectCount = 10; // ���������� ������������ ��������
    public float minDistanceBetweenDynamicObjects = 2.0f; // ����������� ���������� ����� ������������� ���������
    public float removeRadius = 5.0f; // ������ ��� �������� ������������ �������� ������ ����������� ��������
    public float minXY;
    public float maxXY;

    public List<GameObject> spawnedDynamicObjects = new List<GameObject>(); // ������ ������������ ��������, ������� ���� �������
    private HashSet<Vector3> occupiedPositions = new HashSet<Vector3>(); // ���������� HashSet ��� ��������� ������������������

    void Start()
    {
        LevelGenerate();
    }

    public void LevelGenerate()
    {
        GenerateDynamicObjects();
        GenerateStaticObjects();
        RandomizeDynamicObjectRotations(); // ����� ������ ��� ������������ ��������
    }

    void GenerateStaticObjects()
    {
        foreach (var spawnPoint in spawnPoints)
        {
            // ���������, ���� � ������� ����������� �������� ���� ��������
            if (staticObjects.Length > 0)
            {
                // �������� ��������� ����������� ������
                GameObject objToSpawn = staticObjects[Random.Range(0, staticObjects.Length)];

                // ������� ����������� ������ �� ��������� �����
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
                // ������� ���� �������
                spawnPosition = new Vector3(Random.Range(minXY, maxXY), 0, Random.Range(minXY, maxXY));
                attempts++;
                if (attempts > 10) // ����� �� ����� ����� 10 ������
                {
                    break; // ��������� ����� ��������� ����������, ���� ������� ����� ������
                }
            }
            while (!IsValidPosition(spawnPosition));

            // ���������, ������� �� ����� ���������� �������
            if (attempts <= 20)
            {
                GameObject dynamicObject = Instantiate(objToSpawn, spawnPosition, Quaternion.identity);

                spawnedDynamicObjects.Add(dynamicObject); // ��������� � ������ ��������� ������������ ��������
                occupiedPositions.Add(spawnPosition); // ���������� � HashSet
            }
        }
    }

    void RemoveDynamicObjectsInRange(Vector3 position)
    {
        // ������� ������ ��� ����������� ��������
        List<GameObject> objectsToDestroy = new List<GameObject>();

        foreach (GameObject dynamicObject in spawnedDynamicObjects)
        {
            if (Vector3.Distance(dynamicObject.transform.position, position) <= removeRadius)
            {
                objectsToDestroy.Add(dynamicObject); // ��������� ��� �����������
            }
        }

        // ������� ������� �� ������
        foreach (GameObject obj in objectsToDestroy)
        {
            spawnedDynamicObjects.Remove(obj);
            Destroy(obj); // ������� ������
        }
    }

    bool IsValidPosition(Vector3 position)
    {
        // ���������� HashSet ��� �������� ������� �������
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
            float randomYRotation = Random.Range(0f, 300f); // ��������� ���������� ���� �� ��� Y
            dynamicObject.transform.rotation = Quaternion.Euler(0f, randomYRotation, 0f); // ��������� ������ ��������
        }
    }
}

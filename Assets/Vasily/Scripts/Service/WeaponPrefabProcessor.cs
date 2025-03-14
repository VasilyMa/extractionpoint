using UnityEngine;
using UnityEditor;
using System.IO;

public class WeaponPrefabProcessor : EditorWindow
{
    // ���� � ������
    private string addFolderPath = "Assets/Prototype/WeaponPrefab/WeaponViewReady/FORFIX"; // ����� ��� ���������� �����������
    private string removeFolderPath = "Assets/Prototype/WeaponPrefab/WeaponViewReady/FORREMOVE"; // ����� ��� �������� �����������

    // �������, ������� ����� ��������
    public GameObject FirePoint;
    public GameObject Muzzle;
    public GameObject GilzaParticle;

    [MenuItem("Tools/Process Weapon Prefabs")]
    public static void ShowWindow()
    {
        GetWindow<WeaponPrefabProcessor>("Process Weapon Prefabs");
    }

    private void OnGUI()
    {
        GUILayout.Label("�������� ������� ��� ���������� �� �������", EditorStyles.boldLabel);

        FirePoint = (GameObject)EditorGUILayout.ObjectField("FirePoint", FirePoint, typeof(GameObject), false);
        Muzzle = (GameObject)EditorGUILayout.ObjectField("Muzzle", Muzzle, typeof(GameObject), false);
        GilzaParticle = (GameObject)EditorGUILayout.ObjectField("GilzaParticle", GilzaParticle, typeof(GameObject), false);

        GUILayout.Space(10);

        GUILayout.Label("���� � ����� ��� ���������� �����������:", EditorStyles.boldLabel);
        addFolderPath = EditorGUILayout.TextField("Add Path", addFolderPath);

        GUILayout.Label("���� � ����� ��� �������� �����������:", EditorStyles.boldLabel);
        removeFolderPath = EditorGUILayout.TextField("Remove Path", removeFolderPath);

        GUILayout.Space(10);

        if (GUILayout.Button("�������� ���������� � �������"))
        {
            ProcessPrefabs(addFolderPath, true);
        }

        if (GUILayout.Button("������� ���������� � �������"))
        {
            ProcessPrefabs(removeFolderPath, false);
        }
    }

    private void ProcessPrefabs(string folderPath, bool isAdding)
    {
        if (!Directory.Exists(folderPath))
        {
            Debug.LogError($"����� �� �������: {folderPath}");
            return;
        }

        string[] prefabFiles = Directory.GetFiles(folderPath, "*.prefab", SearchOption.AllDirectories);

        foreach (string prefabPath in prefabFiles)
        {
            GameObject prefab = PrefabUtility.LoadPrefabContents(prefabPath);
            if (prefab != null)
            {
                if (isAdding)
                {
                    AddComponentsToPrefab(prefab);
                }
                else
                {
                    RemoveComponentsFromPrefab(prefab);
                }

                PrefabUtility.SaveAsPrefabAsset(prefab, prefabPath);
                PrefabUtility.UnloadPrefabContents(prefab);
            }
        }

        Debug.Log(isAdding ? "���������� ����������� ���������!" : "�������� ����������� ���������!");
    }

    private void AddComponentsToPrefab(GameObject prefab)
    {
        if (prefab.GetComponent<WeaponView>() == null)
        {
            prefab.AddComponent<WeaponView>();
        }

        if (prefab.GetComponent<AudioSource>() == null)
        {
            var wv = prefab.GetComponent<WeaponView>();
            var shootSource = prefab.AddComponent<AudioSource>();
            var reloadSource = prefab.AddComponent<AudioSource>();
            shootSource.playOnAwake = false;
            reloadSource.playOnAwake = false;
            wv.ShootAudioSource = shootSource;
            wv.ReloadAudioSource = reloadSource;
            wv.RecoilStrengh = 5;
            wv.RecoilCameraShakeStrengh = 4;
        }

        AddGameObjectsToPrefab(prefab);
        AddRigidBodyAndColliderToMagObjects(prefab);
    }

    private void RemoveComponentsFromPrefab(GameObject prefab)
    {
        // ������� WeaponView
        WeaponView weaponView = prefab.GetComponent<WeaponView>();
        if (weaponView != null)
        {
            DestroyImmediate(weaponView);
        }

        // ������� AudioSource
        foreach (AudioSource audioSource in prefab.GetComponents<AudioSource>())
        {
            DestroyImmediate(audioSource);
        }

        // ������� FirePoint, Muzzle, GilzaParticle
        RemoveGameObjectsFromPrefab(prefab);

        // ������� Rigidbody � BoxCollider � ��������, ���������� "mag" � ��������
        RemoveRigidBodyAndColliderFromMagObjects(prefab);
    }

    private void AddGameObjectsToPrefab(GameObject prefab)
    {
        if (FirePoint != null)
        {
            GameObject newObj1 = (GameObject)PrefabUtility.InstantiatePrefab(FirePoint);
            newObj1.transform.SetParent(prefab.transform);
            newObj1.transform.localPosition = new Vector3(0, 1, 0);
            prefab.GetComponent<WeaponView>().FirePointTransform = newObj1.transform;
        }

        if (Muzzle != null)
        {
            GameObject newObj2 = (GameObject)PrefabUtility.InstantiatePrefab(Muzzle);
            newObj2.transform.SetParent(prefab.transform);
            newObj2.transform.localPosition = Vector3.zero;
            prefab.GetComponent<WeaponView>().MuzzleParticle = newObj2.GetComponent<ParticleSystem>();
        }

        if (GilzaParticle != null)
        {
            GameObject newObj3 = (GameObject)PrefabUtility.InstantiatePrefab(GilzaParticle);
            newObj3.transform.SetParent(prefab.transform);
            newObj3.transform.localPosition = new Vector3(0, -1, 0);
            prefab.GetComponent<WeaponView>().GilzaParticle = newObj3.GetComponent<ParticleSystem>();
        }
    }

    private void RemoveGameObjectsFromPrefab(GameObject prefab)
    {
        foreach (Transform child in prefab.transform)
        {
            if (child.name.Contains("FirePoint") || child.name.Contains("Bullet_GoldFire") || child.name.Contains("GilzaParticle"))
            {
                DestroyImmediate(child.gameObject);
            }
        }
    }

    private void AddRigidBodyAndColliderToMagObjects(GameObject prefab)
    {
        foreach (Transform child in prefab.GetComponentsInChildren<Transform>(true))
        {
            if (child.name.ToLower().Contains("mag"))
            {
                if (child.gameObject.GetComponent<Rigidbody>() == null)
                {
                    Rigidbody rb = child.gameObject.AddComponent<Rigidbody>();
                    rb.isKinematic = true;
                }

                if (child.gameObject.GetComponent<BoxCollider>() == null)
                {
                    BoxCollider boxCollider = child.gameObject.AddComponent<BoxCollider>();
                    boxCollider.excludeLayers = LayerMask.GetMask("Enemy", "Player");

                }
            }
        }
    }

    private void RemoveRigidBodyAndColliderFromMagObjects(GameObject prefab)
    {
        foreach (Transform child in prefab.GetComponentsInChildren<Transform>(true))
        {
            if (child.name.ToLower().Contains("mag"))
            {
                Rigidbody rb = child.gameObject.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    DestroyImmediate(rb);
                }

                BoxCollider boxCollider = child.gameObject.GetComponent<BoxCollider>();
                if (boxCollider != null)
                {
                    DestroyImmediate(boxCollider);
                }
            }
        }
    }
}

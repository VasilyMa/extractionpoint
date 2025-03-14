using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
public class RemoveColliders : EditorWindow
{
    [MenuItem("Tools/Remove All Colliders")]
    public static void RemoveAllColliders()
    {
        // �������� ��� ������� � �����
        GameObject[] allGameObjects = GameObject.FindObjectsOfType<GameObject>(true); // true ��� ������ ���� ���������� ��������

        int removedCount = 0;

        // �������� �� ���� �������� � ������� ���������� Collider
        foreach (var gameObject in allGameObjects)
        {
            // �������� ��� ���������� Collider �� �������
            Collider[] colliders = gameObject.GetComponents<Collider>();

            // ���� ���� ����������, ������� ��
            foreach (var collider in colliders)
            {
                Undo.DestroyObjectImmediate(collider); // ������� ��������� � ������������ ������
                removedCount++;
            }
        }

        // ������� ��������� � �������
        Debug.Log($"������� �����������: {removedCount}");
    }
}
#endif

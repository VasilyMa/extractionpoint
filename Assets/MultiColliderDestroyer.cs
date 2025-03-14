using UnityEngine;
using System.Collections.Generic;

public class MultiColliderDestroyer : MonoBehaviour
{
    // ������ ����������, ������� ����� ��������� ��������
    public List<BoxCollider> colliders;


    private void Awake()
    {
        // ������� ��� BoxCollider'�, ������������ � ����� GameObject.  
        //  �����: ���������, ��� �� �������� �� � ��������� Unity *�����* �������� �����.
        colliders = new List<BoxCollider>(GetComponents<BoxCollider>());

        // ��������, ���� �� ���� �� ���� ��������
        if (colliders.Count == 0)
        {
            Debug.LogError("No BoxColliders found on this GameObject! Please add at least one in the Unity editor.");
            enabled = false; // ��������� ������, ���� ���������� ���.
            return;
        }

        // �������� ���������� ���������� (��� �������������� ������������)
        if (colliders.Count > 10)
        {
            Debug.LogWarning("More than 10 colliders detected.  This might affect performance.");
        }

        foreach (var collider in colliders)
        {
            if (!collider.isTrigger)
            {
                Debug.LogError("Collider must be a trigger!  Set 'Is Trigger' to true in the Inspector.");
                enabled = false;
                return;
            }
        }

    }


    private void OnTriggerEnter(Collider other)
    {
        // ���������, ��� ������ ��������� �� ���� "Default"
        if (other.gameObject.layer == LayerMask.NameToLayer("Deletable"))
        {
            Destroy(other.gameObject);
        }
    }
}

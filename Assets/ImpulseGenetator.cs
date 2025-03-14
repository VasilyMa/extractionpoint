using Cinemachine;
using UnityEngine;

public class ImpulseGenerator : MonoBehaviour
{
    public CinemachineImpulseSource cinemachineImpulseSource;
    public float impulseInterval = 1f; // �������� � �������� ����� ����������
    public float impulseForce = 1f; // ���� ��������

    void Start()
    {
        cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();

        // ��������� ����� GenerateImpulse ������ �������
        InvokeRepeating(nameof(GenerateImpulse), 0f, impulseInterval);
    }

    void GenerateImpulse()
    {
        if (cinemachineImpulseSource != null)
        {
            cinemachineImpulseSource.GenerateImpulseWithForce(impulseForce);
        }
    }
}

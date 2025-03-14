using Cinemachine;
using UnityEngine;

public class ImpulseGenerator : MonoBehaviour
{
    public CinemachineImpulseSource cinemachineImpulseSource;
    public float impulseInterval = 1f; // Интервал в секундах между импульсами
    public float impulseForce = 1f; // Сила импульса

    void Start()
    {
        cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();

        // Запускаем вызов GenerateImpulse каждую секунду
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

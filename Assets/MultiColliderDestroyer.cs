using UnityEngine;
using System.Collections.Generic;

public class MultiColliderDestroyer : MonoBehaviour
{
    // Список колайдеров, которые будут проверять коллизии
    public List<BoxCollider> colliders;


    private void Awake()
    {
        // Находим все BoxCollider'ы, прикреплённые к этому GameObject.  
        //  Важно: убедитесь, что вы добавили их в редакторе Unity *перед* запуском сцены.
        colliders = new List<BoxCollider>(GetComponents<BoxCollider>());

        // Проверка, есть ли хотя бы один колайдер
        if (colliders.Count == 0)
        {
            Debug.LogError("No BoxColliders found on this GameObject! Please add at least one in the Unity editor.");
            enabled = false; // Отключаем скрипт, если колайдеров нет.
            return;
        }

        // Проверка количества колайдеров (для дополнительной безопасности)
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
        // Проверяем, что объект находится на слое "Default"
        if (other.gameObject.layer == LayerMask.NameToLayer("Deletable"))
        {
            Destroy(other.gameObject);
        }
    }
}

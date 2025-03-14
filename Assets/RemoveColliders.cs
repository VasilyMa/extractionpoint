using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
public class RemoveColliders : EditorWindow
{
    [MenuItem("Tools/Remove All Colliders")]
    public static void RemoveAllColliders()
    {
        // Получаем все объекты в сцене
        GameObject[] allGameObjects = GameObject.FindObjectsOfType<GameObject>(true); // true для поиска даже неактивных объектов

        int removedCount = 0;

        // Проходим по всем объектам и удаляем компоненты Collider
        foreach (var gameObject in allGameObjects)
        {
            // Получаем все компоненты Collider на объекте
            Collider[] colliders = gameObject.GetComponents<Collider>();

            // Если есть коллайдеры, удаляем их
            foreach (var collider in colliders)
            {
                Undo.DestroyObjectImmediate(collider); // Удаляем коллайдер с возможностью отмены
                removedCount++;
            }
        }

        // Выводим сообщение в консоль
        Debug.Log($"Удалено коллайдеров: {removedCount}");
    }
}
#endif

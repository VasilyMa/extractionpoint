using UnityEngine;
using UnityEngine.Animations.Rigging; // Не забудьте подключить этот namespace
using System.Collections;

public class GunController : MonoBehaviour
{
    public Animator animator; // Ссылка на Animator
    public Rig rig; // Компонент Rig
    public RigBuilder rigBuilder; // Компонент Rig
    private float rigWeightTarget = 1f; // Целевое значение веса
    public Transform target;
    public float RotationSpeed;
    private void Update()
    {
      // KeepCharacterAligned();
    }
/*
    private IEnumerator Reload()
    {
        // Запускаем анимацию перезарядки
        animator.SetTrigger("ReloadTrigger");

        // Уменьшаем вес режима (rig.weight) до 0 за 0.1 секунды
        float time = 0;
        float duration = 0.2f;

        while (time < duration)
        {
            rig.weight = Mathf.Lerp(1f, 0f, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        rig.weight = 0f; // Убедимся, что вес действительно 0

        // Ждем завершения анимации
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        // Возвращаем вес обратно к 1 за 0.1 секунды
        time = 0;
        while (time < duration)
        {
            rig.weight = Mathf.Lerp(0f, 1f, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        rig.weight = 1f; // Убедимся, что вес действительно 1
    }*/
    private void KeepCharacterAligned()
    {
        if (target != null)
        {
            // Получаем направление к таргету с фиксированной высотой Y = 1
            Vector3 directionToTarget = new Vector3(target.position.x, 1f, target.position.z) - new Vector3(transform.position.x, 1f, transform.position.z);

            // Проверяем, есть ли какое-то направление
            if (directionToTarget != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * RotationSpeed); // Мягкий поворот
            }
        }
    }
}

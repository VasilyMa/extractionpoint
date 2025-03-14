using System.Collections;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class CharacterRotationController : MonoBehaviour
{
    public Transform spine; // Укажите spine_1
    public Transform characterBody; // Укажите корень персонажа
    public Transform target; // Укажите цель, к которой будет поворачиваться персонаж
    public float rotationThreshold = 90f; // Порог поворота в градусах
    public float rotationSpeed = 5f; // Скорость поворота

    public Rig rig;
    public Animator anim;

    public float weightChangeDuration = 0.5f; // Длительность изменения веса
    public float animationDelay = 0.2f; // Задержка между началом анимации и уменьшением веса
    public float endDelay = 0.2f; // Задержка между завершением анимации и увеличением веса

    private void Start()
    {
        anim = GetComponent<Animator>();

        // Проверяем наличие компонентов
        if (rig == null)
        {
            Debug.LogError("Rig component not found on " + gameObject.name);
        }

        if (anim == null)
        {
            Debug.LogError("Animator component not found on " + gameObject.name);
        }
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            StartCoroutine(ReloadCoroutine());
        }

        // Ваша логика поворота будет здесь, если она необходима.
    }

    private IEnumerator ReloadCoroutine()
    {
        // Начинаем анимацию перезарядки
        anim.CrossFade("Reload", 0.15f);

        // Ждем animationDelay перед уменьшением веса
        yield return new WaitForSeconds(animationDelay);

        // Запускаем первую корутину для уменьшения веса
        yield return StartCoroutine(ReduceRigWeightCoroutine());

        // Ждем окончания анимации
        while (anim.GetCurrentAnimatorStateInfo(0).IsName("Reload"))
        {
            yield return null; // Ждем следующий кадр
        }

        // Ждем endDelay перед увеличением веса
        yield return new WaitForSeconds(endDelay);

        // Запускаем вторую корутину для увеличения веса
        yield return StartCoroutine(IncreaseRigWeightCoroutine());
    }

    private IEnumerator ReduceRigWeightCoroutine()
    {
        float elapsedTime = 0f;

        // Уменьшаем вес за weightChangeDuration
        while (elapsedTime < weightChangeDuration)
        {
            rig.weight = Mathf.Lerp(1, 0, elapsedTime / weightChangeDuration);
            elapsedTime += Time.deltaTime;
            yield return null; // Ждем следующий кадр
        }

        // Устанавливаем вес рига на 0
        rig.weight = 0;
    }

    private IEnumerator IncreaseRigWeightCoroutine()
    {
        float elapsedTime = 0f;

        // Увеличиваем вес за weightChangeDuration
        while (elapsedTime < weightChangeDuration)
        {
            rig.weight = Mathf.Lerp(0, 1, elapsedTime / weightChangeDuration);
            elapsedTime += Time.deltaTime;
            yield return null; // Ждем следующий кадр
        }

        // Убеждаемся, что вес рига установлен на 1 после завершения
        rig.weight = 1;
    }
}

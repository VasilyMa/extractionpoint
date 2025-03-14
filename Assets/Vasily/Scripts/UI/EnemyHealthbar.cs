using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthbar : MonoBehaviour
{

    [SerializeField] private Renderer _healthBarRenderer;
    private MaterialPropertyBlock materialPropertyBlock; // Используется для изменения параметров материала
    private static readonly int ProgressPropertyID = Shader.PropertyToID("_Progress");

    private PhotonUnit _refUnit;
    private Transform _cameraTransform;

    float duration = 0.2f;
    float targetHealth;
    float maxHealth;

    Coroutine healthBarCoroutine; // Ссылка на корутину

    public void Init(PhotonUnit sourceUnit)
    {
        // Инициализация MaterialPropertyBlock
        materialPropertyBlock = new MaterialPropertyBlock();
        _refUnit = sourceUnit;
        _refUnit.GetHealth().OnValueUpdate += OnHealthChange;
        _cameraTransform = Camera.main.transform;

        if (healthBarCoroutine != null)
        {
            StopCoroutine(healthBarCoroutine);
            healthBarCoroutine = null;
        }
    }

    public void Invoke()
    {
        materialPropertyBlock.SetFloat(ProgressPropertyID, 1);
        _healthBarRenderer.SetPropertyBlock(materialPropertyBlock);
        _healthBarRenderer.gameObject.SetActive(false);
    }

    public void Run()
    {
        if (!gameObject.activeSelf) return;

        transform.LookAt(transform.position + _cameraTransform.forward, Vector3.up);
    }

    public void OnHealthChange(float currentValue, float maxValue)
    {
        // Если здоровье на максимуме, отключаем отображение
        if (currentValue >= maxValue)
        {
            _healthBarRenderer.gameObject.SetActive(false);
            return;
        }

        _healthBarRenderer.gameObject.SetActive(true);

        // Устанавливаем целевое и максимальное значение здоровья
        targetHealth = currentValue;
        maxHealth = maxValue;

        // Если корутина уже работает, не запускаем новую
        if (healthBarCoroutine == null)
        {
            healthBarCoroutine = StartCoroutine(UpdateHealthBarSmoothly());
        }
    }

    private IEnumerator UpdateHealthBarSmoothly()
    {
        float time = 0f; // Счётчик времени

        // Получаем начальное значение прогресса
        _healthBarRenderer.GetPropertyBlock(materialPropertyBlock);
        float initialHealth = materialPropertyBlock.GetFloat(ProgressPropertyID);

        while (true)
        {
            // Увеличиваем время
            time += Time.deltaTime;

            // Линейно интерполируем между текущим и целевым значением
            float progress = Mathf.Lerp(initialHealth, targetHealth / maxHealth, time / duration);
            materialPropertyBlock.SetFloat(ProgressPropertyID, progress);
            _healthBarRenderer.SetPropertyBlock(materialPropertyBlock);

            // Если время вышло, завершаем цикл
            if (time >= duration)
            {
                materialPropertyBlock.SetFloat(ProgressPropertyID, targetHealth / maxHealth);
                _healthBarRenderer.SetPropertyBlock(materialPropertyBlock);
                healthBarCoroutine = null; // Сбрасываем ссылку на корутину
                break;
            }

            yield return null; // Ждём следующий кадр
        }

        // Если здоровье достигло максимума, скрываем объект
        if (targetHealth >= maxHealth || targetHealth <= 0)
        {
            _healthBarRenderer.gameObject.SetActive(false);
        }
    }
    public void Disable()
    {
        targetHealth = 1f;
        maxHealth = 1f;

        if (healthBarCoroutine != null)
        {
            StopCoroutine(healthBarCoroutine);
            healthBarCoroutine = null;
        }
    }

    public void Dispose()
    {
        _refUnit.GetHealth().OnValueUpdate -= OnHealthChange;

        Disable();
    }
}

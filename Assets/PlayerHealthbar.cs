using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealthbar : MonoBehaviour
{
    [SerializeField] private Image _progress;

    [SerializeField] private float maxAnimationDuration = 0.25f; // Максимальная длительность анимации

    private float _targetHealth;
    private float _currentHealth;
    private float _maxHealth;
    private Coroutine healthBarCoroutine; // Ссылка на корутину

    PhotonPlayer _player;

    public PlayerHealthbar Init(PhotonPlayer player)
    {
        _player = player;

        var health = player.GetHealth();

        health.OnValueUpdate += OnUpdateHealthbar;

        _currentHealth = health.Current;
        _maxHealth = health.MaxValue;

        _progress.fillAmount = _currentHealth / _maxHealth;

        gameObject.SetActive(false);

        return this;
    }

    private void OnUpdateHealthbar(float currentValue, float maxValue)
    {
        if (currentValue >= maxValue)
        {
            _currentHealth = currentValue;
            _maxHealth = maxValue;
            _progress.fillAmount = 1;
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(true); // Подсвечиваем объект

        _targetHealth = currentValue; // Обновляем целевое здоровье
        _maxHealth = maxValue; // Обновляем максимальное здоровье

        // Вычисляем дельту изменения здоровья
        float healthDelta = Mathf.Abs(_currentHealth - _targetHealth);

        // Длительность анимации с учётом ограничения
        float duration = Mathf.Clamp(healthDelta / _maxHealth * maxAnimationDuration, 0.1f, maxAnimationDuration);

        // Если корутина уже запущена, останавливаем её
        if (healthBarCoroutine != null)
        {
            StopCoroutine(healthBarCoroutine);
        }

        // Запускаем корутину с актуальными данными
        healthBarCoroutine = StartCoroutine(UpdateHealthBarSmoothly(duration));
    }

    private IEnumerator UpdateHealthBarSmoothly(float duration)
    {
        float time = 0; // Счётчик времени
        float initialHealth = _currentHealth; // Начальное значение здоровья

        while (time < duration)
        {
            time += Time.deltaTime;

            // Линейная интерполяция между текущим и целевым значением
            _currentHealth = Mathf.Lerp(initialHealth, _targetHealth, time / duration);

            // Обновляем значение заполнения прогресс-бара
            _progress.fillAmount = _currentHealth / _maxHealth;

            yield return null;
        }

        // Убедимся, что прогресс-бар точно соответствует целевому здоровью
        _currentHealth = _targetHealth;
        _progress.fillAmount = _currentHealth / _maxHealth;

        healthBarCoroutine = null; // Сбрасываем ссылку на корутину

        // Скрываем объект, если здоровье полностью восстановлено
        if (_targetHealth >= _maxHealth)
        {
            gameObject.SetActive(false);
        }
    }
    public void Open()
    {

    }

    public void Close()
    {
        if (healthBarCoroutine != null) StopCoroutine(healthBarCoroutine);
        healthBarCoroutine = null;
    }

    public void Dispose()
    {
        _player.GetHealth().OnValueUpdate -= OnUpdateHealthbar;

        if (healthBarCoroutine != null) StopCoroutine(healthBarCoroutine);
        healthBarCoroutine = null;
    }
}

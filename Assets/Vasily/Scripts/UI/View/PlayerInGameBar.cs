using System.Collections;

using UnityEngine;
using UnityEngine.UI;

public class PlayerInGameBar : MonoBehaviour
{
    public bool IsUsed;

    [SerializeField] Text _titleName;
    [SerializeField] Image _iconRank;

    private PhotonPlayer _player;
    private Camera _camera;


    [SerializeField] private Image _progress;

    [SerializeField] private float maxAnimationDuration = 0.25f; // Максимальная длительность анимации

    private float _targetHealth;
    private float _currentHealth;
    private float _maxHealth;
    private Coroutine healthBarCoroutine; // Ссылка на корутину

    public void Init(PhotonPlayer player)
    {
        _player = player;
        _camera = Camera.main;

        _player.GetHealth().OnValueUpdate += OnUpdateHealthbar;
        _progress.fillAmount = 1f;
        _titleName.text = player.PlayerNickName;

        if (string.IsNullOrEmpty(player.RankID))  _iconRank.enabled = false;

        IsUsed = true;

        gameObject.SetActive(true);
    }

    private void Update()
    {
        transform.position = _camera.WorldToScreenPoint(_player.transform.position + new Vector3(0, 3.5f, 0));
    }

    private void OnUpdateHealthbar(float currentValue, float maxValue)
    {
        if (currentValue >= maxValue)
        {
            _currentHealth = currentValue;
            _maxHealth = maxValue;
            _progress.fillAmount = 1;
            _progress.transform.parent.gameObject.SetActive(false);
            return;
        }

        _progress.transform.parent.gameObject.SetActive(true); // Подсвечиваем объект

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
            _progress.transform.parent.gameObject.SetActive(false);
        }
    }
}

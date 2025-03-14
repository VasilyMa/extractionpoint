using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealthbar : MonoBehaviour
{
    [SerializeField] private Image _progress;

    [SerializeField] private float maxAnimationDuration = 0.25f; // ������������ ������������ ��������

    private float _targetHealth;
    private float _currentHealth;
    private float _maxHealth;
    private Coroutine healthBarCoroutine; // ������ �� ��������

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

        gameObject.SetActive(true); // ������������ ������

        _targetHealth = currentValue; // ��������� ������� ��������
        _maxHealth = maxValue; // ��������� ������������ ��������

        // ��������� ������ ��������� ��������
        float healthDelta = Mathf.Abs(_currentHealth - _targetHealth);

        // ������������ �������� � ������ �����������
        float duration = Mathf.Clamp(healthDelta / _maxHealth * maxAnimationDuration, 0.1f, maxAnimationDuration);

        // ���� �������� ��� ��������, ������������� �
        if (healthBarCoroutine != null)
        {
            StopCoroutine(healthBarCoroutine);
        }

        // ��������� �������� � ����������� �������
        healthBarCoroutine = StartCoroutine(UpdateHealthBarSmoothly(duration));
    }

    private IEnumerator UpdateHealthBarSmoothly(float duration)
    {
        float time = 0; // ������� �������
        float initialHealth = _currentHealth; // ��������� �������� ��������

        while (time < duration)
        {
            time += Time.deltaTime;

            // �������� ������������ ����� ������� � ������� ���������
            _currentHealth = Mathf.Lerp(initialHealth, _targetHealth, time / duration);

            // ��������� �������� ���������� ��������-����
            _progress.fillAmount = _currentHealth / _maxHealth;

            yield return null;
        }

        // ��������, ��� ��������-��� ����� ������������� �������� ��������
        _currentHealth = _targetHealth;
        _progress.fillAmount = _currentHealth / _maxHealth;

        healthBarCoroutine = null; // ���������� ������ �� ��������

        // �������� ������, ���� �������� ��������� �������������
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

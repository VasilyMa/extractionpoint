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

    [SerializeField] private float maxAnimationDuration = 0.25f; // ������������ ������������ ��������

    private float _targetHealth;
    private float _currentHealth;
    private float _maxHealth;
    private Coroutine healthBarCoroutine; // ������ �� ��������

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

        _progress.transform.parent.gameObject.SetActive(true); // ������������ ������

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
            _progress.transform.parent.gameObject.SetActive(false);
        }
    }
}

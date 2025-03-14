using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthbar : MonoBehaviour
{

    [SerializeField] private Renderer _healthBarRenderer;
    private MaterialPropertyBlock materialPropertyBlock; // ������������ ��� ��������� ���������� ���������
    private static readonly int ProgressPropertyID = Shader.PropertyToID("_Progress");

    private PhotonUnit _refUnit;
    private Transform _cameraTransform;

    float duration = 0.2f;
    float targetHealth;
    float maxHealth;

    Coroutine healthBarCoroutine; // ������ �� ��������

    public void Init(PhotonUnit sourceUnit)
    {
        // ������������� MaterialPropertyBlock
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
        // ���� �������� �� ���������, ��������� �����������
        if (currentValue >= maxValue)
        {
            _healthBarRenderer.gameObject.SetActive(false);
            return;
        }

        _healthBarRenderer.gameObject.SetActive(true);

        // ������������� ������� � ������������ �������� ��������
        targetHealth = currentValue;
        maxHealth = maxValue;

        // ���� �������� ��� ��������, �� ��������� �����
        if (healthBarCoroutine == null)
        {
            healthBarCoroutine = StartCoroutine(UpdateHealthBarSmoothly());
        }
    }

    private IEnumerator UpdateHealthBarSmoothly()
    {
        float time = 0f; // ������� �������

        // �������� ��������� �������� ���������
        _healthBarRenderer.GetPropertyBlock(materialPropertyBlock);
        float initialHealth = materialPropertyBlock.GetFloat(ProgressPropertyID);

        while (true)
        {
            // ����������� �����
            time += Time.deltaTime;

            // ������� ������������� ����� ������� � ������� ���������
            float progress = Mathf.Lerp(initialHealth, targetHealth / maxHealth, time / duration);
            materialPropertyBlock.SetFloat(ProgressPropertyID, progress);
            _healthBarRenderer.SetPropertyBlock(materialPropertyBlock);

            // ���� ����� �����, ��������� ����
            if (time >= duration)
            {
                materialPropertyBlock.SetFloat(ProgressPropertyID, targetHealth / maxHealth);
                _healthBarRenderer.SetPropertyBlock(materialPropertyBlock);
                healthBarCoroutine = null; // ���������� ������ �� ��������
                break;
            }

            yield return null; // ��� ��������� ����
        }

        // ���� �������� �������� ���������, �������� ������
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

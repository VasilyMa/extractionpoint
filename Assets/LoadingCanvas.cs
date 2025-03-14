using System.Collections;

using UnityEngine;
using UnityEngine.UI;

public class LoadingCanvas : SourceCanvas
{
    [SerializeField] Image _slider;
    [SerializeField] Text _title;

    public void LoadingUpdate(string title, float value)
    {
        _title.text = title;
        StartCoroutine(SmoothLoadingUpdate(value, 0.1f));
    }
    IEnumerator SmoothLoadingUpdate(float targetValue, float duration)
    {
        float startValue = _slider.fillAmount;
        float elapsedTime = 0f;

        // Постепенно обновляем значение fillAmount
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float newValue = Mathf.Lerp(startValue, targetValue, elapsedTime / duration);
            _slider.fillAmount = newValue;
            yield return null; // Ждём следующий кадр
        }

        // Устанавливаем точное конечное значение, чтобы избежать погрешностей
        _slider.fillAmount = targetValue;
    }

    public void ResetLoad()
    {
        _slider.fillAmount = 0f;
    }
}

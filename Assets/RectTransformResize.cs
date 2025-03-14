using DG.Tweening;

using UnityEngine;

public class RectTransformResize : MonoBehaviour
{
    public RectTransform targetRect; // Ссылка на объект с RectTransform
    public float duration = 0.5f; // Длительность анимации

    private Vector2 _originalSize; // Исходный размер

    private void Start()
    {
        _originalSize = targetRect.sizeDelta; // Сохраняем оригинальный размер
    }

    public void ResizeWidth(float newWidth)
    {
        DOTween.To(() => targetRect.sizeDelta, x => targetRect.sizeDelta = x, new Vector2(newWidth, targetRect.sizeDelta.y), duration);
    }

    public void ResizeHeight(float newHeight)
    {
        DOTween.To(() => targetRect.sizeDelta, x => targetRect.sizeDelta = x, new Vector2(targetRect.sizeDelta.x, newHeight), duration);
    }

    public void ResetSize()
    {
        DOTween.To(() => targetRect.sizeDelta, x => targetRect.sizeDelta = x, _originalSize, duration);
    }
}

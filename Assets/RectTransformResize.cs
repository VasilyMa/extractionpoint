using DG.Tweening;

using UnityEngine;

public class RectTransformResize : MonoBehaviour
{
    public RectTransform targetRect; // ������ �� ������ � RectTransform
    public float duration = 0.5f; // ������������ ��������

    private Vector2 _originalSize; // �������� ������

    private void Start()
    {
        _originalSize = targetRect.sizeDelta; // ��������� ������������ ������
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

using DG.Tweening;

using System;

using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public abstract class SourcePanel : MonoBehaviour
{
    public Ease EaseOpen = Ease.InBack;
    public Ease EaseClose = Ease.OutBack;

    public float DurationAnimate = 0.5f;

    protected CanvasGroup _canvasGroup;
    protected RectTransform _rectTransform;
    protected SourceCanvas _sourceCanvas;

    protected Sequence _sequenceHide;
    protected Sequence _sequenceShow;

    public bool isOpenOnInit;
    public bool isAlwaysOpen;
    protected bool isOpen;

    public virtual void Init(SourceCanvas canvasParent)
    {
        _sourceCanvas = canvasParent;
        _rectTransform = GetComponent<RectTransform>();
        _canvasGroup = GetComponent<CanvasGroup>();
        
        isOpen = false;

        if(!isOpenOnInit) gameObject.SetActive(false);
    }
    public virtual void OnOpen(params Action[] onComplete)
    {
        gameObject.SetActive(true);

        if (isOpen) return;

        Show(onComplete);
    }

    protected virtual void Show(params Action[] onComplete)
    {
        gameObject.SetActive(true);
        _canvasGroup.alpha = 0f;
        _rectTransform.localScale = Vector3.one * 1.1f;

        // Анимация появления
        _sequenceShow = DOTween.Sequence();
        _sequenceShow.Append(_canvasGroup.DOFade(1f, DurationAnimate));
        _sequenceShow.Join(_rectTransform.DOScale(1f, DurationAnimate).SetEase(EaseOpen)).OnComplete(() =>
        {
            foreach (var action in onComplete)
            {
                action?.Invoke(); // Вызываем колбэк
            }

            isOpen = true;
        });
    }

    public virtual void OnCLose(params Action[] onComplete)
    {
        if (isOpen) Hide(onComplete);
    }
    protected virtual void Hide(params Action[] onComplete)
    {
        // Анимация исчезновения
        _sequenceHide = DOTween.Sequence();
        _sequenceHide.Append(_canvasGroup.DOFade(0f, DurationAnimate));
        _sequenceHide.Join(_rectTransform.DOScale(1.1f, DurationAnimate).SetEase(EaseClose))
            .OnComplete(() =>
            {
                if (onComplete.Length > 0)
                {
                    foreach (var action in onComplete)
                    {
                        action?.Invoke();
                    }
                }

                // Полностью отключаем объект после анимации
                gameObject.SetActive(false);
                isOpen = false;
            });
    }

    public virtual void OnDipose()
    {
        _sequenceHide?.Kill();
        _sequenceShow?.Kill();
    }
}

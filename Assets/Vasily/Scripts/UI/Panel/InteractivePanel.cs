using System;

using UnityEngine;

public class InteractivePanel : SourcePanel
{
    [SerializeField] CrateViewObject _viewObject;

    Coroutine _timerCoroutine;

    public override void Init(SourceCanvas canvasParent)
    {
        if (_viewObject == null) _viewObject = GetComponentInChildren<CrateViewObject>(); 
        _viewObject.gameObject.SetActive(false);

        base.Init(canvasParent);
    }

    public override void OnDipose()
    {
        if (_timerCoroutine != null)
        {
            StopCoroutine(_timerCoroutine);
            _timerCoroutine = null;
        }

        base.OnDipose();
    }

    public void InvokeViewTimer(InteractiveViewData viewData)
    {
        _viewObject.gameObject.SetActive(true);

        if(_timerCoroutine == null) _timerCoroutine = State.Instance.RunCoroutine(_viewObject.Timer(viewData), onTimerEnd);
        else
        {
            StopCoroutine(_timerCoroutine);
            _timerCoroutine = null;
            _timerCoroutine = State.Instance.RunCoroutine(_viewObject.Timer(viewData), onTimerEnd);
        }
    }

    public void InvokeViewTimer(InteractiveViewData viewData, Action callback = null)
    {
        _viewObject.gameObject.SetActive(true);

        if(_timerCoroutine == null) _timerCoroutine = State.Instance.RunCoroutine(_viewObject.Timer(viewData), onTimerEnd, callback);
        else
        {
            StopCoroutine(_timerCoroutine);
            _timerCoroutine = null;
            _timerCoroutine = State.Instance.RunCoroutine(_viewObject.Timer(viewData), onTimerEnd, callback);
        }
    }

    void onTimerEnd()
    {
        _timerCoroutine = null;
        _viewObject.gameObject.SetActive(false);
        //TODO end timer
    }
}

using System;

using UnityEngine;
using UnityEngine.UI;

public class DifficultSlotView : MonoBehaviour
{
    [SerializeField] Difficult _difficult;
    Button _btnStart;
    Action _onClick;

    public void Init(Action onClick)
    {
        _onClick = onClick;
        _btnStart = GetComponentInChildren<Button>();
        _btnStart.onClick.AddListener(InvokeStart);
    }

    void InvokeStart()
    {
        PlayModeEntity.Instance.Difficult = _difficult;
        _onClick.Invoke();
    }

    public void Dispose()
    {
        _btnStart.onClick.RemoveAllListeners();
    }
}

public enum Difficult { easy, medium, hard }
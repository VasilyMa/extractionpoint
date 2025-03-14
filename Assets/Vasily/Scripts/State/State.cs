using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public abstract class State : MonoBehaviour
{

    protected PoolModule _pool = new PoolModule();
    protected static State _instance;
    public static State Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<State>();
            }
            return _instance;
        }
    }

    protected List<SourceCanvas> _canvases;

    private float _tick;
    protected virtual void Awake()
    {
        _instance = this;
    }

    protected virtual void Start()
    {
        InitCanvas();
    }

    protected virtual void InitCanvas()
    {
        _canvases = new List<SourceCanvas>();

        var canveses = FindObjectsOfType<SourceCanvas>();

        for (int i = 0; i < canveses.Length; i++)
        {
            _canvases.Add(canveses[i]);
        }

        _canvases.ForEach(canvas => canvas.Init());
    }

    public virtual T InvokeCanvas<T>() where T : SourceCanvas
    {
        SourceCanvas returnedCanvas = null;

        foreach (var canvas in _canvases)
        {
            if (canvas is T returned)
            {
                returnedCanvas = returned;
            }
            else
            {
                canvas.CloseCanvas();
            }
        }

        returnedCanvas.InvokeCanvas();

        return returnedCanvas as T;
    }
    
    public virtual T InvokeCanvas<T, M>() where T : SourceCanvas where M : SourceCanvas
    {
        SourceCanvas returnedCanvas = null;

        foreach (var canvas in _canvases)
        {
            if (canvas is T returned)
            {
                returnedCanvas = returned;
            }
            else if(canvas is M)
            {
                continue;
            }
            else
            {
                canvas.CloseCanvas();
            }
        }

        returnedCanvas.InvokeCanvas();

        return returnedCanvas as T;
    }

    public virtual T GetCanvas<T>() where T : SourceCanvas
    {
        foreach (var canvas in _canvases)
        {
            if (canvas is T returned)
            {
                return returned as T;
            }
        }

        return null;
    }

    public virtual bool IsInitedCanvses()
    {
        return _canvases != null && !_canvases.Any(canvas => !canvas.isInited);  // Проверяем, инициализированы ли все канвасы
    }
    protected virtual void DisposeCanvas()
    {
        _canvases.ForEach(canvas => canvas.Dispose());
    }

    public Coroutine RunCoroutine(IEnumerator coroutine, Action callback = null)
    {
        if (Instance != null)
        {
            return StartCoroutine(Instance.CoroutineWrapper(coroutine, callback));
        }
        else
        {
            Debug.LogError("CoroutineHelper Instance is missing in the scene!");
            return null;
        }
    }
    
    public Coroutine RunCoroutine(IEnumerator coroutine, params Action[] callback)
    {
        if (Instance != null)
        {
            return StartCoroutine(Instance.CoroutineWrapper(coroutine, callback));
        }
        else
        {
            Debug.LogError("CoroutineHelper Instance is missing in the scene!");
            return null;
        }
    }

    public IEnumerator CoroutineWrapper(IEnumerator coroutine, Action callback = null)
    {
        yield return StartCoroutine(coroutine); // Ждем завершения переданной корутины

        Debug.Log("Callback coroutine");

        callback?.Invoke(); // Вызываем колбэк, если он есть
    }
    
    public IEnumerator CoroutineWrapper(IEnumerator coroutine, params Action[] callback)
    {
        yield return StartCoroutine(coroutine); // Ждем завершения переданной корутины

        Debug.Log("Callback coroutine");

        for (int i = 0; i < callback.Length; i++)
        {
            callback[i]?.Invoke();
        }
    }

    public abstract void Dispose();
}

using System;

using UnityEngine;

public class LobbyState : State
{
    public static new LobbyState Instance
    {
        get
        {
            return (LobbyState)State.Instance;
        }
    }

    public Action OnSwipeLeft;
    public Action OnSwipeRight;
    public Action OnSwipeUp;
    public Action OnSwipeDown;

    private Vector2 _startTouchPosition;
    private Vector2 _endTouchPosition;
    private float _swipeThreshold = 50f; 

    protected override void Awake()
    {
        base.Awake();

        OnSwipeLeft += () => InvokeCanvas<InventoryCanvas>().OpenPanel<InventoryPanel>(); 
        OnSwipeRight += () => InvokeCanvas<MainMenuCanvas>().OpenPanel<MenuPanel>();
    }

    protected override void Start()
    {
        base.Start();

        InvokeCanvas<MainMenuCanvas>().OpenPanel<MenuPanel>();
    }

    public override void Dispose()
    {
        _canvases.ForEach(x => x.Dispose());    
        _pool.Dispose();
    }

    private void Update()
    {
        DetectSwipe();
    } 

    private void DetectSwipe()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _startTouchPosition = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0))
        {
            _endTouchPosition = Input.mousePosition;
            ProcessSwipe();
        }
    }

    private void ProcessSwipe()
    {
        Vector2 swipeDelta = _endTouchPosition - _startTouchPosition;

        if (swipeDelta.magnitude < _swipeThreshold) return; // Игнорируем слишком короткие свайпы

        if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y))
        {
            if (swipeDelta.x > 0) OnSwipeRight?.Invoke();
            else OnSwipeLeft?.Invoke();
        }
        else
        {
            if (swipeDelta.y > 0) OnSwipeUp?.Invoke();
            else OnSwipeDown?.Invoke();
        }
    }
}

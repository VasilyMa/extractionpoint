using UnityEngine;
using UnityEngine.UI;

public class InteractiveCrate : InteractiveObject
{
    public float Timer;
    private bool isActive = false;
    public override Sprite Icon => throw new System.NotImplementedException();

    public override string Discription => throw new System.NotImplementedException();

    public void Awake()
    {
    }
    public override void BecomeInteractive()
    {
        if(!isActive) PhotonRunHandler.Instance.InvokeOpenEvent();
    }

    public override void StopBeingInteractive()
    {

    }

    public void OnDisable()
    {

    }

    public void InvokeOpenEvent()
    {
        isActive = true;
        State.Instance.InvokeCanvas<InteractiveCanvas, BattleCanvas>().InvokeMessage(new InteractiveViewData(transform, Timer), onFinishOpen);
    }
    
    void onFinishOpen()
    {
        OnlineState.Instance.TestAnimation.SetActive(true);
    }
}

public class InteractiveViewData
{
    public Transform Target;
    public float RemainingTime;

    public InteractiveViewData(Transform target, float remainingTime)
    {
        Target = target;
        RemainingTime = remainingTime;
    }
}
using UnityEngine;

public class PreparePanel : SourcePanel
{
    private InfoLoaderView loaderView;

    public override void Init(SourceCanvas canvasParent)
    {
        base.Init(canvasParent);

        if(loaderView == null) loaderView = GetComponentInChildren<InfoLoaderView>();
    }

    public override void OnDipose()
    {

    }

    public void UpdateInfo(string info)
    {
        loaderView.UpdateInfo(info);
    }

    public void UpdateTimer(int timer)
    {
        loaderView.UpdateTimerView(timer);
    }
    public void UpdateFill(float value, bool isSwitch = false)
    {
        loaderView.UpdateFillView(value, isSwitch);
    }
}

using System;

public class InteractiveCanvas : SourceCanvas
{
    public void InvokeMessage(InteractiveViewData viewData)
    {
        OpenPanel<InteractivePanel>().InvokeViewTimer(viewData);
    }
    public void InvokeMessage(InteractiveViewData viewData, Action callback = null)
    {
        OpenPanel<InteractivePanel>().InvokeViewTimer(viewData, callback);
    }
}

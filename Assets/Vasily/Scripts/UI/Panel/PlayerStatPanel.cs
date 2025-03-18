public class PlayerStatPanel : SourcePanel
{
    public PlayerInfoValueSlotView[] playerInfoValueSlotViews;

    public override void Init(SourceCanvas canvasParent)
    {
        base.Init(canvasParent);

        playerInfoValueSlotViews = GetComponentsInChildren<PlayerInfoValueSlotView>();

        for (int i = 0; i < playerInfoValueSlotViews.Length; i++)
        {
            playerInfoValueSlotViews[i].Init();
        }
    }

    public override void OnDipose()
    {
        base.OnDipose();

        for (int i = 0; i < playerInfoValueSlotViews.Length; i++)
        {
            playerInfoValueSlotViews[i].Dispose();
        }
    }
}

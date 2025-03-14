using UnityEngine.UI;
using UnityEngine;

public class MenuPanel : SourcePanel
{
    [SerializeField] Button _btnStartTheGame;

    public override void Init(SourceCanvas canvasParent)
    {
        _btnStartTheGame.onClick.AddListener(StartGame);

        base.Init(canvasParent);
    }

    void StartGame() => State.Instance.InvokeCanvas<MatchmakingCanvas>();

    public override void OnDipose()
    {
        base.OnDipose();

        _btnStartTheGame.onClick.RemoveAllListeners();
    }
}

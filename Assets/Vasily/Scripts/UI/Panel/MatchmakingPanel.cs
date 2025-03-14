using UnityEngine;
using UnityEngine.UI;

public class MatchmakingPanel : SourcePanel
{
    [SerializeField] Button _solo;
    [SerializeField] Button _multiplayer;
    [SerializeField] Button _close;

    public override void Init(SourceCanvas canvasParent)
    {
        _solo.onClick.AddListener(Singleplayer);
        _multiplayer.onClick.AddListener(Multiplayer);
        _close.onClick.AddListener(Close);
        base.Init(canvasParent);
    }
    void Multiplayer() => _sourceCanvas.OpenPanel<MultiplayerPanel>();
    void Singleplayer() => _sourceCanvas.OpenPanel<SoloPanel>();
    void Close() => State.Instance.InvokeCanvas<MainMenuCanvas>();
    public override void OnDipose()
    {
        _solo.onClick.RemoveAllListeners();
        _multiplayer.onClick.RemoveAllListeners();
        _close.onClick.RemoveAllListeners();
    }
}

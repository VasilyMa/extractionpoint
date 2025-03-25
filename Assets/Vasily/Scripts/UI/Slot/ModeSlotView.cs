using UnityEngine;
using UnityEngine.UI;

public class ModeSlotView : MonoBehaviour
{
    StartMultiplayerView _view;
    [SerializeField] Button _lobby;
    [SerializeField] Button _create;
    [SerializeField] Button _random;

    public void Init(StartMultiplayerView view)
    {
        _view = view;
        _lobby.onClick.AddListener(Lobby);
        _create.onClick.AddListener(Create);
        _random.onClick.AddListener(Random);
    }

    void Create()
    {
        PlayModeEntity.Instance.MultiplayerMode = MultiplayerMode.createLobby;
        OpenCreateLobby();
    }

    void Lobby()
    {
        PlayModeEntity.Instance.MultiplayerMode = MultiplayerMode.findLobby;
        OpenFindLobby();
    }

    void Random()
    {
        PlayModeEntity.Instance.MultiplayerMode = MultiplayerMode.random;
        OpenDifficult();
    }
    
    public void Dispose()
    {
        _create.onClick.RemoveListener(Create);
        _random.onClick.RemoveListener(Random);
        _lobby?.onClick.RemoveListener(Lobby);
    }

    void OpenDifficult() => _view.OpenDifficult();
    void OpenFindLobby() => _view.OpenFindLobby();
    void OpenCreateLobby() => _view.OpenCreateLobby();
}

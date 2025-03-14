using UnityEngine;
using UnityEngine.UI;

public class ModeSlotView : MonoBehaviour
{
    StartMultiplayerView _view;
    [SerializeField] Button _lobby;
    [SerializeField] Button _random;

    public void Init(StartMultiplayerView view)
    {
        _view = view;
        _lobby.onClick.AddListener(Lobby);
        _random.onClick.AddListener(Random);
    }

    void Lobby()
    {
        PlayModeEntity.Instance.MultiplayerMode = MultiplayerMode.lobby;
        OpenDifficult();
    }

    void Random()
    {
        PlayModeEntity.Instance.MultiplayerMode = MultiplayerMode.random;
        OpenDifficult();
    }
    
    void OpenDifficult() => _view.OpenDifficult();
}

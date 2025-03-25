using UnityEngine;
using UnityEngine.UI;

public class LobbyCreateLayout : MonoBehaviour
{
    [SerializeField] Button _btnCreate;
    MultiplayerPanel _panel;

    LobbySettingCreateData _data;

    public LobbyCreateLayout Init(MultiplayerPanel panel)
    {
        _panel = panel;
        _btnCreate.onClick.AddListener(onCreate);
        _data = PhotonInitializer.Instance.CreateLobbyData;
        return this;
    }



    public void SetMode(string mapID)
    {
        _data.MapID = mapID;
    }

    public void SetDifficult(Difficult difficult)
    {
        _data.Difficult = difficult;
    }

    public void SetLobbyName(string name)
    {
        _data.LobbyRoomID = name;
    }

    public void SetRankLevel(int level) 
    {
        _data.RankLevelMinimal = level;
    }

    public void SetPrivate(bool value = false)
    {
        _data.IsPrivate = value;
    }

    void onCreate()
    {
        PhotonInitializer.Instance.CreateLobbyWithCustomProperties();

        OpenLobby();
    }

    public void Dispose()
    {
        _btnCreate.onClick.RemoveAllListeners();
    }

    void OpenLobby() => _panel.OpenLobby();
}

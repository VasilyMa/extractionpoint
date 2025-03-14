using Photon.Realtime;

using UnityEngine;

public class LobbyLayoutView : MonoBehaviour
{
    MultiplayerPanel _panel;
    PlayerInfoScreen _playerInfo;
    PlayerLobbySlotView[] _playerLobbySlotsView;

    public LobbyLayoutView Init(MultiplayerPanel panel)
    {
        _panel = panel;
        _playerInfo = GetComponentInChildren<PlayerInfoScreen>().Init(this);
        _playerLobbySlotsView = GetComponentsInChildren<PlayerLobbySlotView>();

        for (int i = 0; i < _playerLobbySlotsView.Length; i++)
        {
            _playerLobbySlotsView[i].Init(this);
        }

        PhotonInitializer.OnPlayerEnter += onPlayerEnter;

        return this;
    }

    void onPlayerEnter(Player player)
    {
        if (player.CustomProperties.ContainsKey("PlayFabID"))
        {
            string playFabID = (string)player.CustomProperties["PlayFabID"];
            Debug.Log($"Игрок {player.NickName} имеет PlayFabID: {playFabID}");

            for (global::System.Int32 i = 0; i < _playerLobbySlotsView.Length; i++)
            {
                if (_playerLobbySlotsView[i].IsEmpty)
                {
                    _playerLobbySlotsView[i].UpdateSlotInfo(player, playFabID);
                    break;
                }
            }
        }
    }

    public void OpenPlayerInfo(Player player, string playFabID)
    {
        _playerInfo.UpdateInfoPlayer(player, playFabID);
    }
}

using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using Photon.Realtime;

using PlayFab;
using PlayFab.ClientModels;

using UnityEngine;
using UnityEngine.UI;

public class PlayerLobbySlotView : MonoBehaviour
{
    [SerializeField] Image _rankIcon;
    [SerializeField] Text _nickName;
    [SerializeField] Button _more;

    LobbyLayoutView _lobby;
    Player _player;
    string _playFadID;

    public bool IsEmpty => _player == null;

    public void Init(LobbyLayoutView lobby)
    {
        _lobby = lobby;
        _more.onClick.AddListener(onOpenMore);
        gameObject.SetActive(false);
    }

    public void OnReset()
    {

    }

    public void UpdateSlotInfo(Player player, string playFadID)
    {
        _player = player;
        _playFadID = playFadID;

        PlayFabClientAPI.GetUserData(new GetUserDataRequest { PlayFabId = _playFadID },
        result =>
        {
            if (result.Data.TryGetValue("PlayerData", out var equipmentData))
            {
                byte[] binaryData = Convert.FromBase64String(equipmentData.Value);
                using (MemoryStream stream = new MemoryStream(binaryData))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    PlayerData loadedPlayerData = (PlayerData)formatter.Deserialize(stream);
                    onPlayerDataComplete(loadedPlayerData);
                }
            }
            else
            {
                Debug.LogWarning($"No equipment data found for PlayFab ID: {_playFadID}");
            }
        },
        error =>
        {
            Debug.LogError($"Failed to load equipment for PlayFab ID {_playFadID}: {error.GenerateErrorReport()}");
        });

        var request = new GetPlayerProfileRequest
        {
            PlayFabId = _playFadID,
            ProfileConstraints = new PlayerProfileViewConstraints
            {
                ShowDisplayName = true // Запрашиваем никнейм
            }
        };

        PlayFabClientAPI.GetPlayerProfile(request, onPlayerNameComplete, onError);
    }

    void onPlayerDataComplete(PlayerData loadedPlayerData)
    {
        var rankConfig = ConfigModule.GetConfig<RankConfig>();

        if (!string.IsNullOrEmpty(loadedPlayerData.RankID))
        {
            _rankIcon.sprite = rankConfig.GetByID(loadedPlayerData.RankID).SourceRank.Rank_Icon;
        }
    }

    void onPlayerNameComplete(GetPlayerProfileResult result)
    {
        if (!string.IsNullOrEmpty(result.PlayerProfile.DisplayName))
        {
            _nickName.text = result.PlayerProfile.DisplayName;
        }
        else
        {
            _nickName.text = $"Player {UnityEngine.Random.Range(1000, 2000)}";
        }
    }

    void onError(PlayFabError error)
    {
        Debug.LogError($"Cant load play data with error: {error.ErrorMessage}");
    }

    void onOpenMore()
    {
        _lobby.OpenPlayerInfo(_player, _playFadID);
    }

    public void Dispose()
    {
        _more?.onClick.RemoveAllListeners();
    }
}

using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using Photon.Pun;
using Photon.Realtime;

using PlayFab;
using PlayFab.ClientModels;

using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoScreen : MonoBehaviour
{
    LobbyLayoutView _lobby;
    [SerializeField] Text _playerNickName;
    [SerializeField] Image _rankIcon;
    [SerializeField] StatSlotView[] _stats;

    [SerializeField] Button _kickBtn;
    [SerializeField] Button _addToFriend;
    [SerializeField] Button _back;

    PlayerData _currentPlayerData;

    Player _photonPlayer;

    public PlayerInfoScreen Init(LobbyLayoutView lobby)
    {
        _lobby = lobby;
        _stats = GetComponentsInChildren<StatSlotView>();

        for (int i = 0; i < _stats.Length; i++)
        {
            _stats[i].gameObject.SetActive(false);
        }

        _kickBtn.onClick.AddListener(KickFromLobby);
        _addToFriend.onClick.AddListener(AddFriend);
        _back.onClick.AddListener(Back);

        gameObject.SetActive(false);
        return this;
    }

    public void UpdateInfoPlayer(Player photonPlayer, string playerPlayFabID)
    {
        _photonPlayer = photonPlayer; 

        PlayFabClientAPI.GetUserData(new GetUserDataRequest { PlayFabId = playerPlayFabID },
        result =>
        {
            if (result.Data.TryGetValue("PlayerData", out var equipmentData))
            {
                byte[] binaryData = Convert.FromBase64String(equipmentData.Value);
                using (MemoryStream stream = new MemoryStream(binaryData))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    PlayerData loadedPlayerData = (PlayerData)formatter.Deserialize(stream);
                    onPlayerEquipmentComplete(loadedPlayerData);
                }
            }
            else
            {
                Debug.LogWarning($"No equipment data found for PlayFab ID: {playerPlayFabID}");
            }
        },
        error =>
        {
            Debug.LogError($"Failed to load equipment for PlayFab ID {playerPlayFabID}: {error.GenerateErrorReport()}");
        });

        var request = new GetPlayerProfileRequest
        {
            PlayFabId = playerPlayFabID,
            ProfileConstraints = new PlayerProfileViewConstraints
            {
                ShowDisplayName = true // Запрашиваем никнейм
            }
        };

        PlayFabClientAPI.GetPlayerProfile(request, onPlayerProfileComplete, onError);

        if (PhotonNetwork.IsMasterClient)
        {
            _kickBtn.gameObject.SetActive(true);
        }
        else
        {
            _kickBtn.gameObject.SetActive(false);
        }
    }
    void onPlayerEquipmentComplete(PlayerData loadedPlayerData)
    {
        _currentPlayerData = loadedPlayerData;

        var equipConfig = ConfigModule.GetConfig<EquipConfig>();

        WeaponPlayer mainWeapon = null;
        WeaponPlayer heavyWeapon = null;

        if (_currentPlayerData.MainWeapon != null)
        {
            if (!string.IsNullOrEmpty(_currentPlayerData.MainWeapon.KEY_ID))
            {
                mainWeapon = equipConfig.GetEquipBase(_currentPlayerData.MainWeapon.KEY_ID).GetEquip<WeaponPlayer>();
                mainWeapon.SetStats(_currentPlayerData.MainWeapon.Stats);

                var stats = mainWeapon.GetWeaponStats();

                for (int i = 0; i < _stats.Length; i++)
                {
                    string title = stats[i].Name;

                    float value = stats[i].MaxValue;

                    float fillValue = stats[i].BaseValue / stats[i].MaxValue;

                    _stats[i].UpdateView(fillValue, value.ToString(), title);
                }
            }
        }
        else if (_currentPlayerData.HeavyWeapon != null)
        {
            if (!string.IsNullOrEmpty(_currentPlayerData.HeavyWeapon.KEY_ID))
            {
                heavyWeapon = equipConfig.GetEquipBase(_currentPlayerData.HeavyWeapon.KEY_ID).GetEquip<WeaponPlayer>();
                heavyWeapon.SetStats(_currentPlayerData.MainWeapon.Stats);

                var stats = heavyWeapon.GetWeaponStats();

                for (int i = 0; i < _stats.Length; i++)
                {
                    string title = stats[i].Name;

                    float value = stats[i].MaxValue;

                    float fillValue = stats[i].BaseValue / stats[i].MaxValue;

                    _stats[i].UpdateView(fillValue, value.ToString(), title);
                }
            }
        }

        _rankIcon.enabled = false;

        var rankConfig = ConfigModule.GetConfig<RankConfig>();

        if (!string.IsNullOrEmpty(loadedPlayerData.RankID))
        {
            _rankIcon.enabled = true; 

            _rankIcon.sprite = rankConfig.GetByID(loadedPlayerData.RankID).SourceRank.Rank_Icon;
        }
    }
    void onPlayerProfileComplete(GetPlayerProfileResult result)
    {
        if (!string.IsNullOrEmpty(result.PlayerProfile.DisplayName))
        {
            _playerNickName.text = result.PlayerProfile.DisplayName;
        }
        else
        {
            _playerNickName.text = $"Player {UnityEngine.Random.Range(1000, 2000)}";
        }
    }
    void playerViewUpdate(PlayerData loadedPlayerData)
    {
        //TODO set player view
    }
    void Back() => gameObject.SetActive(false);

    void AddFriend()
    {
        //TODO Add to friend
    }

    void KickFromLobby()
    {
        //TODO Kick from lobby

        PhotonInitializer.Instance.DropPlayerName(_photonPlayer);
    }

    void onError(PlayFabError error)
    {
        Debug.LogError($"Ошибка получения никнейма: {error.GenerateErrorReport()}");
    }

    public void Dispose()
    {
        _addToFriend.onClick.RemoveAllListeners();
        _back.onClick.RemoveAllListeners();
        _kickBtn.onClick.RemoveAllListeners();
    }
}

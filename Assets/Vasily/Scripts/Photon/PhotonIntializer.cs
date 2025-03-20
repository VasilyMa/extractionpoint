using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;
using ExitGames.Client.Photon;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector.Editor.GettingStarted;

public class PhotonInitializer : MonoBehaviourPunCallbacks
{
    public string DestinationScene;

    [HideInInspector] public bool InRandom;
    [HideInInspector] public bool InLobby;

    public static Action<bool> OnMatchmakingReady;
    public static Action OnStartSolo;
    public static Action OnRoomListChange;
    public static Action<Player> OnPlayerEnter;
    public static Action<Player> OnPlayerLeft;
    public static PhotonInitializer Instance;

    private string _createdRoomName = null;

    public LobbySettingCreateData CreateLobbyData;
    public LobbySettingSearchData SearchLobbyData;

    private List<RoomInfo> _cachedRoomList = new List<RoomInfo>();


    private void Awake()
    {
        Instance = this;

        if (PhotonNetwork.IsConnectedAndReady)
        {
            Hashtable playerProperties = new Hashtable();
            playerProperties["PlayFabID"] = PlayerEntity.Instance.PlayFabUserID;

            PhotonNetwork.LocalPlayer.SetCustomProperties(playerProperties);
        }
    }

    public void StartSolo()
    {
        InRandom = false;
        InLobby = false;

        _createdRoomName = UnityEngine.Random.Range(0, 10000).ToString();

        int difficult = 0;
        string mapId = PlayModeEntity.Instance.MapID;

        switch (PlayModeEntity.Instance.Difficult)
        {
            case Difficult.easy:
                difficult = 0;
                break;
            case Difficult.medium:
                difficult = 1;
                break;
            case Difficult.hard:
                difficult = 2;
                break;
        } 

        var roomOptions = new RoomOptions { MaxPlayers = 1 };
        roomOptions.CustomRoomProperties = new Hashtable
        {
            { "Difficult", difficult },
            { "MapID", mapId },
        };

        PhotonNetwork.CreateRoom(_createdRoomName, roomOptions);
    }

    public void CreateLobby()
    {
        InRandom = false;
        InLobby = true;

    }
    public void TryToJoin(RoomInfo room)
    {
        if (room == null)
        {
            Debug.LogWarning("Попытка подключения к пустой комнате!");
            return;
        }

        if (!room.IsOpen || !room.IsVisible)
        {
            Debug.LogWarning($"Комната {room.Name} закрыта или невидима.");
            return;
        }

        PhotonNetwork.JoinRoom(room.Name);
        Debug.Log($"Подключаемся к комнате: {room.Name}");
    }
    public void CreateLobbyWithCustomProperties()
    {
        string roomName = CreateLobbyData.LobbyRoomID; Difficult difficulty = CreateLobbyData.Difficult; string mapID = CreateLobbyData.MapID; int levelMinimum = CreateLobbyData.RankLevelMinimal; bool isPrivate = CreateLobbyData.IsPrivate; byte maxPlayers = 2;

        RoomOptions roomOptions = new RoomOptions
        {
            MaxPlayers = maxPlayers,
            IsVisible = !isPrivate,
            IsOpen = true,
            CustomRoomProperties = new ExitGames.Client.Photon.Hashtable
            {
                { "IsLobby" , true },
                { "Difficulty", (int)difficulty },
                { "MapID", mapID },
                { "LevelMinimum", levelMinimum },
                { "IsPrivate", isPrivate }
            },
            CustomRoomPropertiesForLobby = new string[] { "IsLobby", "Difficulty", "MapID", "LevelMinimum", "IsPrivate" }
        };

        PhotonNetwork.CreateRoom(roomName, roomOptions);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);

        foreach (var room in roomList)
        {
            if (room.RemovedFromList)
                _cachedRoomList.RemoveAll(r => r.Name == room.Name);
            else
                _cachedRoomList.Add(room);
        }

        OnRoomListChange?.Invoke();
    }

    public List<RoomInfo> GetFilteredRooms()
    {
        Difficult difficulty = SearchLobbyData.Difficult; string mapID = SearchLobbyData.MapID; int levelMinimum = SearchLobbyData.RankLevelMinimal; bool isPrivate = false;

        return _cachedRoomList.Where(room =>
        {
            if (!room.IsOpen || !room.IsVisible) return false; // Игнорируем закрытые или невидимые комнаты

            var properties = room.CustomProperties;

            if ((!properties.ContainsKey("IsLobby") || !(bool)properties["IsLobby"]))
                return false;

            if (isPrivate && (!properties.ContainsKey("IsPrivate") || !(bool)properties["IsPrivate"]))
                return false;

            if (!isPrivate && properties.ContainsKey("IsPrivate") && (bool)properties["IsPrivate"])
                return false;

            if (properties.ContainsKey("Difficulty") && (Difficult)(int)properties["Difficulty"] != difficulty && difficulty != Difficult.any)
                return false;

            if (properties.ContainsKey("MapID") && (string)properties["MapID"] != mapID && !string.IsNullOrEmpty(mapID))
                return false;

            if (properties.ContainsKey("LevelMinimum") && (int)properties["LevelMinimum"] > levelMinimum)
                return false;

            return true;
        }).ToList();
    }

    public void DropPlayerName(Player player)
    {
        PhotonNetwork.CloseConnection(player);
    }

    public void FindRoom()
    {
        InRandom = true;
        InLobby = false;

        PhotonNetwork.NickName = $"Player number {UnityEngine.Random.Range(0, 100)}";

        if (PhotonNetwork.InRoom)
        {
            Debug.LogWarning("Already in a room, skipping room search.");
            return;
        }

        Debug.Log("Trying to find a room...");
        PhotonNetwork.JoinRandomRoom();
    }
    
    public void DropFindRoom()
    {
        InRandom = false;
        InLobby = false;

        if (PhotonNetwork.InRoom) PhotonNetwork.LeaveRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        _createdRoomName = UnityEngine.Random.Range(0, 10000).ToString();

        Debug.Log($"No available rooms found. Creating a new one... {_createdRoomName}");

        int difficult = 0;
        string mapId = PlayModeEntity.Instance.MapID;

        switch (PlayModeEntity.Instance.Difficult)
        {
            case Difficult.easy:
                difficult = 0;
                break;
            case Difficult.medium:
                difficult = 1;
                break;
            case Difficult.hard:
                difficult = 2;
                break;
        }

        var roomOptions = new RoomOptions { MaxPlayers = 2 };
        roomOptions.CustomRoomProperties = new Hashtable
        {
            { "Difficult", difficult },
            { "MapID", mapId },
        };

        PhotonNetwork.CreateRoom(_createdRoomName, roomOptions);

        OnMatchmakingReady?.Invoke(false);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError($"Failed to create room: {message}");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log($"Joined room: {PhotonNetwork.CurrentRoom.Name}");

        if (_createdRoomName != null && _createdRoomName == PhotonNetwork.CurrentRoom.Name)
        {
            Debug.Log("This is our own created room, waiting for players...");
        }
        else
        {
            Debug.Log("Joined an existing room, removing our own if it was created.");
            _createdRoomName = null; // Очистка, если мы нашли чужую комнату
        }

        if (PhotonNetwork.CurrentRoom.MaxPlayers == 1)
        {
            if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("MapID", out object sceneName))
            {
                Debug.Log($"Загружаю сцену: {sceneName}");
                PhotonNetwork.LoadLevel(sceneName.ToString());
            }
            else
            {
                Debug.LogWarning("SceneName не найден в Room Properties!");
            }
        }

        CheckRoomStatus();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);

        OnMatchmakingReady?.Invoke(false);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log($"Player {newPlayer.NickName} joined.");

        OnPlayerEnter?.Invoke(newPlayer);

        CheckRoomStatus();
    }

    private void CheckRoomStatus()
    {
        Debug.Log($"Current players in room: {PhotonNetwork.CurrentRoom.PlayerCount}");

        if (PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            Debug.Log("Room is full, ready to start!");
            OnMatchmakingReady?.Invoke(true);
            // Здесь можно вызывать коллбэк для UI, например, кнопку старта
        }
    }

    public void LoadSceneWithPhoton()
    {
        if (PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("MapID", out object sceneName))
            {
                Debug.Log($"Загружаю сцену: {sceneName}");
                PhotonNetwork.LoadLevel(sceneName.ToString());
                ConfigModule.GetConfig<PlayFabConfig>().StartMission();
            }
            else
            {
                Debug.LogWarning("SceneName не найден в Room Properties!");
            }
        }
    }
}

public class LobbySettingSearchData
{
    public int RankLevelMinimal;
    public Difficult Difficult;
    public string MapID;
}

public class LobbySettingCreateData
{
    public int RankLevelMinimal;
    public Difficult Difficult;
    public string MapID;
    public bool IsPrivate;
    public string LobbyRoomID;
}
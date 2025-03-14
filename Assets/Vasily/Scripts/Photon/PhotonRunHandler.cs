using UnityEngine;
using Photon.Pun;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using PlayFab;

public class PhotonRunHandler : MonoBehaviourPunCallbacks
{
    public static PhotonRunHandler Instance;
    [SerializeField] PhotonPlayer PlayerPref;
    [HideInInspector] public bool IsAllPlayersReady;
    [SerializeField] private PlayerSpawnPoint[] playerSpawnPoints;
    [HideInInspector] public Dictionary<int, PhotonPlayer> Players = new Dictionary<int, PhotonPlayer>();
    [HideInInspector] public List<PhotonEnemy> ActiveUnits = new List<PhotonEnemy>();

    public const float TICK_GLOBAL_SYNC = 1f;
    public const float TICK_STAT_SYNC = 0.5f;

    private double lastTickGlobalSyncTime;
    private double lastTickStatSyncTime;

    public IEnumerator Init()
    {
        if(Instance == null) Instance = this;

        if(playerSpawnPoints == null) playerSpawnPoints = FindObjectsOfType<PlayerSpawnPoint>();

        int indexInRoom = 0;

        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            if (PhotonNetwork.LocalPlayer == PhotonNetwork.PlayerList[i])
            {
                indexInRoom = i;
                break;
            }
        }

        object[] sendData = new object[] { PlayerEntity.Instance.PlayFabUserID, indexInRoom };

        var player = PhotonNetwork.Instantiate($"Unit/{PlayerPref.name}", Vector3.zero, Quaternion.identity, data: sendData);

        yield return StartCoroutine(WaitStartGame());
    }
    IEnumerator WaitStartGame()
    {
        var preparePanel = State.Instance.GetCanvas<BattleCanvas>().GetPanel<PreparePanel>();

        preparePanel.UpdateInfo("Wait other player");

        yield return new WaitUntil(() => Players.Count == PhotonNetwork.CurrentRoom.MaxPlayers);

        preparePanel.UpdateInfo("Start in...");

        float remainingTime = ConfigModule.GetConfig<PhotonConfig>().ReadyToStart;

        int secondsLeft = Mathf.CeilToInt(remainingTime); // Считаем секунды

        preparePanel.UpdateTimer(secondsLeft);

        while (remainingTime > 0)
        {
            float fillTimer = 0f; // Счетчик для анимации заполнения
            float startTime = Time.time;

            while (fillTimer < 1f)
            {
                fillTimer = (Time.time - startTime) / 1f; // Линейное заполнение за 1 секунду
                preparePanel.UpdateFill(fillTimer);
                yield return null;
            }

            // Очищаем fillAmount после 1 секунды
            preparePanel.UpdateFill(0, true);

            remainingTime -= 1f;
            secondsLeft--;

            preparePanel.UpdateTimer(secondsLeft); // Обновляем UI

            yield return null;
        }

        if (PhotonNetwork.IsMasterClient)
        {
            foreach (var player in Players)
            {
                player.Value.photonView.RPC("Init", RpcTarget.AllViaServer);
            }
        }

        yield return new WaitUntil(() => Players.Values.All(player => player.IsInitAndReady));
    }
    public static byte[] SerializeObject(object obj)
    {
        if (obj is null) return null;

        BinaryFormatter formatter = new BinaryFormatter();
        using (MemoryStream memoryStream = new MemoryStream())
        {
            formatter.Serialize(memoryStream, obj);
            return memoryStream.ToArray();
        }
    }
    public static object DeserializeObject(byte[] bytes)
    {
        if (bytes == null) return null;

        BinaryFormatter formatter = new BinaryFormatter();
        using (MemoryStream memoryStream = new MemoryStream(bytes))
        {
            return formatter.Deserialize(memoryStream);
        }
    }

    public PlayerSpawnPoint GetPlayerSpawnPoint(int index)
    {
        return playerSpawnPoints[index];
    }

    public void RunPhotonLogic()
    {
        if (PhotonNetwork.Time > lastTickGlobalSyncTime + TICK_GLOBAL_SYNC)
        {
            lastTickGlobalSyncTime = PhotonNetwork.Time;

            var sendData = new SendSyncLogicData(ActiveUnits.Count);

            for (global::System.Int32 i = 0; i < ActiveUnits.Count; i++)
            {
                var target = ActiveUnits[i].GetNearestTarget();

                if (target == null) continue; 

                sendData.viewID[i] = ActiveUnits[i].photonView.ViewID;
                sendData.targetViewID[i] = target.photonView.ViewID;
            }

            foreach (var player in Players.Values)
            {
                player.photonView.RPC("SendRPCSyncLogic", RpcTarget.AllViaServer, SerializeObject(sendData));
            }
        }

        if (PhotonNetwork.Time > lastTickStatSyncTime + TICK_STAT_SYNC)
        {
            lastTickStatSyncTime = PhotonNetwork.Time;


        }

    }
    public void UpdateSyncLogic(SendSyncLogicData dataSync)
    {
        for (int i = 0; i < dataSync.viewID.Length; i++)
        {
            if (i >= ActiveUnits.Count) break;

            var enemy = ActiveUnits[i];

            if (enemy.photonView.ViewID == dataSync.viewID[i])
            {
                if (Players.TryGetValue(dataSync.targetViewID[i], out PhotonPlayer player))
                {
                    enemy.UpdateTarget(player);
                }
            }
        }
    }
    public void UpdateSyncStat(SendSyncStatData dataSync)
    {
        for (int i = 0; i < dataSync.viewID.Length; i++)
        {
            if (i >= ActiveUnits.Count) break;

            var enemy = ActiveUnits[i];

            if (enemy.photonView.ViewID == dataSync.viewID[i])
            {
                if (OnlineState.Instance.TryGetEnemy(dataSync.viewID[i].ToString(),out PhotonEnemy photonEnemy))
                {

                }
            }
        }
    }
    public void InvokeOpenEvent()
    {
        foreach (var player in Players.Values)
        {
            player.photonView.RPC("SendRPCInteractiveEvent", RpcTarget.AllViaServer);
        }
    }

    public bool TryGetPlayer(int id, out PhotonPlayer player)
    {
        if (Players.TryGetValue(id, out player))  return true;

        return false;
    }
}
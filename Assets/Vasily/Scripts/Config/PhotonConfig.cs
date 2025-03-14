using System.Collections;
using System.Collections.Generic;

using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

[CreateAssetMenu(fileName = "PhotonConfig", menuName = "Config/Photon")]
public class PhotonConfig : Config, IConnectionCallbacks
{
    [Tooltip("Time delay before match is starting")][Range(3, 10)] public float ReadyToStart;
    public bool IsConnected;

    public override IEnumerator Init()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();
        yield return new WaitUntil( ()=> PhotonNetwork.IsConnectedAndReady);
    }

    public void OnConnected()
    {

    }

    public void OnConnectedToMaster()
    {
        IsConnected = true;
        Debug.Log("Connected to master server");
    }

    public void OnCustomAuthenticationFailed(string debugMessage)
    {
        Debug.Log($"Connected to master server failed {debugMessage}");
    }

    public void OnCustomAuthenticationResponse(Dictionary<string, object> data)
    {

    }

    public void OnDisconnected(DisconnectCause cause)
    {

    }

    public void OnRegionListReceived(RegionHandler regionHandler)
    {

    }
}

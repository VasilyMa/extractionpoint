using System.Collections;
using System.Collections.Generic;

using Photon.Realtime;

using UnityEngine;
using UnityEngine.UI;

public class FindedMatchSlot : MonoBehaviour
{
    Button _btn;
    RoomInfo _roomData;

    public void Init()
    {
        _btn = GetComponent<Button>();
        _btn.onClick.AddListener(TryToJoin);
        gameObject.SetActive(false);
    }

    public void UpdateInfo(RoomInfo roomInfo)
    {
        if (roomInfo == null)
        {
            gameObject.SetActive(false);
            return;
        }

        _roomData = roomInfo;

    }

    void TryToJoin()
    {
        PhotonInitializer.Instance.TryToJoin(_roomData);
    }

    public void Dispose()
    {
        _btn.onClick.RemoveAllListeners();
    }
}

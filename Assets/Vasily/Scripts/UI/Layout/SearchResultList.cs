using UnityEngine;

public class SearchResultList : MonoBehaviour
{
    FindedMatchSlot[] findedSlots;

    public SearchResultList Init(LobbyListLayout layout)
    {
        findedSlots = GetComponentsInChildren<FindedMatchSlot>();
        PhotonInitializer.OnRoomListChange += Refresh;

        return this;
    }

    public void Refresh()
    {
        var listRoom = PhotonInitializer.Instance.GetFilteredRooms();

        int index = 0;

        foreach (var room in listRoom) 
        {
            if(index >= findedSlots.Length) break;

            findedSlots[index].UpdateInfo(room);

            index++;
        }
    }

    public void Dispose()
    {
        for (int i = 0; i < findedSlots.Length; i++)
        {
            findedSlots[i].Dispose();
        }

        PhotonInitializer.OnRoomListChange -= Refresh;
    }
}

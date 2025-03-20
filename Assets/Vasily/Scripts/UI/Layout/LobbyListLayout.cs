using UnityEngine;
using UnityEngine.UI;

public class LobbyListLayout : MonoBehaviour
{
    MultiplayerPanel _panel;
    [SerializeField] Button _refresh;
    SearchResultList _searchResults;
    SearchSettingsList _searchSettings;


    public LobbyListLayout Init(MultiplayerPanel panel)
    {
        _panel = panel;

        _refresh.onClick.AddListener(onRefresh);

        _searchResults = GetComponentInChildren<SearchResultList>().Init(this);
        _searchSettings = GetComponentInChildren<SearchSettingsList>().Init(this);

        return this;
    }

    void onRefresh()
    {
        PhotonInitializer.OnRoomListChange?.Invoke();
    }

    public void Dispose()
    {
        _refresh.onClick.RemoveAllListeners();
    }
}

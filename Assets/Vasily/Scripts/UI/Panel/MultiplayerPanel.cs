using System;

using UnityEngine;
using UnityEngine.UI;

public class MultiplayerPanel : SourcePanel
{
    StartMultiplayerView _multiplayerView;
    DifficultLayout _difficultView;
    LobbyLayoutView _lobbyView;
    LobbyCreateLayout _lobbyCreateView;
    RandomLayoutView _randomView;
    LobbyListLayout _lobbyListView;
    [SerializeField] Button _back;

    public override void Init(SourceCanvas canvasParent)
    {
        _multiplayerView = FindObjectOfType<StartMultiplayerView>().Init(this);
        _difficultView = FindObjectOfType<DifficultLayout>().Init(this);
        _randomView = FindObjectOfType<RandomLayoutView>().Init(this);
        _lobbyView = FindObjectOfType<LobbyLayoutView>().Init(this);
        _lobbyListView = FindObjectOfType<LobbyListLayout>().Init(this);
        _lobbyCreateView = FindObjectOfType<LobbyCreateLayout>().Init(this);
        _back.onClick.AddListener(Back);

        OpenMultiplayer();

        base.Init(canvasParent);
        return;
    }

    public override void OnOpen(params Action[] onComplete)
    {
        if (PhotonInitializer.Instance.InRandom)
        {
            OpenRandom();
        }
        else if (PhotonInitializer.Instance.InLobby)
        {
            OpenLobby();
        }
        else
        {
            OpenMultiplayer();
        }

        base.OnOpen(onComplete);
    }


    public void OpenMultiplayer()
    {
        _lobbyListView.gameObject.SetActive(false);
        _randomView.gameObject.SetActive(false);
        _difficultView.gameObject.SetActive(false);
        _lobbyView.gameObject.SetActive(false);
        _lobbyCreateView.gameObject.SetActive(false);
        _multiplayerView.gameObject.SetActive(true);
    }
    public void OpenFindLobby()
    {
        _randomView.gameObject.SetActive(false);
        _difficultView.gameObject.SetActive(false);
        _lobbyView.gameObject.SetActive(false);
        _multiplayerView.gameObject.SetActive(false);
        _lobbyCreateView.gameObject.SetActive(false);
        _lobbyListView.gameObject.SetActive(true);
    }

    public void OpenCreateLobby()
    {
        _lobbyListView.gameObject.SetActive(false);
        _multiplayerView.gameObject.SetActive(false);
        _randomView.gameObject.SetActive(false);
        _difficultView.gameObject.SetActive(false);
        _lobbyView.gameObject.SetActive(false);
        _lobbyCreateView.gameObject.SetActive(true);
    }

    public void OpenLobby()
    {
        _lobbyListView.gameObject.SetActive(false);
        _multiplayerView.gameObject.SetActive(false);
        _randomView.gameObject.SetActive(false);
        _difficultView.gameObject.SetActive(false);
        _lobbyCreateView.gameObject.SetActive(false);

        _lobbyView.Open();
    }
    public void OpenRandom()
    {
        _lobbyListView.gameObject.SetActive(false);
        _multiplayerView.gameObject.SetActive(false);
        _difficultView.gameObject.SetActive(false);
        _lobbyView.gameObject.SetActive(false);
        _lobbyCreateView.gameObject.SetActive(false);
        _randomView.gameObject.SetActive(true);
        
        PhotonInitializer.Instance.FindRoom();
    }

    public void OpenDifficult()
    {
        _lobbyListView.gameObject.SetActive(false);
        _multiplayerView.gameObject.SetActive(false);
        _randomView.gameObject.SetActive(false);
        _lobbyView.gameObject.SetActive(false);
        _difficultView.gameObject.SetActive(true);
    }

    void Back() => _sourceCanvas.OpenPanel<MatchmakingPanel>();

    public override void OnDipose()
    {
        base.OnDipose();

        _back.onClick.RemoveAllListeners();

    }
}

using UnityEngine;
using UnityEngine.UI;

public class SetDifficultLayout : MonoBehaviour
{
    [SerializeField] Button _easyBtn;
    [SerializeField] Button _mediumBtn;
    [SerializeField] Button _hardBtn;

    public void Init()
    {
        _easyBtn.onClick.AddListener(onEasySet);
        _mediumBtn.onClick.AddListener(onMediumSet);
        _hardBtn.onClick.AddListener(onHardSet);
    }

    void onEasySet()
    {
        PhotonInitializer.Instance.SearchLobbyData.Difficult = Difficult.easy;
    }

    void onMediumSet()
    {
        PhotonInitializer.Instance.SearchLobbyData.Difficult = Difficult.medium;
    }

    void onHardSet()
    {
        PhotonInitializer.Instance.SearchLobbyData.Difficult = Difficult.hard;
    }

    public void Dispose()
    {
        _easyBtn.onClick.RemoveAllListeners();
        _mediumBtn.onClick.RemoveAllListeners();
        _hardBtn.onClick.RemoveAllListeners();
    }
}

using UnityEngine;
using UnityEngine.UI;

public class RandomLayoutView : MonoBehaviour
{
    MultiplayerPanel _panel;

    [SerializeField] Text _loadingInfo;
    [SerializeField] Text _tips;
    [SerializeField] Button _cancel;
    Animation _animationLoading;

    public RandomLayoutView Init(MultiplayerPanel panel)
    {
        _panel = panel;
        _cancel.onClick.AddListener(Cancel);
        PhotonInitializer.OnMatchmakingReady += UpdateInfoView;
        _animationLoading = GetComponentInChildren<Animation>();
        return this;
    }

    void UpdateInfoView(bool value)
    {
        if (value)
        {
            _loadingInfo.text = "TEAMMATE IS FOUND";

            PhotonInitializer.Instance.LoadSceneWithPhoton();
        }
        else
        {
            _loadingInfo.text = "FINDING A TEAMMATE";

            _animationLoading.Play();
        }
    }

    void Cancel()
    {
        PhotonInitializer.Instance.DropFindRoom();

        _panel.OpenMultiplayer();
    }
}

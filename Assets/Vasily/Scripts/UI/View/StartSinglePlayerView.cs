using Sirenix.Utilities;

using UnityEngine;
using UnityEngine.UI;

public class StartSinglePlayerView : MonoBehaviour
{
    [SerializeField] Button _back;
    private DifficultSlotView[] _difficultSlots;
    private SoloPanel _panel;

    public StartSinglePlayerView Init(SoloPanel panel)
    {
        _panel = panel;
        _difficultSlots = GetComponentsInChildren<DifficultSlotView>();
        _difficultSlots.ForEach(slot => slot.Init(StartSolo));
        _back.onClick.AddListener(Back);
        gameObject.SetActive(false);
        return this;
    }

    void StartSolo() => PhotonInitializer.Instance.StartSolo();
    void Back() => _panel.OpenMapView(); 
    public void Dispose()
    {
        _difficultSlots?.ForEach(slot => slot.Dispose());   
        _back.onClick.RemoveAllListeners();
    }
}

using Sirenix.Utilities;

using UnityEngine;
using UnityEngine.UI;

public class SelectMapView : MonoBehaviour
{
    [SerializeField] Button _back;
    private MapSlotView[] _mapSlots;
    private SoloPanel _panel;

    public SelectMapView Init(SoloPanel panel)
    {
        _panel = panel; 
        _back.onClick.AddListener(Back);
        _mapSlots = GetComponentsInChildren<MapSlotView>();
        _mapSlots.ForEach(slot => slot.Init(_panel));
        gameObject.SetActive(false);
        return this;
    }

    void Back() => State.Instance.GetCanvas<MatchmakingCanvas>().OpenPanel<MatchmakingPanel>();

    public void UpdateView()
    {

    }

    public void Dispose()
    {
        _mapSlots?.ForEach(slot => slot.Dispose());
        _back.onClick.RemoveAllListeners();
    }
}

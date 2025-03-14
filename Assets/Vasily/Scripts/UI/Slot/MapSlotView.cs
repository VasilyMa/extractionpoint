using UnityEngine;
using UnityEngine.UI;

public class MapSlotView : MonoBehaviour, ISerializationCallbackReceiver
{
    private SoloPanel _panel;
    private Button _btn;

#if UNITY_EDITOR
    public UnityEditor.SceneAsset SceneTarget;
#endif
    [SerializeField][ReadOnlyInspector] private string _sceneTargetID;

    public void Init(SoloPanel panel)
    {
        _panel = panel;

        _btn = gameObject.AddComponent<Button>();
        _btn.onClick.AddListener(InvokeDifficultView);

        if (string.IsNullOrEmpty(_sceneTargetID)) Disable();
        else Enable(); 

    }

#if UNITY_EDITOR
    public void OnBeforeSerialize()
    {
        if (SceneTarget != null) _sceneTargetID = SceneTarget.name; 
    }

    public void OnAfterDeserialize()
    {

    }
#endif

    public void InvokeDifficultView()
    {
        PlayModeEntity.Instance.MapID = _sceneTargetID;
        _panel.OpenSinglePlayerView();
    }

    public void Dispose()
    {
        _btn.onClick.RemoveAllListeners();
    }

    void Enable()
    {
        _btn.interactable = true;
    }

    void Disable()
    {
        _btn.interactable = false;
    }
}

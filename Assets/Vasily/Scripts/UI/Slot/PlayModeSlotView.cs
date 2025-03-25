using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class PlayModeSlotView : MonoBehaviour, ISerializationCallbackReceiver
{
    Button _btn;

#if UNITY_EDITOR
    public UnityEditor.SceneAsset SceneTarget;
#endif
    [SerializeField][ReadOnlyInspector] private string _sceneTargetID;
    public void Init()
    {
        _btn = GetComponent<Button>();
        _btn.onClick.AddListener(onPlayModeSelect);
    }

    void onPlayModeSelect()
    {
        PhotonInitializer.Instance.SearchLobbyData.MapID = _sceneTargetID;
    }

    public void Dispose()
    {
        _btn.onClick.RemoveAllListeners();
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

}

using UnityEngine;

public class StartMultiplayerView : MonoBehaviour
{
    ModeSlotView[] _modes;
    MultiplayerPanel _panel;

    public StartMultiplayerView Init(MultiplayerPanel panel)
    {
        _panel = panel;
        _modes = FindObjectsOfType<ModeSlotView>();

        for (int i = 0; i < _modes.Length; i++) _modes[i].Init(this);

        return this;
    }

    public void OpenDifficult() => _panel.OpenDifficult();
}

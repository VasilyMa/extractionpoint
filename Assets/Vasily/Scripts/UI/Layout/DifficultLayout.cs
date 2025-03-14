using UnityEngine;

public class DifficultLayout : MonoBehaviour
{
    MultiplayerPanel _panel;
    DifficultSlotView[] _diffSlots;

    public DifficultLayout Init(MultiplayerPanel panel)
    {
        _panel = panel;

        _diffSlots = FindObjectsOfType<DifficultSlotView>();

        for (int i = 0; i < _diffSlots.Length; i++) _diffSlots[i].Init(OpenNext);
        return this;
    }

    void OpenNext()
    {
        switch (PlayModeEntity.Instance.MultiplayerMode)
        {
            case MultiplayerMode.lobby:
                _panel.OpenLobby();
                break;
            case MultiplayerMode.random:
                _panel.OpenRandom();
                break;
        }
    }
}

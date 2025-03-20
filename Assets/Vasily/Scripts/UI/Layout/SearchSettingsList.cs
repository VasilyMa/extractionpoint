using UnityEngine;

public class SearchSettingsList : MonoBehaviour
{
    SetDifficultLayout _difficultSetLayout;
    SetPlayModeLayout _playModeLayout;
    SetRankLayout _rankLayout;

    public SearchSettingsList Init(LobbyListLayout layout)
    {
        _difficultSetLayout = GetComponentInChildren<SetDifficultLayout>();
        _difficultSetLayout.Init();

        _playModeLayout = GetComponentInChildren<SetPlayModeLayout>();
        _playModeLayout.Init();

        _rankLayout = GetComponentInChildren<SetRankLayout>();
        _rankLayout.Init();

        return this;
    }

    public void Dispose()
    {
        _difficultSetLayout?.Dispose();
    }
}

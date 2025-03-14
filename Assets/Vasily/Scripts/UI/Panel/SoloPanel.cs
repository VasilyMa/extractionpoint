using System;

using UnityEngine;
using UnityEngine.UI;

public class SoloPanel : SourcePanel
{
    [SerializeField] Button _back;
    [SerializeField] StartSinglePlayerView _singlePlayerView;
    [SerializeField] SelectMapView _selectMapView;

    public override void Init(SourceCanvas canvasParent)
    {
        _back.onClick.AddListener(Back);
        
        _selectMapView = GetComponentInChildren<SelectMapView>().Init(this);
        _singlePlayerView = GetComponentInChildren<StartSinglePlayerView>().Init(this);

        base.Init(canvasParent);
    }

    public override void OnOpen(params Action[] onComplete)
    {
        _selectMapView.gameObject.SetActive(true);
        base.OnOpen(onComplete);
    }

    public override void OnCLose(params Action[] onComplete)
    {
        _selectMapView.gameObject.SetActive(false);
        _singlePlayerView.gameObject.SetActive(false); 
        base.OnCLose(onComplete);
    }

    public void OpenSinglePlayerView()
    {
        _singlePlayerView.gameObject.SetActive(true);
        _selectMapView.gameObject.SetActive(false);
    }
    
    public void OpenMapView()
    {
        _singlePlayerView.gameObject.SetActive(false);
        _selectMapView.gameObject.SetActive(true);
    }

    void Back() => _sourceCanvas.OpenPanel<MatchmakingPanel>();

    public override void OnDipose()
    {
        base.OnDipose();
        _selectMapView.Dispose();
        _singlePlayerView.Dispose();
        _back.onClick.RemoveAllListeners();
    }
}

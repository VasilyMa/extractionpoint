using System.Collections;

using PlayFab;
using PlayFab.ClientModels;

using UnityEngine;

[CreateAssetMenu(fileName = "PlayFabConfig", menuName = "Config/PlayFab")]
public class PlayFabConfig : Config
{
    [HideInInspector] public bool isConnectAndReady;
    [HideInInspector] public bool isLoadedData;

    public override IEnumerator Init()
    {

        isConnectAndReady = false;
        isLoadedData = false;

        if (PlayerPrefs.HasKey(GuestLoginScreen.PlayerPrefsKey))
        {
            string uniqueID = PlayerPrefs.GetString(GuestLoginScreen.PlayerPrefsKey);

            var request = new LoginWithCustomIDRequest
            {
                CustomId = uniqueID,
                CreateAccount = false // ������� ��� ����������
            };

            PlayFabClientAPI.LoginWithCustomID(request, result =>
            {
                isConnectAndReady = true;
            }, error =>
            {
                Debug.LogError("������ �����: " + error.GenerateErrorReport());
            });
        }
        else State.Instance.GetCanvas<LoadingCanvas>().OpenPanel<LoginPanel>();

        yield return new WaitUntil(() => isConnectAndReady );

        SaveModule.LoadAllDataFromPlayFab(()=> isLoadedData = true);

        yield return new WaitUntil(() => isLoadedData );
    }


    public void RequestMissionReward(string difficulty)
    {
        var request = new ExecuteCloudScriptRequest
        {
            FunctionName = "RewardCurrency",
            FunctionParameter = new { difficulty = difficulty },
            GeneratePlayStreamEvent = true // ����� ���������, ���� PlayStream �� �����
        };

        PlayFabClientAPI.ExecuteCloudScript(request, OnSuccess, OnError);
    }

    private void OnSuccess(ExecuteCloudScriptResult result)
    {
        // ������ JSON-����� �� PlayFab
        string message = result.FunctionResult != null ? result.FunctionResult.ToString() : "������ ��������� ������";
        Debug.Log($"CloudScript Success: {message}");
    }

    private void OnError(PlayFabError error)
    {
        Debug.LogError($"CloudScript Error: {error.GenerateErrorReport()}");
    }
    public void StartMission()
    {
        // �������� ������� ��� ������ ������� Cloud Script
        var request = new ExecuteCloudScriptRequest
        {
            FunctionName = "StartMission",
            FunctionParameter = new { },
            GeneratePlayStreamEvent = true // ����� ���������, ���� PlayStream �� �����
        };

        PlayFabClientAPI.ExecuteCloudScript(request, OnMissionStartSuccess, OnMissionStartError);
    }

    private void OnMissionStartSuccess(ExecuteCloudScriptResult result)
    {
        if (result.FunctionResult != null)
        {
            Debug.Log("Mission started successfully.");
        }
        else
        {
            Debug.LogError("Error starting mission.");
        }
    }

    private void OnMissionStartError(PlayFabError error)
    {
        Debug.LogError($"CloudScript Error while starting mission: {error.GenerateErrorReport()}");
    }
}

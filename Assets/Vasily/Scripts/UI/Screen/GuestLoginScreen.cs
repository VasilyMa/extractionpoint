using System;
using System.Collections;

using PlayFab;
using PlayFab.ClientModels;

using UnityEngine;
using UnityEngine.UI;

public class GuestLoginScreen : MonoBehaviour
{
    public const string PlayerPrefsKey = "PlayFabPlayerID";

    private LoginPanel _loginPanel;

    [SerializeField] InputField _loginField;
    [SerializeField] Text _result;

    [SerializeField] Button _enterButton;
    [SerializeField] Button _loginButton;

    string UniqueID;

    public void Init(LoginPanel logPanel)
    {
        _loginPanel = logPanel;
        _enterButton.onClick.AddListener(RegisterNewUser);
        _loginButton.onClick.AddListener(ToLogin);
        _result.text = "Enter nickname!";


        /*if (PlayerPrefs.HasKey(PlayerPrefsKey))
        {
            savedID = PlayerPrefs.GetString(PlayerPrefsKey);
        }*/
    }

    IEnumerator WaitEnter()
    {
        yield return new WaitUntil(() => ConfigModule.AllConfigLoaded);
    }
    void Enter()
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = UniqueID,
            CreateAccount = false // Аккаунт уже существует
        };

        PlayFabClientAPI.LoginWithCustomID(request, result =>
        {
            ConfigModule.GetConfig<PlayFabConfig>().isConnectAndReady = true;
            _result.text = $"Succsess enter!";
            Debug.Log("Успешный вход! ID: " + UniqueID);
        }, error =>
        {
            _result.text = $"{error.ErrorMessage}";

            Debug.LogError("Ошибка входа: " + error.GenerateErrorReport());
        });
    }

    void RegisterNewUser()
    {
        UniqueID = Guid.NewGuid().ToString(); // Генерируем уникальный ID

        var request = new LoginWithCustomIDRequest
        {
            CustomId = UniqueID,
            CreateAccount = true
        };

        PlayFabClientAPI.LoginWithCustomID(request, result =>
        {
            Debug.Log("Регистрация успешна! ID: " + UniqueID);

            // Сохраняем ID для автологина
            PlayerPrefs.SetString(PlayerPrefsKey, UniqueID);
            PlayerPrefs.Save();

            // Сохраняем никнейм
            UpdateDisplayName(_loginField.text);

            State.Instance.RunCoroutine(WaitEnter(), Enter);

        }, error =>
        {
            Debug.LogError("Ошибка регистрации: " + error.GenerateErrorReport());
        });
    }
    
    void UpdateDisplayName(string nickname)
    {
        var request = new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = nickname
        };

        PlayFabClientAPI.UpdateUserTitleDisplayName(request, result =>
        {
            Debug.Log("Никнейм сохранён: " + nickname);
        }, error =>
        {
            Debug.LogError("Ошибка сохранения никнейма: " + error.GenerateErrorReport());
        });
    }

    void ToLogin()
    {
        _loginPanel.OpenLoginScreen();
    }

    public void OnDispose()
    {
        _loginButton.onClick.RemoveAllListeners();
        _enterButton.onClick.RemoveAllListeners();
    }
}

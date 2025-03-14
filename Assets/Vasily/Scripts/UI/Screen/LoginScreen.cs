using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;

public class LoginScreen : MonoBehaviour
{
    private LoginPanel _loginPanel;

    private const string _rememberKey = "RememberMe";
    private const string _passwordKey = "PasswordID";
    private const string _emailKey = "emailID";

    [SerializeField] InputField _loginField;
    [SerializeField] InputField _passField;
    [SerializeField] Text _result;
    [SerializeField] Text _rememberMe;


    [SerializeField] Button _loginButton;
    [SerializeField] Button _rememberButton;
    [SerializeField] Button _back;

    bool isRemember;

    public void Init(LoginPanel logPanel)
    {
        _loginPanel = logPanel;
        _loginButton.onClick.AddListener(Login);
        _rememberButton.onClick.AddListener(Remember);
        _back.onClick.AddListener(ToGuest);
        _result.text = "Enter login and password!";

        if (PlayerPrefs.HasKey(_rememberKey))
        {
            int value = PlayerPrefs.GetInt(_rememberKey);

            if (value == 1)
            {
                isRemember = true;
                _rememberMe.fontStyle = FontStyle.BoldAndItalic;

                string email = string.Empty;
                string password = string.Empty;

                if(PlayerPrefs.HasKey(_emailKey)) email = PlayerPrefs.GetString(_emailKey);
                if(PlayerPrefs.HasKey(_passwordKey)) password = PlayerPrefs.GetString(_passwordKey);

                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password)) return;

                _loginField.text = email;
                _passField.text = password;
            }
        }
    }

    void ToGuest()
    {
        _loginPanel.OpenGuestScreen();
    }

    void Remember()
    {
        if (PlayerPrefs.HasKey(_rememberKey))
        {
            int value = PlayerPrefs.GetInt(_rememberKey);

            if (value == 0)
            {
                PlayerPrefs.SetInt(_rememberKey, 1);
                _rememberMe.fontStyle = FontStyle.BoldAndItalic;
                isRemember = true;
            }
            else
            {
                PlayerPrefs.SetInt(_rememberKey, 0);
                _rememberMe.fontStyle = FontStyle.Normal;
                isRemember = false;
            }
        }
        else
        {
            PlayerPrefs.SetInt(_rememberKey, 1);
            _rememberMe.fontStyle = FontStyle.BoldAndItalic;
            isRemember = true;
        }
    }

    void Login()
    {
        string email = _loginField.text;
        string password = _passField.text;

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            _result.text = "Enter login and password!";
            return;
        }

        Login(email, password);
    }

    private void Login(string email, string password)
    {
        var request = new LoginWithEmailAddressRequest
        {
            Email = email,
            Password = password
        };

        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnLoginFailure);
    }

    private void OnLoginSuccess(LoginResult result)
    {
        _result.text = "Login Success!";

        if (isRemember)
        {
            PlayerPrefs.SetString(_emailKey, _loginField.text);
            PlayerPrefs.SetString(_passwordKey, _passField.text);
            PlayerPrefs.Save();
        }
        else
        {
            PlayerPrefs.SetString(_emailKey, string.Empty);
            PlayerPrefs.SetString(_passwordKey, string.Empty);
            PlayerPrefs.Save();
        }

        ConfigModule.GetConfig<PlayFabConfig>().isConnectAndReady = true;

        Debug.Log("Login Success!");
    }

    private void OnLoginFailure(PlayFabError error)
    {
        Debug.LogError("Login Failed: " + error.GenerateErrorReport());

        if (error.ErrorDetails != null)
        {
            // Проверяем, есть ли ошибки в email и пароле
            if (error.ErrorDetails.ContainsKey("Email"))
            {
                Debug.Log("Неверный email. Попросите игрока ввести корректный email.");
                _result.text = "Invalid email!";
            }
            if (error.ErrorDetails.ContainsKey("Password"))
            {
                Debug.Log("Пароль слишком короткий. Запросите пароль от 6 символов."); 
                _result.text = "Invalid password!";
            }
        }

        // Если ошибка связана с отсутствием аккаунта - предлагаем регистрацию
        if (error.Error == PlayFabErrorCode.AccountNotFound)
        {
            Debug.Log("Аккаунт не найден. Предлагаем регистрацию.");
            _loginPanel.OpenRegisterScreen();
        }
    }

    public void OnDispose()
    {
        _loginButton.onClick.RemoveAllListeners();
        _rememberButton.onClick.RemoveAllListeners();
        _back.onClick.RemoveAllListeners();
    }
}

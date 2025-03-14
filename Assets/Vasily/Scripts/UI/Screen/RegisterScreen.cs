using System.Collections;
using System.Collections.Generic;

using PlayFab;
using PlayFab.ClientModels;

using UnityEngine;
using UnityEngine.UI;

public class RegisterScreen : MonoBehaviour
{
    private LoginPanel _loginPanel;

    [SerializeField] InputField _loginField;
    [SerializeField] InputField _emailField;
    [SerializeField] InputField _passField;

    [SerializeField] Button _loginButton;

    public void Init(LoginPanel logPanel)
    {
        _loginPanel = logPanel;
        _loginButton.onClick.AddListener(RegisterNewUser);
    }

    void RegisterNewUser()
    {
        var request = new RegisterPlayFabUserRequest
        {
            Email = _emailField.text,
            Password = _passField.text,
            Username = _loginField.text
        };

        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnRegisterFailure);
    }

    void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        Debug.Log("Register succsess!");
        // Автоматически логинимся после регистрации
        _loginPanel.OpenLoginScreen();
    }

    void OnRegisterFailure(PlayFabError error)
    {
        Debug.LogError("Ошибка регистрации: " + error.GenerateErrorReport());
    }

    public void OnDispose()
    {
        _loginButton.onClick.RemoveAllListeners();
    }
}

using System;

public class LoginPanel : SourcePanel
{
    private LoginScreen _loginScreen;
    private RegisterScreen _registerScreen;
    private GuestLoginScreen _guestScreen;

    public override void Init(SourceCanvas canvasParent)
    {
        _loginScreen = GetComponentInChildren<LoginScreen>();
        _registerScreen = GetComponentInChildren<RegisterScreen>();
        _guestScreen = GetComponentInChildren<GuestLoginScreen>();
        _loginScreen.Init(this); 
        _registerScreen.Init(this);
        _guestScreen.Init(this);

        gameObject.SetActive(false);

        base.Init(canvasParent);
    }

    public override void OnOpen(params Action[] onComplete)
    {
        OpenGuestScreen();
        base.OnOpen(onComplete);
    }


    public void OpenLoginScreen()
    {
        _registerScreen.gameObject.SetActive(false);
        _guestScreen.gameObject.SetActive(false);
        _loginScreen.gameObject.SetActive(true);
    }

    public void OpenRegisterScreen()
    {
        _loginScreen.gameObject.SetActive(false);
        _guestScreen.gameObject.SetActive(false);
        _registerScreen.gameObject.SetActive(true);
    }

    public void OpenGuestScreen()
    {
        _loginScreen.gameObject.SetActive(false);
        _registerScreen.gameObject.SetActive(false);
        _guestScreen.gameObject.SetActive(true);
    }

    public override void OnDipose()
    {
        base.OnDipose();

        _loginScreen.OnDispose();
        _guestScreen.OnDispose();
        _registerScreen.OnDispose();
    }
}

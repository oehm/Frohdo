using UnityEngine;
using System.Collections;

public class Login : MonoBehaviour
{
    public GUISkin style;
    public SceneDestroyer destroyer;

    float screenHeight;
    float screenWidth;

    string login, pass;

    // Use this for initialization
    void Start()
    {
        login = PlayerPrefs.GetString("LoginName");
        if (login == null) login = "";
        pass = "";
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnGUI()
    {
        screenHeight = ForceAspectRatio.screenHeight;
        screenWidth = ForceAspectRatio.screenWidth;

        style.customStyles[0].fixedWidth = screenWidth;
        style.customStyles[0].fixedHeight = screenHeight;

        style.textField.fixedWidth = screenWidth / 2;
        style.textField.fixedHeight = screenHeight / 10;
        style.textField.fontSize = (int)((screenHeight / 10) * 0.7);

        style.label.fontSize = (int)((screenHeight / 10) * 0.7);

        style.customStyles[1].fontSize = (int)((screenHeight / 10) * 0.7);
        style.customStyles[1].fixedWidth = screenWidth / 4;
        style.customStyles[1].fixedHeight = screenHeight / 8;

        style.customStyles[2].fontSize = (int)((screenHeight / 10) * 0.7);
        style.customStyles[2].fixedWidth = screenWidth / 2;
        style.customStyles[2].fixedHeight = screenHeight / 8;

        style.customStyles[3].fontSize = (int)((screenHeight / 10) * 0.7);
        style.customStyles[3].fixedWidth = screenWidth / 2;
        style.customStyles[3].fixedHeight = screenHeight / 8;

        style.button.fontSize = (int)((screenHeight / 10) * 0.7);
        style.button.fixedHeight = screenHeight / 10;
        style.button.fixedWidth = screenWidth / 4;

        GUI.skin = style;
        GUILayout.BeginArea(new Rect(ForceAspectRatio.xOffset + screenWidth / 4, ForceAspectRatio.yOffset, screenWidth / 2, screenHeight), "", style.customStyles[0]);

        switch (NetworkManager.Instance.GlobalStatus)
        {
            case NetworkManager.LoginStatus.LoggedOut:
                loggedoff();
                break;
            case NetworkManager.LoginStatus.LoggingIn:
                logginIn();
                break;
            case NetworkManager.LoginStatus.LoggedIn:
                loggedIn();
                break;
            case NetworkManager.LoginStatus.Reconnecting:
                reconnecting();
                break;
            case NetworkManager.LoginStatus.Refused:
                refused();
                break;
            case NetworkManager.LoginStatus.LoginIncorrect:
                LoginIcorr();
                break;
        }

        GUILayout.EndArea();
        GUI.skin = null;
    }

    private void LoginIcorr()
    {
        GUILayout.BeginVertical();
        GUILayout.FlexibleSpace();
        GUILayout.Label("Logindata incorrect!", "CenterAlignLabelDoubleWidth");
        if (GUILayout.Button("OK", "ButtonDoubleWidth"))
        {
            NetworkManager.Instance.LogOut();
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndVertical();
    }

    void loggedoff()
    {
        GUILayout.BeginVertical();
        GUILayout.FlexibleSpace();
        GUILayout.Label("Login:", style.label);
        login = GUILayout.TextField(login, style.textField);
        GUILayout.Label("Pass:", style.label);
        pass = GUILayout.PasswordField(pass, '*', style.textField);
        //GUILayout.Space(screenHeight / 10);
        GUILayout.FlexibleSpace();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Login"))
        {

            //if (!(login.Trim().Length == 0 || pass.Trim().Length == 0))
            {
                PlayerPrefs.SetString("LoginName", login.Trim());
                NetworkManager.Instance.tryConnect(login.Trim(), pass.Trim());
                Debug.Log("Try to log in: " + login + " _ " + pass);
            }
        }

        if (GUILayout.Button("Stay offline"))
        {
            SceneManager.Instance.returnFromLoginAndEscMenu();
        }
        GUILayout.EndHorizontal();

        drawmenubutton();

        GUILayout.FlexibleSpace();
        GUILayout.EndVertical();
    }

    void loggedIn()
    {
        GUILayout.BeginVertical();
        GUILayout.FlexibleSpace();
        GUILayout.Label("You are Logged In as: " + NetworkManager.Instance.User, "CenterAlignLabelDoubleWidth");
        GUILayout.FlexibleSpace();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Logout"))
        {
            NetworkManager.Instance.LogOut();
            pass = "";
        }

        if (GUILayout.Button("Continue online"))
        {
            SceneManager.Instance.returnFromLoginAndEscMenu();
        }
        GUILayout.EndHorizontal();

        drawmenubutton();

        GUILayout.FlexibleSpace();
        GUILayout.EndVertical();
    }

    void logginIn()
    {
        GUILayout.BeginVertical();
        GUILayout.FlexibleSpace();
        GUILayout.Label("Logging in. Please wait ...", "CenterAlignLabelDoubleWidth");
        GUILayout.FlexibleSpace();
        GUILayout.EndVertical();
    }

    void reconnecting()
    {

    }

    void refused()
    {

    }

    void drawmenubutton()
    {
        if (SceneManager.Instance.getSavedSceneTypeWhenInLogin() != SceneManager.Scene.MainMenu)
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Back to main menu", "ButtonDoubleWidth"))
            {
                SceneManager.Instance.loadScene(SceneManager.Scene.MainMenu);
            }
            GUILayout.EndHorizontal();
        }
    }
}

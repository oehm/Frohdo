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

        GUILayout.BeginArea(new Rect(ForceAspectRatio.xOffset + screenWidth / 4, ForceAspectRatio.yOffset, screenWidth / 2, screenHeight), "", style.customStyles[0]);

        switch (LoginManager.GlobalStatus)
        {
            case LoginManager.LoginStatus.LoggedOut:
                loggedoff();
                break;
            case LoginManager.LoginStatus.LoggingIn:
                logginIn();
                break;
            case LoginManager.LoginStatus.LoggedIn:
                loggedIn();
                break;
            case LoginManager.LoginStatus.Reconnecting:
                reconnecting();
                break;
            case LoginManager.LoginStatus.Refused:
                refused();
                break;
        }

        GUILayout.EndArea();
    }

    void loggedoff()
    {
        GUILayout.BeginVertical();
        GUILayout.FlexibleSpace();
        GUILayout.Label("Login:", style.label);
        login = GUILayout.TextField(login, style.textField);
        GUILayout.Label("Pass:", style.label);
        pass = GUILayout.PasswordField(pass, '*', style.textField);
        GUILayout.Space(screenHeight / 10);
        GUILayout.FlexibleSpace();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Login"))
        {

            //if (!(login.Trim().Length == 0 || pass.Trim().Length == 0))
            {
                PlayerPrefs.SetString("LoginName", login.Trim());
                LoginManager.Instance.tryConnect(login.Trim(), pass.Trim());
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

    }

    void logginIn()
    {

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
            if (GUILayout.Button("Back to main menu"))
            {
                SceneManager.Instance.loadScene(SceneManager.Scene.MainMenu);
            }
            GUILayout.EndHorizontal();
        }
    }
}

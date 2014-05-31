using UnityEngine;
using System.Collections;
using System.Xml.Serialization;
using System.IO;

public class Login : MonoBehaviour
{
    public GUISkin style;
    public SceneDestroyer destroyer;
    public Texture2D ColorHexagon;

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

        style.textField.fixedWidth = screenWidth/2.5f;
        style.textField.fixedHeight = screenHeight / 12;
        style.textField.fontSize = (int)((screenHeight / 12) * 0.7);

        style.label.fontSize = (int)((screenHeight / 12) * 0.7);

        //Center Align Label
        style.customStyles[1].fontSize = (int)((screenHeight / 12) * 0.7);
        style.customStyles[1].fixedWidth = screenWidth / 5;
        style.customStyles[1].fixedHeight = screenHeight / 8;

        //centerAlignDoubleWidth
        style.customStyles[2].fontSize = (int)((screenHeight / 12) * 0.7);
        style.customStyles[2].fixedWidth = screenWidth / 2.5f;
        style.customStyles[2].fixedHeight = screenHeight / 8;

        //double width button
        style.customStyles[3].fontSize = (int)((screenHeight / 12) * 0.7);
        style.customStyles[3].fixedWidth = screenWidth / 2.5f;
        style.customStyles[3].fixedHeight = screenHeight / 8;

        //fontsize for login-button ...
        style.button.fontSize = (int)((screenHeight / 12) * 0.7);
        style.button.fixedHeight = screenHeight / 10;
        style.button.fixedWidth = screenWidth / 5;

        //font for visit community button
        style.customStyles[5].fontSize = (int)((screenHeight / 12) * 0.6);
        style.customStyles[5].fixedWidth = screenWidth / 2.5f;
        style.customStyles[5].fixedHeight = screenHeight / 8;

        //halfbox to split screen into 2 halfs
        style.customStyles[4].fixedWidth = screenWidth / 2f;
        style.customStyles[4].fixedHeight = screenHeight;

        //box with color hexagon puicture
        style.box.fixedHeight = screenHeight;
        style.box.fixedWidth = screenWidth / 2;

        GUI.skin = style;
        GUILayout.BeginArea(new Rect(ForceAspectRatio.xOffset, ForceAspectRatio.yOffset, screenWidth, screenHeight), "", style.customStyles[0]);
            GUILayout.BeginHorizontal();
                GUILayout.BeginVertical("HalfBox");
                    GUILayout.FlexibleSpace();
                    GUILayout.BeginHorizontal();
                        GUILayout.FlexibleSpace();
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
                        GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                    GUILayout.FlexibleSpace();
                GUILayout.EndVertical();

                GUILayout.BeginVertical("HalfBox");
                    GUILayout.FlexibleSpace();
                        GUILayout.Box(ColorHexagon);
                    GUILayout.FlexibleSpace();
                GUILayout.EndVertical();
            GUILayout.EndHorizontal();
        GUILayout.EndArea();
        GUI.skin = null;
    }

    private void LoginIcorr()
    {
        GUILayout.BeginVertical();
        GUILayout.FlexibleSpace();
        GUILayout.Label("Logindata incorrect!", "CenterAlignLabelDoubleWidth");
        if (GUILayout.Button("OK", "ButtonDoubleWidth") || (Event.current.type == EventType.KeyUp && Event.current.keyCode == KeyCode.Return))
        {
            NetworkManager.Instance.LogOut();
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndVertical();
    }

    void loggedoff()
    {
        GUILayout.BeginVertical();
        if (GUILayout.Button("register: community.mediacube.at", "RegisterButton"))
        {
            Application.OpenURL("http://community.mediacube.at");
        }
        GUILayout.FlexibleSpace();
        GUILayout.Label("Login:", style.label);
        login = GUILayout.TextField(login, style.textField);
        GUILayout.Label("Pass:", style.label);
        pass = GUILayout.PasswordField(pass, '*', style.textField);
        GUILayout.FlexibleSpace();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Login") || (Event.current.type == EventType.KeyUp && Event.current.keyCode == KeyCode.Return))
        {

            //if (!(login.Trim().Length == 0 || pass.Trim().Length == 0))
            {
                PlayerPrefs.SetString("LoginName", login.Trim());
                NetworkManager.Instance.tryConnect(login.Trim(), pass.Trim());
                Debug.Log("Try to log in: " + login);
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

        if (GUILayout.Button("Continue online") || (Event.current.type == EventType.KeyUp && Event.current.keyCode == KeyCode.Return))
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
        GUILayout.BeginVertical();
        GUILayout.FlexibleSpace();
        GUILayout.Label("Unable to Login.\nMaybe you are not properly connected or there are \"Server issues\" (Nope!)", "CenterAlignLabelDoubleWidth");
        if (GUILayout.Button("back", "ButtonDoubleWidth") || (Event.current.type == EventType.KeyUp && Event.current.keyCode == KeyCode.Return))
        {
            NetworkManager.Instance.LogOut();
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndVertical();
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

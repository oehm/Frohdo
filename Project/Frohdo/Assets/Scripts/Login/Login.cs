using UnityEngine;
using System.Collections;

public class Login : MonoBehaviour
{


    public GUISkin style;
    public SceneDestroyer destroyer;

    string login, pass;

    enum LoginStatus { LoggedOut, LoggingIn, LoggedIn };

    // Use this for initialization
    void Start()
    {
        login = "";
        pass = "";
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnGUI()
    {
        float screenHeight = ForceAspectRatio.screenHeight;
        float screenWidth = ForceAspectRatio.screenWidth;

        style.customStyles[0].fixedWidth = screenWidth;
        style.customStyles[0].fixedHeight = screenHeight;

        style.textField.fixedWidth = screenWidth / 2;
        style.textField.fixedHeight = screenHeight / 10;
        style.textField.fontSize = (int)((screenHeight / 10) * 0.7);

        style.label.fontSize = (int)((screenHeight / 10) * 0.7);

        GUILayout.BeginArea(new Rect(ForceAspectRatio.xOffset + screenWidth / 4, ForceAspectRatio.yOffset, screenWidth / 2, screenHeight), "", style.customStyles[0]);

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
            if (!(login.Trim().Length == 0 || pass.Trim().Length == 0))
            {
                Debug.Log("Try to log in: " + login + " _ " + pass);
            }
        }

        if (GUILayout.Button("Stay offline"))
        {
            SceneManager.Instance.returnFromLoginAndEscMenu();
        }
        GUILayout.EndHorizontal();

        if (SceneManager.Instance.getSavedSceneTypeWhenInLogin() != SceneManager.Scene.MainMenu)
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Back to main menu"))
            {
                SceneManager.Instance.loadScene(SceneManager.Scene.MainMenu);
            }
            GUILayout.EndHorizontal();
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndVertical();

        GUILayout.EndArea();
    }
}

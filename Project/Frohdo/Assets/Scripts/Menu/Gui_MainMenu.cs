using UnityEngine;
using System.Collections;

public class Gui_MainMenu : MonoBehaviour
{
    private delegate void MenuDelegate();
    private MenuDelegate menuFunction;

    public SceneDestroyer destoryer;

    public GUISkin style;

    private GUIContent mainScreen;

    public Texture2D panda;

    float screenHeight;
    float screenWidth;
    
    void Start()
    {
        if (SceneManager.Instance.Startup) SceneManager.Instance.showLoginAndEscMenu();
        menuFunction = mainMenu;
    }

    void OnGUI()
    {
        screenHeight = ForceAspectRatio.screenHeight;
        screenWidth = ForceAspectRatio.screenWidth;

        style.customStyles[0].fixedWidth = screenWidth;
        style.customStyles[0].fixedHeight = screenHeight;

        style.textField.fixedWidth = screenWidth / 2.5f;
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
        style.button.fontSize = (int)((screenHeight / 8) * 0.7);
        style.button.fixedHeight = screenHeight / 10;
        style.button.fixedWidth = screenWidth / 5;
        style.button.margin.left = (int)screenHeight / 8;

        //halfbox to split screen into 2 halfs
        style.customStyles[4].fixedWidth = screenWidth / 2f;
        style.customStyles[4].fixedHeight = screenHeight;

        //box with idle animation right side!
        style.box.fixedHeight = screenHeight;
        style.box.fixedWidth = screenWidth / 2;

        menuFunction();
    }

    void mainMenu()
    {
        GUI.skin = style;
        GUILayout.BeginArea(new Rect(ForceAspectRatio.xOffset, ForceAspectRatio.yOffset, screenWidth, screenHeight), "", style.customStyles[0]);
        GUILayout.BeginHorizontal();
        GUILayout.BeginVertical("HalfBox");
        GUILayout.FlexibleSpace();
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.BeginVertical();
        GUILayout.FlexibleSpace();

        if (GUILayout.Button("Puke around", "ButtonDoubleWidth"))
        {
            SceneManager.Instance.loadScene(SceneManager.Scene.LevelSelect);
        }
        if (GUILayout.Button("Change puke things", "ButtonDoubleWidth"))
        {
            SceneManager.Instance.loadScene(SceneManager.Scene.Options);
        }
        if (GUILayout.Button("How to puke", "ButtonDoubleWidth"))
        {
            SceneManager.Instance.loadScene(SceneManager.Scene.Help);
        }
        if (GUILayout.Button("End puking", "ButtonDoubleWidth"))
        {
            Application.Quit();
        }

        GUILayout.FlexibleSpace();
        GUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.EndVertical();

        GUILayout.BeginVertical("HalfBox");
        GUILayout.FlexibleSpace();
        GUILayout.Box(panda);
        GUILayout.FlexibleSpace();
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
        GUILayout.EndArea();
        GUI.skin = null;
    }

}

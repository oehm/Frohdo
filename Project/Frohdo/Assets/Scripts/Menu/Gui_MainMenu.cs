using UnityEngine;
using System.Collections;

public class Gui_MainMenu : MonoBehaviour
{
    private delegate void MenuDelegate();
    private MenuDelegate menuFunction;

    public SceneDestroyer destoryer;

    public GUISkin mainStyle;

    private GUIContent mainScreen;

    private bool muted = false;

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
        menuFunction();
    }

    void mainMenu()
    {
        GUILayout.BeginArea(new Rect((ForceAspectRatio.screenWidth) / 2 - 300 + ForceAspectRatio.xOffset, (ForceAspectRatio.screenHeight) / 2 - 200 + ForceAspectRatio.yOffset, 600, 400));
        
        if (GUILayout.Button("GameScene", mainStyle.button))
        {
            SceneManager.Instance.loadScene(SceneManager.Scene.LevelSelect);
        }
        if (GUILayout.Button("LevelEditorScene", mainStyle.button))
        {
            SceneManager.Instance.loadScene(SceneManager.Scene.Editor);
        }
        if (GUILayout.Button("Options", mainStyle.button))
        {
            SceneManager.Instance.loadScene(SceneManager.Scene.Options);
        }
        if(GUILayout.Button("Help", mainStyle.button))
        {
            SceneManager.Instance.loadScene(SceneManager.Scene.Help);
        }
        if (GUILayout.Button("Quit", mainStyle.button))
        {
            Application.Quit();
        }
        GUILayout.EndArea();
    }

}

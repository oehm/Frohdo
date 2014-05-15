using UnityEngine;
using System.Collections;

public class Gui_MainMenu : MonoBehaviour
{
    private delegate void MenuDelegate();
    private MenuDelegate menuFunction;

    public SceneDestroyer destoryer;

    public GUISkin mainStyle;

    private GUIContent mainScreen;

    //options
    int quallity = 4;
    public GUIContent[] quallityOptions;

    void Start()
    {

        menuFunction = mainMenu;
    }

    void OnGUI()
    {
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
            menuFunction = options;
        }
        if (GUILayout.Button("Quit", mainStyle.button))
        {
            Application.Quit();
        }
        GUILayout.EndArea();
    }

    void options()
    {
        GUILayout.BeginArea(new Rect((ForceAspectRatio.screenWidth + ForceAspectRatio.xOffset) / 2 - 300, (ForceAspectRatio.screenHeight + ForceAspectRatio.yOffset) / 2 - 200, 600, 400));

        GUILayout.Label("QuallityLevel", mainStyle.label);
        quallity = GUILayout.Toolbar(quallity, quallityOptions,mainStyle.customStyles[0]);
        QualitySettings.SetQualityLevel(quallity);

        if (GUILayout.Button("Fullscreen ON/OFF", mainStyle.button))
        {
            Screen.fullScreen = !Screen.fullScreen;
        }

        if (GUILayout.Button("Back", mainStyle.button))
        {
            menuFunction = mainMenu;
        }
        GUILayout.EndArea();
    }
}

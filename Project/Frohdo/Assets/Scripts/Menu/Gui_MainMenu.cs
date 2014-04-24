using UnityEngine;
using System.Collections;

public class Gui_MainMenu : MonoBehaviour
{
    private delegate void MenuDelegate();
    private MenuDelegate menuFunction;

    public GUISkin mainStyle;

    private GUIContent mainScreen;

    //options
    int quallity = 5;
    public GUIContent[] quallityOptions;

    int resolutionopt = 2;
    int numberOfResolution;
    private GUIContent[] resolutions;
    // Use this for initialization
    void Start()
    {
        numberOfResolution = Screen.resolutions.Length;
        resolutions = new GUIContent[numberOfResolution];
        for (int i = 0; i < numberOfResolution; i++)
        {
            if (Screen.resolutions[i].width == Screen.width || Screen.resolutions[i].height == Screen.height)
            {
                resolutionopt = i;
            }
            resolutions[i] = new GUIContent(Screen.resolutions[i].width.ToString() + "x" + Screen.resolutions[i].height.ToString());
        }

        menuFunction = mainMenu;
    }

    void OnGUI()
    {
        menuFunction();
    }

    void mainMenu()
    {
        GUILayout.BeginArea(new Rect(Screen.width / 2 - 300, Screen.height / 2 - 200, 600, 400));
        if (GUILayout.Button("GameScene", mainStyle.button))
        {
            Application.LoadLevel(2);
        }
        if (GUILayout.Button("LevelEditorScene", mainStyle.button))
        {
            Application.LoadLevel(3);
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
        GUILayout.BeginArea(new Rect(Screen.width / 2 - 300, Screen.height / 2 - 200, 600, 400));

        GUILayout.Label("QuallityLevel", mainStyle.label);
        quallity = GUILayout.Toolbar(quallity, quallityOptions);
        QualitySettings.SetQualityLevel(quallity);

        GUILayout.Label("Resolution", mainStyle.label);
        int resolutionoptnew = GUILayout.SelectionGrid(resolutionopt, resolutions, 4);
        if (resolutionopt != resolutionoptnew)
        {
            resolutionopt = resolutionoptnew;
            Screen.SetResolution(Screen.resolutions[resolutionopt].width, Screen.resolutions[resolutionopt].height, Screen.fullScreen);
        }


        if (GUILayout.Button("Fullscreen/Windowed", mainStyle.button))
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

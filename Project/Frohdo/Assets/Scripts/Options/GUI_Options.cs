using UnityEngine;
using System.Collections;

public class GUI_Options : MonoBehaviour
{

    public GUISkin mainStyle;

    //options
    private Vector2 lastSize;
    int quallity = 4;
    public GUIContent[] quallityOptions;


    void Start()
    {
        lastSize = new Vector2(Screen.width, Screen.height);                
    }

    void OnGUI()
    {
        GUILayout.BeginArea(new Rect((ForceAspectRatio.screenWidth + ForceAspectRatio.xOffset) / 2 - 300, (ForceAspectRatio.screenHeight + ForceAspectRatio.yOffset) / 2 - 200, 600, 400));

        GUILayout.Label("QualityLevel", mainStyle.label);
        quallity = GUILayout.Toolbar(quallity, quallityOptions, mainStyle.customStyles[0]);
        QualitySettings.SetQualityLevel(quallity);

        if (GUILayout.Button("Fullscreen ON/OFF", mainStyle.button))
        {
            if(!Screen.fullScreen)
            {
                lastSize = new Vector2(Screen.width, Screen.height);                
                Screen.SetResolution(Screen.width, Screen.height, true);
            }
            else
            {
                Screen.SetResolution((int)lastSize.x, (int)lastSize.y, false);
            }
        }
        GUILayout.Label("Background volume", mainStyle.label);
        SoundController.Instance.BackgroundSoundVolume = GUILayout.HorizontalSlider(SoundController.Instance.BackgroundSoundVolume, 0, 1);
        GUILayout.Label("Sounds", mainStyle.label);
        SoundController.Instance.MiscSoundVolume = GUILayout.HorizontalSlider(SoundController.Instance.MiscSoundVolume, 0, 1);

        if (GUILayout.Button("Back", mainStyle.button))
        {
            SceneManager.Instance.loadScene(SceneManager.Scene.MainMenu);
        }
        GUILayout.EndArea();
    }
}

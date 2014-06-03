using UnityEngine;
using System.Collections;

public class GUI_Options : MonoBehaviour
{

    public GUISkin style;

    //options
    private Vector2 lastSize;
    int quallity = 4;
    public GUIContent[] quallityOptions;

    float screenHeight;
    float screenWidth;

    void Start()
    {
        lastSize = new Vector2(Screen.width, Screen.height);                
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
        style.customStyles[3].fixedWidth = screenWidth / 1.6f;
        style.customStyles[3].fixedHeight = screenHeight / 8;

        //fontsize for login-button ...
        style.button.fontSize = (int)((screenHeight / 12) * 0.7);
        style.button.fixedHeight = screenHeight / 10;
        style.button.fixedWidth = screenWidth / 8;
        //style.button.margin.left = (int)screenHeight / 8;

        //halfbox to split screen into 2 halfs
        style.customStyles[4].fixedWidth = screenWidth / 1.6f;
        style.customStyles[4].fixedHeight = screenHeight;

        //box with idle animation right side!
        style.box.fixedHeight = screenHeight;
        style.box.fixedWidth = screenWidth / 2;


        GUI.skin = style;
        GUILayout.BeginArea(new Rect(ForceAspectRatio.xOffset, ForceAspectRatio.yOffset, screenWidth, screenHeight), "", style.customStyles[0]);
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.BeginVertical("HalfBox");
        GUILayout.FlexibleSpace();

        GUILayout.Label("Quality:", style.label);
        quallity = GUILayout.Toolbar(quallity, quallityOptions);
        QualitySettings.SetQualityLevel(quallity);

        if (GUILayout.Button("Fullscreen ON/OFF", "ButtonDoubleWidth"))
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
        GUILayout.Label("Background volume", style.label);
        SoundController.Instance.BackgroundSoundVolume = GUILayout.HorizontalSlider(SoundController.Instance.BackgroundSoundVolume, 0, 1);
        GUILayout.Label("Sounds", style.label);
        SoundController.Instance.MiscSoundVolume = GUILayout.HorizontalSlider(SoundController.Instance.MiscSoundVolume, 0, 1, style.horizontalSlider, style.horizontalSliderThumb);

        if (GUILayout.Button("Back", "ButtonDoubleWidth"))
        {
            SceneManager.Instance.loadScene(SceneManager.Scene.MainMenu);
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.EndArea();
    }
}

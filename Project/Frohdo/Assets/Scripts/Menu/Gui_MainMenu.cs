using UnityEngine;
using System.Collections;

public class Gui_MainMenu : MonoBehaviour
{

    public GUISkin mainStyle;

    private GUIContent mainScreen;
    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(Screen.width / 2 - 300, Screen.height / 2 - 200, 600, 400));
        if (GUILayout.Button("GameScene", mainStyle.button))
        {
            Application.LoadLevel(1);
        }
        if (GUILayout.Button("LevelEditorScene", mainStyle.button))
        {
            Application.LoadLevel(2);
        }
        if (GUILayout.Button("Options", mainStyle.button))
        {

        }
        GUILayout.EndArea();
    }
}

using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour
{
    private delegate void MenuDelegate();
    private MenuDelegate menuFunction;

    private bool isPause = false;

    public SceneDestroyer destroyer;
    // Use this for initialization
    void Start()
    {
        menuFunction = pauseMenuMain;
    }

    void Update()
    {
        isPause = Input.GetAxis("Pause") > 0.0f;
    }
    
    void OnGUI()
    {
        if (isPause)
        {
            menuFunction();
        }
    }

    void pauseMenuMain()
    {
        GUILayout.BeginArea(new Rect(Screen.width / 2 - 300, Screen.height / 2 - 200, 600, 400));
        if (GUILayout.Button("Back to Main Menu"))
        {
            SceneManager.Instance.loadScene(destroyer, 2);
        }

        if (GUILayout.Button("Quit"))
        {
            Application.Quit();
        }
        GUILayout.EndArea();
    }
}

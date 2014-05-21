using UnityEngine;
using System.Collections;

public class LoginAndEscMenuManager : MonoBehaviour
{
    private bool isPause = false;
    // Use this for initialization
    void Start()
    {
    }

    void Update()
    {
        if (Input.GetAxis("Pause") > 0 && 
            (
            SceneManager.Instance.getCurrentSceneType() == SceneManager.Scene.MainMenu || 
            SceneManager.Instance.getCurrentSceneType() == SceneManager.Scene.Editor || 
            SceneManager.Instance.getCurrentSceneType() == SceneManager.Scene.LevelSelect || 
            SceneManager.Instance.getCurrentSceneType() == SceneManager.Scene.Game || 
            SceneManager.Instance.getCurrentSceneType() == SceneManager.Scene.RateScreen
            )
            )
        {
            isPause = !isPause;
            if (isPause)
            {
                SceneManager.Instance.showLoginAndEscMenu();
            }
            else
            {
                SceneManager.Instance.returnFromLoginAndEscMenu();
            }
        }
    }
}

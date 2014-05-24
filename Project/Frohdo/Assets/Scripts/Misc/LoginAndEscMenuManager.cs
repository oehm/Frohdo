using UnityEngine;
using System.Collections;

public class LoginAndEscMenuManager : MonoBehaviour
{

    private static LoginAndEscMenuManager instance = null;

    public static LoginAndEscMenuManager Instance
    {
        get
        {
            return instance;
        }
    }

    private bool _axisLocked = false;

    // Use this for initialization
    void Start()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        instance = this;
    }

    void Update()
    {
        if (!Input.GetButton("Pause")) _axisLocked = false;
        if (!_axisLocked && Input.GetButton("Pause") &&
            (
            SceneManager.Instance.getCurrentSceneType() == SceneManager.Scene.Login ||
            SceneManager.Instance.getCurrentSceneType() == SceneManager.Scene.MainMenu ||
            SceneManager.Instance.getCurrentSceneType() == SceneManager.Scene.Editor ||
            SceneManager.Instance.getCurrentSceneType() == SceneManager.Scene.LevelSelect ||
            SceneManager.Instance.getCurrentSceneType() == SceneManager.Scene.Game ||
            SceneManager.Instance.getCurrentSceneType() == SceneManager.Scene.RateScreen
            )
            )
        {
            _axisLocked = true;
            if (SceneManager.Instance.getCurrentSceneType() == SceneManager.Scene.Login)
            {
                SceneManager.Instance.returnFromLoginAndEscMenu();
            }
            else
            {
                SceneManager.Instance.showLoginAndEscMenu();
            }
        }
    }
}

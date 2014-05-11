using UnityEngine;
using System.Collections;

public class SceneController : MonoBehaviour
{

    private static SceneController instance = null;
    public static SceneController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = (SceneController)Resources.Load("ScriptableObjectInstances/SceneMager");
                DontDestroyOnLoad(instance);
            }

            return instance;
        }
    }

    public GameObject background;

    private bool loading = false;
    private AsyncOperation async;

    void Start()
    {
        Application.LoadLevelAdditive(1);
    }

    // Update is called once per frame
    void Update()
    {
        if(loading)
        {
            Debug.Log(async.progress);
            if(async.isDone)
            {
                loading = false;
            }
        }
    }

    public void loadScene(SceneDestroyer destroyer, int nextScene)
    {
        destroyer.suicide();        
        AsyncOperation async = Application.LoadLevelAdditiveAsync(nextScene);
        loading = true;
    }

    public void changeBackgoundColor(string color)
    {
        background.GetComponent<Renderer>().material.color = LevelObjectController.Instance.GetColor(color);
    }
}

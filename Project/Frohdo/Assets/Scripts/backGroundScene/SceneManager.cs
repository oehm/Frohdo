using UnityEngine;
using System.Collections;

public class SceneManager : MonoBehaviour
{

    private static SceneManager instance = null;
    public static SceneManager Instance
    {
        get
        {
            return instance;
        }
    }

    public GameObject background;

    private bool loading = false;
    private AsyncOperation async;

    public string levelToLoad;
    public string levelToEdit = null;

    public bool loadLevelToEdit { get; set; } 

    void Start()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        instance = this;
        loadLevelToEdit = false;
        Application.LoadLevelAdditive(1);
    }

    // Update is called once per frame
    void Update()
    {
        if(loading)
        {
            //Debug.Log(async.progress * 100.0f);
            if(async.isDone)
            {
                loading = false;
            }
        }
    }

    public void loadScene(SceneDestroyer destroyer, Scene nextScene)
    {
        async = Application.LoadLevelAdditiveAsync((int)nextScene);        
        destroyer.suicide();
        loading = true;
    }

    public enum Scene
    {
        Background,
        Patcher,
        MainMenu,
        Game,
        Editor,
        LevelSelect

    }
}

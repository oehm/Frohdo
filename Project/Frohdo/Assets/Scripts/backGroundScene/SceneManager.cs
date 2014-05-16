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

    public string levelToLoad { get; set; }
    public string levelToEdit { get; set; }

    public bool loadLevelToEdit { get; set; } 

    void Start()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        instance = this;

        levelToLoad = null;
        levelToEdit = null;
        loadLevelToEdit = false;

        Application.LoadLevelAdditive((int)Scene.Patcher);
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

    public void loadScene(Scene nextScene)
    {
        SceneDestroyer destroyer = GameObject.Find("SceneDestroyer").GetComponent<SceneDestroyer>();

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

using UnityEngine;
using System.Collections;
using System.IO;

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

    private Scene curscene;
    private Scene savedSceneWhenInLoginScene;

    public bool Startup { get { return startup; } set { startup = value; } }
    private bool startup = true;

    public LevelAndType levelToLoad { get; set; }
    public string levelToEdit { get; set; }

    public bool loadLevelToEdit { get; set; } 

    void Start()
    {
        if (instance != null)
        {
            Destroy(gameObject);
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
                PageFlipController.Instance.endFlip();
                loading = false;
            }
        }
    }

    public void loadScene(Scene nextScene)
    {
        SceneDestroyer destroyer = GameObject.Find("SceneDestroyer").GetComponent<SceneDestroyer>();
        
        GameObject loginSceneDestroyer = GameObject.Find("LoginSceneDestroyer"); //we need this if we are in login and Esc menu and we choose to go back to the menu for example!
        if (loginSceneDestroyer != null)
        {
            loginSceneDestroyer.GetComponent<SceneDestroyer>().suicide();
        }

        PageFlipController.Instance.startFlip();
        async = Application.LoadLevelAdditiveAsync((int)nextScene);
        destroyer.suicide();
        curscene = nextScene;
        savedSceneWhenInLoginScene = nextScene;
        loading = true;
        Resources.UnloadUnusedAssets();
        System.GC.Collect();
    }
    //Will return the Type of the Current Scene (if in Login Scene the Last scene will be returned!)
    public Scene getCurrentSceneType()
    {
        return curscene;
    }

    public Scene getSavedSceneTypeWhenInLogin()
    {
        return savedSceneWhenInLoginScene;
    }

    public void showLoginAndEscMenu()
    {
        if (startup) startup = false;
        SceneDisabler disabler = GameObject.Find("SceneDisabler").GetComponent<SceneDisabler>();
        curscene = Scene.Login;
        async = Application.LoadLevelAdditiveAsync((int)Scene.Login);
        disabler.disable();
    }

    public void returnFromLoginAndEscMenu() // we need this if we only want to close esc and login scene and continue with the last scene.
    {
        if (NetworkManager.Instance.GlobalStatus == NetworkManager.LoginStatus.LoggedIn ||
            NetworkManager.Instance.GlobalStatus == NetworkManager.LoginStatus.LoggedOut ||
            NetworkManager.Instance.GlobalStatus == NetworkManager.LoginStatus.Refused)
        {
            SceneDisabler disabler = GameObject.Find("SceneDisabler").GetComponent<SceneDisabler>();
            SceneDestroyer destroyer = GameObject.Find("LoginSceneDestroyer").GetComponent<SceneDestroyer>();
            destroyer.suicide();
            disabler.enable();
            curscene = savedSceneWhenInLoginScene;
            GameObject.FindObjectOfType<ForceAspectRatio>().activate();
        }
        Resources.UnloadUnusedAssets();
        System.GC.Collect();
    }

    public enum Scene
    {
        Background,
        Patcher,
        MainMenu,
        Game,
        Editor,
        LevelSelect,
        RateScreen,
        Login,
        Options,
        Help
    }
}

public class LevelAndType
{
    public string LeveltoLoad;
    public string LevelTitle;
    public string LevelDescription;
    public string thumbpath;
    public LevelLoader.LevelType type;

    public Texture2D currentThumb;

    public LevelAndType()
    {
        this.LeveltoLoad = "";
        this.LevelTitle = "";
        this.type = LevelLoader.LevelType.Custom;
        this.LevelDescription = "";
        this.thumbpath = "";
    }

    public LevelAndType(string LeveltoLoad, LevelLoader.LevelType type, Texture2D thumb = null)
    {
        this.LeveltoLoad = LeveltoLoad;
        this.type = type;
        this.LevelTitle = Path.GetFileNameWithoutExtension(LeveltoLoad);
        this.thumbpath = LeveltoLoad.Substring(0, LeveltoLoad.Length - 4) + "_thumb.png";
        this.LevelDescription = "";
        this.currentThumb = thumb;
    }
}

using UnityEngine;
using System.Collections;

public class BuildManager : MonoBehaviour
{


    private static BuildManager instance = null;

    public static BuildManager Instance
    {
        get
        {
            return instance;
        }
    }

    public enum BuildPlatform { Editor, Windows, Mac, Linux }
    private BuildPlatform curPlatform;
    private string dataPath;
    private string pathForPatcher;
    private string mapsPath;
    private string screensPath;

    public BuildPlatform CurPlatform { get { return curPlatform; } }
    public string DataPath { get { return dataPath; } }
    public string PathForPatcher { get { return pathForPatcher; } }
    public string MapsPath { get { return mapsPath; } }
    public string ScreenshotPath { get { return screensPath; } }
    // Use this for initialization
    void Start()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        instance = this;

        GlobalVars.Initialise();

        if (!PlayerPrefs.HasKey("QualityLevel"))
        {
            PlayerPrefs.SetInt("QualityLevel", 3);
        }

        QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("QualityLevel"));

        //#if UNITY_EDITOR
        //    curPlatform = BuildPlatform.Editor;
        //    dataPath = Application.dataPath;
        //    pathForPatcher = Application.dataPath + @"\..\";
        //#endif

        #if UNITY_STANDALONE_WIN
            curPlatform = BuildPlatform.Windows;
            dataPath = Application.dataPath + @"/";
            pathForPatcher = Application.dataPath + @"/../";
            mapsPath = dataPath + @"Levels/";
            screensPath = dataPath + @"Screenshots/";
        #endif

        #if UNITY_STANDALONE_OSX
            curPlatform = BuildPlatform.Mac;
            dataPath = Application.dataPath + @"/Data/";
            pathForPatcher = Application.dataPath + @"/";
            mapsPath = dataPath + @"../../Levels/";
            screensPath = dataPath + @"../../Screenshots/";
        #endif
    }

    // Update is called once per frame
    void Update()
    {

    }
}

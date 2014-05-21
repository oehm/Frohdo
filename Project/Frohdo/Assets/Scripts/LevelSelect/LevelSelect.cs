using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;

public class LevelSelect : MonoBehaviour {

	// Use this for initialization

    public int LevelsToShow = 10;
    public GUISkin style;
    public string buttonDisabledColorHexString = "#d35400";

    public SceneDestroyer destroyer;

    List<Levelobj> levels;

    int LevelMaxPages;
    int LevelsCurPage;

    int selectedLevelid;

    int regsiterToShow; //0 = own custom levels (custom folder); 1 = Top onlineLevels (online); 2 = Story-Levels (story levels); 3 = Playlist (Maps folder)
    bool reloadLevels;

	void Start () {
        regsiterToShow = 3;
        reloadLevels = true;
        selectedLevelid = -1;
	}
	
	// Update is called once per frame
	void Update () {
        if (reloadLevels)
        {
            selectedLevelid = -1;
            reloadLevels = false;
            switch (regsiterToShow)
            {
                case 0: loadCustomLevels(); break;
                case 1: loadOnlineLevelList(); break;
                case 2: loadLocalLevelList(); break;
                case 3: loadStoryLevels(); break;
            }
            LevelMaxPages = levels.Count / LevelsToShow + 1;
            LevelsCurPage = 0;
        }
	}

    void loadLocalLevelList()
    {
        levels = new List<Levelobj>();

        DirectoryInfo dinfo = new DirectoryInfo(Application.dataPath + @"\Levels\downloaded\");

        List<string> levelpaths = new List<string>();

        if (dinfo.Exists)
        {
            int id = 0;
            //Debug.Log(dinfo.GetDirectories().Length);
            foreach (DirectoryInfo d in dinfo.GetDirectories())
            {
                Levelobj obj;
                try
                {
                    obj = gameObject.AddComponent(typeof(LocalLevelObj)) as LocalLevelObj;
                    ((LocalLevelObj)obj).init(id, d);
                    id++;
                    levels.Add(obj);
                }
                catch (Exception e)
                {
                    GC.Collect();
                    Debug.Log(e);
                    continue;
                }
                Debug.Log(d.FullName + " added!");
            }
        }
        else
        {
            Debug.Log("FAIL!");
        }
    }

    void loadOnlineLevelList()
    {
        levels = new List<Levelobj>();

        for (int i = 0; i < 18; i++)
        {
            Levelobj obj;
            try
            {
                obj = gameObject.AddComponent(typeof(OnlineLevelObj)) as OnlineLevelObj;
                ((OnlineLevelObj)obj).init(i, "hash");
            }
            catch (Exception)
            {
                GC.Collect();
                continue;
            }
            levels.Add(obj);
        }
    }

    void loadCustomLevels()
    {
        levels = new List<Levelobj>();

        DirectoryInfo dinfo = new DirectoryInfo(Application.dataPath + @"\Levels\custom\");

        List<string> levelpaths = new List<string>();

        if (dinfo.Exists)
        {
            int id = 0;
            //Debug.Log(dinfo.GetDirectories().Length);
            foreach (DirectoryInfo d in dinfo.GetDirectories())
            {
                Levelobj obj;
                try
                {
                    obj = gameObject.AddComponent(typeof (CustomLevelObj)) as CustomLevelObj;
                    ((CustomLevelObj)obj).init(id, d);
                    id++;
                    levels.Add(obj);
                }
                catch (Exception e)
                {
                    GC.Collect();
                    Debug.Log(e);
                    continue;
                }
                //Debug.Log(d.FullName + " added!");
            }
        }
        else
        {
            Debug.Log("FAIL!");
        }
    }

    void loadStoryLevels()
    {
        levels = new List<Levelobj>();
    }

    void OnGUI()
    {
        //custom styles:
        // 0: infobox -- elem
        // 1: topbar -- elem
        // 2: topbarbutton --  button
        // 3: levelselectbox -- elem
        // 4: bottombar -- elem
        // 5: forwardbackwardbutton -- button
        // 6: bottombarCurLevelLabel -- label
        // 7: mainbox -- elem
        // 8: imagebox -- elem
        // 9: highscoreNumbersBox -- elem

        float screenHeight = ForceAspectRatio.screenHeight;
        float screenWidth = ForceAspectRatio.screenWidth;

        style.customStyles[0].fixedWidth = screenWidth / 2;
        style.customStyles[0].fixedHeight = screenHeight * 0.8f;

        style.customStyles[1].fixedWidth = screenWidth;
        style.customStyles[1].fixedHeight = screenHeight * 0.1f;

        style.customStyles[2].fixedHeight = screenHeight * 0.1f;
        style.customStyles[2].fixedWidth = screenWidth / 6;
        style.customStyles[2].fontSize = (int)(screenHeight * 0.065f);

        style.customStyles[3].fixedWidth = screenWidth / 2;
        style.customStyles[3].fixedHeight = screenHeight * 0.8f;

        style.customStyles[4].fixedWidth = screenWidth / 2;
        style.customStyles[4].fixedHeight = screenHeight * 0.1f;

        style.customStyles[5].fixedWidth = screenWidth / 6;
        style.customStyles[5].fixedHeight = screenHeight * 0.1f;
        style.customStyles[5].fontSize = (int)(screenHeight * 0.065f);

        style.customStyles[6].fixedWidth = screenWidth / 10;
        style.customStyles[6].fixedHeight = screenHeight * 0.1f;
        style.customStyles[6].fontSize = (int)(screenHeight * 0.065f);

        style.customStyles[7].fixedWidth = screenWidth;
        style.customStyles[7].fixedHeight = screenHeight;

        style.customStyles[8].fixedWidth = screenWidth / 2.5f;
        style.customStyles[8].fixedHeight = screenHeight / 2.5f;

        style.customStyles[9].fixedWidth = screenWidth / 6;

        style.button.fixedWidth = screenWidth / 2;
        style.button.fixedHeight = screenHeight * 0.8f / LevelsToShow;
        style.button.fontSize = (int)((screenHeight * 0.8f / LevelsToShow) * 0.7f);

        style.label.fontSize = (int)((screenHeight * 0.8f / LevelsToShow) * 0.7f);

        GUI.skin = style;
        GUILayout.BeginVertical();
            GUILayout.BeginArea(new Rect(ForceAspectRatio.xOffset, ForceAspectRatio.yOffset, screenWidth, screenHeight), "", style.customStyles[7]);
                GUILayout.BeginHorizontal("", "topbar");
                    GUILayout.FlexibleSpace();
                    if (regsiterToShow == 3)
                    {
                        GUI.enabled = false;
                        GUILayout.Button("<color=" + buttonDisabledColorHexString + ">Story</color>", "topbarbutton");
                        GUI.enabled = true;
                    }
                    else
                    {
                        if (GUILayout.Button("Story", "topbarbutton"))
                        {
                            regsiterToShow = 3;
                            reloadLevels = true;
                        }
                    }

                    if (regsiterToShow == 2)
                    {
                        GUI.enabled = false;
                        GUILayout.Button("<color=" + buttonDisabledColorHexString + ">Playlist</color>", "topbarbutton");
                        GUI.enabled = true;
                    }
                    else
                    {
                        if (GUILayout.Button("Playlist", "topbarbutton"))
                        {
                            regsiterToShow = 2;
                            reloadLevels = true;
                        }
                    }

                    if (regsiterToShow == 0)
                    {
                        GUI.enabled = false;
                        GUILayout.Button("<color=" + buttonDisabledColorHexString + ">Custom</color>", "topbarbutton");
                        GUI.enabled = true;
                    }
                    else
                    {
                        if (GUILayout.Button("Custom", "topbarbutton"))
                        {
                            regsiterToShow = 0;
                            reloadLevels = true;
                        }
                    }

                    if (regsiterToShow == 1)
                    {
                        GUI.enabled = false;
                        GUILayout.Button("<color=" + buttonDisabledColorHexString + ">Top Online</color>", "topbarbutton");
                        GUI.enabled = true;
                    }
                    else
                    {
                        if (GUILayout.Button("Top Online", "topbarbutton"))
                        {
                            regsiterToShow = 1;
                            reloadLevels = true;
                        }
                    }
                    GUILayout.FlexibleSpace();

                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();

                    GUILayout.BeginVertical("box");
                        GUILayout.BeginVertical("levelselectbox");
                        if (levels != null && levels.Count != 0)
                        {
                            for (int i = LevelsToShow * LevelsCurPage; i < levels.Count; i++)
                            {
                                if (selectedLevelid == levels[i].id)
                                {
                                    GUI.enabled = false;
                                    GUILayout.Button("<color=" + buttonDisabledColorHexString + ">" + levels[i].name + "</color>", "button");
                                    GUI.enabled = true;
                                }
                                else
                                {
                                    if (GUILayout.Button(levels[i].name, "button"))
                                    {
                                        selectedLevelid = levels[i].id;
                                        //Environment.SetEnvironmentVariable("SelectedLevel", levels[i].XMLPath.FullName, EnvironmentVariableTarget.Process);

                                    }
                                }
                                if (i == LevelsToShow * LevelsCurPage + LevelsToShow - 1) break;
                            }
                        }
                        else
                        {
                            GUILayout.Label("");
                        }

                        GUILayout.EndVertical();

                        GUILayout.BeginHorizontal("bottombar");

                        GUILayout.FlexibleSpace();

                        if (LevelsCurPage == 0)
                        {
                            GUI.enabled = false;
                            GUILayout.Button("", "forwardbackwardbutton");
                            GUI.enabled = true;
                        }
                        else
                        {
                            if (GUILayout.Button("< Page " + LevelsCurPage, "forwardbackwardbutton"))
                            {
                                LevelsCurPage -= 1;
                            }
                        }
                        GUILayout.Label("<color=" + buttonDisabledColorHexString + ">" + (LevelsCurPage + 1).ToString() + "</color>", "bottombarCurLevelLabel");
                        if (LevelsCurPage == LevelMaxPages - 1)
                        {
                            GUI.enabled = false;
                            GUILayout.Button("", "forwardbackwardbutton");
                            GUI.enabled = true;
                        }
                        else
                        {
                            if (GUILayout.Button("Page: " + (LevelsCurPage + 2) + " >", "forwardbackwardbutton"))
                            {
                                LevelsCurPage += 1;
                            }
                        }
                        GUILayout.FlexibleSpace();
                    
                        GUILayout.EndHorizontal();

                    GUILayout.EndVertical();

                    GUILayout.BeginVertical("box");
                        GUILayout.BeginVertical("", "infoBox");
                
                        if(selectedLevelid != -1){

                            levels[selectedLevelid].LevelInfoGui();

                            //if (levels[selectedLevelid].GetType() == typeof(CustomLevelObj))
                            //{
                            //    GUILayout.Label("EIN CUSTOM LEVEL! - LAUFZEIT TYP-PRÜFUNG!");
                            //    if (GUILayout.Button("Spielen"))
                            //    {
                            //        SceneManager.Instance.loadScene(destroyer, 3);
                            //    }
                            //}
                            //if (levels[selectedLevelid].GetType() == typeof(OnlineLevelObj))
                            //    GUILayout.Label("EIN ONLINE LEVEL! - LAUFZEIT TYP-PRÜFUNG!");
                            //if (levels[selectedLevelid].GetType() == typeof(StoryLevelObj))
                            //    GUILayout.Label("EIN STORY LEVEL! - LAUFZEIT TYP-PRÜFUNG!");
                            //if (levels[selectedLevelid].GetType() == typeof(LocalLevelObj))
                            //    GUILayout.Label("EIN LOKAL GESPEICHERTES LEVEL! - LAUFZEIT TYP-PRÜFUNG!");
                            //GUILayout.Label("ID: " + levels[selectedLevelid].id);
                        }
                        else
                        {
                            GUILayout.EndVertical();
                        }

                    GUILayout.EndVertical();
                GUILayout.EndHorizontal();
            GUILayout.EndArea();
        GUILayout.EndVertical();
    }
}

abstract class Levelobj : MonoBehaviour
{
    public int id;
    public string name;

    public Texture2D thumbnail;

    public abstract void loadHighScores();

    public FileInfo XMLPath = null;

    protected bool StartLoadingThumb = true;
    protected bool ThumbCurrentlyLoading = false;

    protected WWW thumbdownload;
    const float downloadTimeout = 3.0f;
    float currentDownloadTime = 0.0f;

    public List<highscore> highscores;//only playlist and online!

    protected void searchLocalLevel(DirectoryInfo levelpath)
    {
        foreach (FileInfo f in levelpath.GetFiles())
        {
            if (f.Extension.Equals(".xml"))
            {
                if (XMLPath != null) throw new Exception("More than one XML-File found in Level-Directory!");
                else XMLPath = f;
            }
        }
        if (XMLPath == null)
        {
            throw new Exception("Level XML not found in Level Directory");
        }
    }

    protected void loadThumbnail(string path)
    {
        if (StartLoadingThumb)
        {
            //Debug.Log("Starting Coroutine with path" + path);
            StartCoroutine(FetchThumb(path));
            StartLoadingThumb = false;
        }
        currentDownloadTime += Time.deltaTime;
        if (currentDownloadTime >= downloadTimeout && ThumbCurrentlyLoading)
        {
            StopAllCoroutines();
            Debug.Log("ABORTED!");
        } 
    }

    IEnumerator FetchThumb(string path)
    {
        //Debug.Log("In COROUTINE!");
        ThumbCurrentlyLoading = true;
        currentDownloadTime = 0.0f;
        thumbdownload = new WWW(path);
        yield return thumbdownload;
        ThumbCurrentlyLoading = false;
        if (thumbdownload.error == null)
        {
            thumbnail = thumbdownload.texture;
        }
        else
        {
            Debug.Log(thumbdownload.error);
        }
        //Debug.Log("FINISHED!");
    }

    protected void showHighscore()
    {
        loadHighScores();
        GUILayout.BeginVertical("box");
            GUILayout.BeginHorizontal();
                GUILayout.Label("Highscores:");
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();

                GUILayout.BeginVertical("highscoreNumbersBox");
                    GameObject.Find("GUI_LevelSelect").GetComponent<LevelSelect>().style.label.alignment = TextAnchor.MiddleRight;
                    for (int i = 0; i < 5; i++)
                    {
                        GUILayout.BeginVertical();
                        GUILayout.Label("#" + (i + 1) + ": ");
                        GUILayout.EndVertical();
                    }
                    GameObject.Find("GUI_LevelSelect").GetComponent<LevelSelect>().style.label.alignment = TextAnchor.MiddleLeft;
                GUILayout.EndVertical();
                GUILayout.BeginVertical("box");
                for (int i = 0; i < 5; i++)
                {
                    GUILayout.BeginVertical();
                    if (highscores != null && i < highscores.Count)
                    {
                        GUILayout.Label(highscores[i].score + " - " + highscores[i].user);
                    }
                    else
                    {
                        GUILayout.Label(" ###");
                    }
                    GUILayout.EndVertical();
                }
                GUILayout.EndVertical();
                GameObject.Find("GUI_LevelSelect").GetComponent<LevelSelect>().style.label.alignment = TextAnchor.MiddleCenter;
            GUILayout.EndHorizontal();
        GUILayout.EndVertical();
    }

    protected void showThumbnail()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label(name);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (thumbnail != null)
        {
            GUILayout.FlexibleSpace();
            GUILayout.Box(thumbnail, "imagebox");
            GUILayout.FlexibleSpace();
        }
        else
        {
            GUILayout.FlexibleSpace();
            GUILayout.BeginHorizontal("imagebox");
            if (ThumbCurrentlyLoading) GUILayout.Label("Loading thumbnail ...");
            else GUILayout.Label("Unable to Load thumbnail!");
            GUILayout.EndHorizontal();
            GUILayout.FlexibleSpace();
        }
        GUILayout.EndHorizontal();
    }

    public abstract void LevelInfoGui();
}

class StoryLevelObj : Levelobj
{
    public void init(int id, DirectoryInfo levelpath)
    {
        this.id = id;
        this.name = levelpath.Name;
        searchLocalLevel(levelpath);
    }
    public override void loadHighScores()
    {
        //throw new NotImplementedException();
    }

    public override void LevelInfoGui()
    {
        if (StartLoadingThumb) loadThumbnail("file://" + XMLPath.FullName.Substring(0, XMLPath.FullName.Length - 4) + "_thumb.png");
        showThumbnail();
    }
}
class CustomLevelObj : Levelobj
{
    public void init(int id, DirectoryInfo levelpath)
    {
        this.id = id;
        this.name = levelpath.Name;
        searchLocalLevel(levelpath);
    }

    public override void loadHighScores()
    {
        //throw new NotImplementedException();
    }

    public override void LevelInfoGui()
    {
        if (StartLoadingThumb) loadThumbnail("file://" + XMLPath.FullName.Substring(0, XMLPath.FullName.Length - 4) + "_thumb.png");
        showThumbnail();
        showHighscore();
        GUILayout.EndVertical();
        GUILayout.BeginHorizontal("bottombar");
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Spielen", "forwardbackwardbutton"))
        {
            SceneManager.Instance.levelToLoad = XMLPath.FullName;
            SceneManager.Instance.loadScene(SceneManager.Scene.Game);
        }
        if (GUILayout.Button("Bearbeiten", "forwardbackwardbutton"))
        {
            SceneManager.Instance.levelToLoad = XMLPath.FullName;
            SceneManager.Instance.loadLevelToEdit = true;
            SceneManager.Instance.loadScene(SceneManager.Scene.Editor);
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
    }
}

class LocalLevelObj : Levelobj //already on disk
{
    public void init(int id, DirectoryInfo levelpath)
    {
        this.id = id;
        this.name = levelpath.Name;
        searchLocalLevel(levelpath);
    }

    public override void loadHighScores()
    {
        //throw new NotImplementedException();
    }

    public override void LevelInfoGui()
    {
        if (StartLoadingThumb) loadThumbnail("file://" + XMLPath.FullName.Substring(0, XMLPath.FullName.Length - 4) + "_thumb.png");
        showThumbnail();
        showHighscore();
        GUILayout.EndVertical();
        GUILayout.BeginHorizontal("bottombar");
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Level starten", "forwardbackwardbutton"))
        {
            SceneManager.Instance.levelToLoad = XMLPath.FullName;
            SceneManager.Instance.loadScene(SceneManager.Scene.Game);
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
    }
}

class OnlineLevelObj : Levelobj //online - not downloaded
{
    String hash = null;

    public void init(int id, string onlinehash)
    {
        this.id = id;
        this.name = "Online-Level " + id;
        this.hash = onlinehash;
    }

    public override void loadHighScores()
    {
        //throw new NotImplementedException();
    }

    public override void LevelInfoGui()
    {
        if (StartLoadingThumb && XMLPath != null) loadThumbnail(XMLPath.FullName.Substring(0, XMLPath.FullName.Length - 4) + "_thumb.png");
        showThumbnail();
        showHighscore();
    }
}

class highscore
{
    public string user;
    public float score;

    public highscore(string user, float score){
        this.user = user;
        this.score = score;
    }
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Xml;

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
            reloadLevels = false;
            if (DownloadPlaylistScriptManager.Instance.GlobalStatus == DownloadPlaylistScriptManager.DownloadStatus.Downloading)
            {
                loadLocalLevelList();
            }
            if(DownloadTopOnlineLevelListManager.Instance.GlobalStatus == DownloadTopOnlineLevelListManager.DownloadStatus.Downloading)
            {
                loadOnlineLevelList();
            }

            if (DownloadTopOnlineLevelListManager.Instance.GlobalStatus != DownloadTopOnlineLevelListManager.DownloadStatus.Downloading &&
                DownloadPlaylistScriptManager.Instance.GlobalStatus != DownloadPlaylistScriptManager.DownloadStatus.Downloading)
            {
                selectedLevelid = -1;
                switch (regsiterToShow)
                {
                    case 0: loadCustomLevels(); break;
                    case 1: loadOnlineLevelList(); break;
                    case 2: loadLocalLevelList(); break;
                    case 3: loadStoryLevels(); break;
                }
                LevelMaxPages = (levels.Count - 1) / LevelsToShow + 1;
                LevelsCurPage = 0;
            }
        }
	}

    void loadLocalLevelList()
    {
        levels = new List<Levelobj>();
        int id = 0;
        if (NetworkManager.Instance.GlobalStatus == NetworkManager.LoginStatus.LoggedIn)
        {
            switch (DownloadPlaylistScriptManager.Instance.GlobalStatus)
            {
                case DownloadPlaylistScriptManager.DownloadStatus.Downloaded:

                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(DownloadPlaylistScriptManager.Instance.XML);

                    //Debug.Log(DownloadPlaylistScriptManager.Instance.XML);

                    DownloadPlaylistScriptManager.Instance.reset();

                    foreach (XmlNode n in doc.GetElementsByTagName("playlist"))
                    {
                        Levelobj obj;
                        string hash = "";
                        string name = "";
                        string thumburl = "";
                        string dlUrl = "";
                        string creatorNickname = "";
                        int onlineid = -1;
                        bool currentUserIsCreator = false;
                        bool inplaylist = false;

                        foreach (XmlNode cn in n.ChildNodes)
                        {
                            if (cn.Name.Equals("hashCode")) hash = cn.InnerText;
                            if (cn.Name.Equals("title")) name = cn.InnerText;
                            if (cn.Name.Equals("urlThumbnail")) thumburl = cn.InnerText;
                            if (cn.Name.Equals("urlXML")) dlUrl = cn.InnerText;
                            if (cn.Name.Equals("levelId")) onlineid = Convert.ToInt32(cn.InnerText);
                            if (cn.Name.Equals("creatorNick")) creatorNickname = cn.InnerText;
                            if (cn.Name.Equals("currentUserIsCreator")) currentUserIsCreator = Convert.ToBoolean(cn.InnerText);
                            if (cn.Name.Equals("levelIsInPlaylist")) inplaylist = Convert.ToBoolean(cn.InnerText);
                            //check if in playlist
                        }
                        try
                        {
                            obj = gameObject.AddComponent(typeof(OnlineLevelObj)) as OnlineLevelObj;
                            ((OnlineLevelObj)obj).init(id, onlineid, hash, name, thumburl, dlUrl, inplaylist, currentUserIsCreator, creatorNickname);
                            id++;
                        }
                        catch (Exception)
                        {
                            GC.Collect();
                            continue;
                        }
                        levels.Add(obj);
                        //Debug.Log("add online Level: " + obj.name);
                    }
                    break;

                case DownloadPlaylistScriptManager.DownloadStatus.Downloading:
                    reloadLevels = true;
                    break;

                case DownloadPlaylistScriptManager.DownloadStatus.NotDownloaded:
                    reloadLevels = true;
                    DownloadPlaylistScriptManager.Instance.startLoadingLevelList();
                    break;
            }
        }

        if (reloadLevels) return;

        DirectoryInfo dinfo = new DirectoryInfo(Application.dataPath + @"\Levels\downloaded\");

        if (dinfo.Exists)
        {
            //Debug.Log(dinfo.GetDirectories().Length);
            foreach (DirectoryInfo d in dinfo.GetDirectories())
            {
                Levelobj obj;
                try
                {
                    obj = gameObject.AddComponent(typeof(LocalLevelObj)) as LocalLevelObj;
                    ((LocalLevelObj)obj).init(id, d);
                    id++;

                    OnlineLevelObj toreplace = null;

                    foreach (Levelobj o in levels)
                    {
                        if (o.GetType() == typeof(OnlineLevelObj))
                        {
                            if (((OnlineLevelObj)o).hash == ((LocalLevelObj)obj).hash)
                            {
                                toreplace = (OnlineLevelObj)o;
                                break;
                            }
                        }
                    }

                    if (toreplace != null)
                    {
                        levels.Remove(toreplace);
                    }

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
            //now we have to renew the id´s because some ids got deleted (if an playlist level was already downloaded!)

            id = 0;

            foreach (Levelobj o in levels)
            {
                o.id = id;
                id++;
            }
        }
        else
        {
            Debug.Log("FAIL!");
        }
        //Debug.Log(levels.Count);
    }

    void loadOnlineLevelList()
    {
        levels = new List<Levelobj>();
        switch (DownloadTopOnlineLevelListManager.Instance.GlobalStatus)
        {
            case DownloadTopOnlineLevelListManager.DownloadStatus.Downloaded:
                //Debug.Log("HERE!");

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(DownloadTopOnlineLevelListManager.Instance.XML);

                //Debug.Log(DownloadTopOnlineLevelListManager.Instance.XML);

                DownloadTopOnlineLevelListManager.Instance.reset();

                int id = 0;
                foreach (XmlNode n in doc.GetElementsByTagName("level"))
                {
                    Levelobj obj;
                    string hash = "";
                    string name = "";
                    string thumburl = "";
                    string dlUrl = "";
                    string creatorNickname = "";
                    int onlineid = -1;
                    bool currentUserIsCreator = false;
                    bool inplaylist = false;

                    foreach (XmlNode cn in n.ChildNodes)
                    {
                        if (cn.Name.Equals("hash")) hash = cn.InnerText;
                        if (cn.Name.Equals("title")) name = cn.InnerText;
                        if (cn.Name.Equals("urlThumbnail")) thumburl = cn.InnerText;
                        if (cn.Name.Equals("urlXML")) dlUrl = cn.InnerText;
                        if (cn.Name.Equals("id")) onlineid = Convert.ToInt32(cn.InnerText);
                        if (cn.Name.Equals("creatorNick")) creatorNickname = cn.InnerText;
                        if (cn.Name.Equals("currentUserIsCreator")) currentUserIsCreator = Convert.ToBoolean(cn.InnerText);
                        if (cn.Name.Equals("levelIsInPlaylist")) inplaylist = Convert.ToBoolean(cn.InnerText);
                        //check if in playlist
                    }
                    try
                    {
                        obj = gameObject.AddComponent(typeof(OnlineLevelObj)) as OnlineLevelObj;
                        ((OnlineLevelObj)obj).init(id, onlineid, hash, name, thumburl, dlUrl, inplaylist, currentUserIsCreator, creatorNickname);
                        id++;
                    }
                    catch (Exception)
                    {
                        GC.Collect();
                        continue;
                    }
                    levels.Add(obj);
                }
                break;

            case DownloadTopOnlineLevelListManager.DownloadStatus.Downloading:
                reloadLevels = true;
                break;

            case DownloadTopOnlineLevelListManager.DownloadStatus.NotDownloaded:
                reloadLevels = true;
                DownloadTopOnlineLevelListManager.Instance.startLoadingLevelList();
                break;
        }
    }

    void loadCustomLevels()
    {
        levels = new List<Levelobj>();

        DirectoryInfo dinfo = new DirectoryInfo(Application.dataPath + @"\Levels\custom\");

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

        //----- This is how Story Levels must be added! -----
        StoryLevelObj level1 = gameObject.AddComponent(typeof(StoryLevelObj)) as StoryLevelObj;
        //-------------------id------------------Path in Resources--------------------------------- (id from 0!! without holes)
        level1.initStoryLevel(0, "Maps/BuiltIn/The very first beginning/The very first beginning"); //WITHOUT EXTENSION XML!!!!
        levels.Add(level1);
        //---------------------------------------------------

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

        style.customStyles[6].fixedWidth = screenWidth / 2;
        style.customStyles[6].fixedHeight = screenHeight * 0.1f;
        style.customStyles[6].fontSize = (int)(screenHeight * 0.065f);

        style.customStyles[7].fixedWidth = screenWidth / 10;
        style.customStyles[7].fixedHeight = screenHeight * 0.1f;
        style.customStyles[7].fontSize = (int)(screenHeight * 0.065f);

        style.customStyles[8].fixedWidth = screenWidth;
        style.customStyles[8].fixedHeight = screenHeight;

        style.customStyles[9].fixedWidth = screenWidth / 2.5f;
        style.customStyles[9].fixedHeight = screenHeight / 2.5f;

        style.customStyles[10].fixedWidth = screenWidth / 6;

        style.customStyles[11].fixedWidth = screenWidth / 2;
        style.customStyles[11].fixedHeight = screenHeight * 0.4f;

        style.customStyles[12].fixedWidth = screenWidth / 2;
        style.customStyles[12].fixedHeight = screenHeight * 0.5f;

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
                        if (NetworkManager.Instance.GlobalStatus != NetworkManager.LoginStatus.LoggedIn)
                        {
                            regsiterToShow = 2;
                        }
                        else
                        {
                            GUI.enabled = false;
                            GUILayout.Button("<color=" + buttonDisabledColorHexString + ">Top Online</color>", "topbarbutton");
                            GUI.enabled = true;
                        }
                    }
                    else
                    {
                        if (NetworkManager.Instance.GlobalStatus != NetworkManager.LoginStatus.LoggedIn) GUI.enabled = false;
                        if (GUILayout.Button("Top Online", "topbarbutton"))
                        {
                            regsiterToShow = 1;
                            reloadLevels = true;
                        }
                        GUI.enabled = true;
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
                                    if (levels[i].GetType() == typeof(OnlineLevelObj)) GUILayout.Button("<color=" + buttonDisabledColorHexString + ">"+ levels[i].name + "-" + ((OnlineLevelObj)levels[i]).creatorNickname + "</color>", "button");
                                    else GUILayout.Button("<color=" + buttonDisabledColorHexString + ">" + levels[i].name + "</color>", "button");
                                    GUI.enabled = true;
                                }
                                else
                                {
                                    if (levels[i].GetType() == typeof(OnlineLevelObj))
                                    {
                                        if (GUILayout.Button(levels[i].name + "-" + ((OnlineLevelObj)levels[i]).creatorNickname, "button"))
                                        {
                                            selectedLevelid = levels[i].id;
                                        }
                                    }
                                    else
                                    {
                                        if (GUILayout.Button(levels[i].name, "button"))
                                        {
                                            selectedLevelid = levels[i].id;
                                        }
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
    public new string name;

    public Texture2D thumbnail;

    public abstract void loadHighScores();

    public FileInfo XMLPath = null;

    protected bool StartLoadingThumb = true;
    protected bool ThumbCurrentlyLoading = false;

    protected WWW thumbdownload;
    const float downloadTimeout = 3.0f;
    float currentDownloadTime = 0.0f;

    public List<highscore> highscores;//only playlist and online!

    protected int localPukeHighscore;
    protected int localTimeHighscore;

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
            StartLoadingThumb = false;
            if (this.GetType() != typeof(StoryLevelObj)) {
                StartCoroutine(FetchThumb(path));
            }
            else
            {
                //Debug.Log("Loading thumb from res: " + path);
                thumbnail = (Texture2D)Resources.Load(path, typeof(Texture2D));
                
            }
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
        //Debug.Log(path);
        thumbdownload = new WWW(path);
        yield return thumbdownload;
        ThumbCurrentlyLoading = false;
        if (thumbdownload.error == null)
        {
            thumbnail = thumbdownload.texture;
        }
        else
        {
            Debug.Log(path);
            Debug.Log(thumbdownload.error);
        }
        //Debug.Log("FINISHED!");
    }

    protected void showHighscore()
    {
        bool onlinehighscores = (this.GetType() == typeof(OnlineLevelObj) || this.GetType() == typeof(LocalLevelObj)) && NetworkManager.Instance.GlobalStatus == NetworkManager.LoginStatus.LoggedIn;
        GUILayout.BeginVertical("highscoreBox");
        if (onlinehighscores)
        {
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
                    if (onlinehighscores)
                    {
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
                    }
                GUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }
        else
        {
            GUILayout.FlexibleSpace();

            GUILayout.BeginHorizontal();

                GUILayout.BeginVertical();
                if (this.GetType() == typeof(StoryLevelObj))
                {
                    GUILayout.Label("Highscores not available on Story Levels");
                }
                if (this.GetType() == typeof(CustomLevelObj))
                {
                    GUILayout.Label("Highscores not available on Custom Levels");
                }
                else if (this.GetType() == typeof(LocalLevelObj))
                {
                    LocalLevelObj obj = (LocalLevelObj)this;
                    obj.loadLocalHighScores();
                    GUILayout.Label("Offline Highscores:");
                    GUILayout.Label("Time:  " + obj.localTimeHighscore);
                    GUILayout.Label("Pukes: " + obj.localPukeHighscore);
                }
                GUILayout.EndVertical();

            GUILayout.EndHorizontal();

            GUILayout.FlexibleSpace();
        }
        GameObject.Find("GUI_LevelSelect").GetComponent<LevelSelect>().style.label.alignment = TextAnchor.MiddleCenter;

        GUILayout.EndVertical();
    }

    protected void showThumbnail()
    {
        GUILayout.BeginVertical("thumbbox");
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
        GUILayout.EndVertical();
    }

    public abstract void LevelInfoGui();
}

class StoryLevelObj : Levelobj
{
    string respath = "";
    public void initStoryLevel(int id, string levelpath)
    {
        this.id = id;
        this.name = Path.GetFileNameWithoutExtension(levelpath);
        this.respath = levelpath;
    }

    public override void LevelInfoGui()
    {
        //Debug.Log(respath);
        if (StartLoadingThumb) loadThumbnail(respath + "_thumb");
        showThumbnail();
        showHighscore();
        GUILayout.EndVertical();
        GUILayout.BeginHorizontal("bottombar");
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Spielen", "forwardbackwardbuttonfullwidth"))
        {
            SceneManager.Instance.levelToLoad = new LevelAndType(respath, LevelLoader.LevelType.Story);
            SceneManager.Instance.loadScene(SceneManager.Scene.Game);
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
    }

    public override void loadHighScores()
    {
        throw new NotImplementedException();
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

    public override void LevelInfoGui()
    {
        if (StartLoadingThumb) loadThumbnail("file://" + XMLPath.FullName.Substring(0, XMLPath.FullName.Length - 4) + "_thumb.png");
        showThumbnail();
        showHighscore();
        GUILayout.EndVertical();
        GUILayout.BeginHorizontal("bottombar");
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Play", "forwardbackwardbutton"))
        {
            SceneManager.Instance.levelToLoad = new LevelAndType(XMLPath.FullName, LevelLoader.LevelType.Custom);
            SceneManager.Instance.loadScene(SceneManager.Scene.Game);
        }
        if (GUILayout.Button("Edit", "forwardbackwardbutton"))
        {
            SceneManager.Instance.levelToLoad = new LevelAndType(XMLPath.FullName, LevelLoader.LevelType.Custom);
            SceneManager.Instance.loadLevelToEdit = true;
            SceneManager.Instance.loadScene(SceneManager.Scene.Editor);
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
    }

    public override void loadHighScores()
    {
        throw new NotImplementedException();
    }
}

class LocalLevelObj : Levelobj //already on disk
{
    public string hash = null;
    protected bool loadLocalHighscore = true;
    public void init(int id, DirectoryInfo levelpath)
    {
        this.id = id;
        this.name = levelpath.Name;
        searchLocalLevel(levelpath);
        if(XMLPath != null) this.hash = ScoreController.Instance.getMD5ofFile(XMLPath.FullName);
    }

    public override void LevelInfoGui()
    {
        if (StartLoadingThumb) loadThumbnail("file://" + XMLPath.FullName.Substring(0, XMLPath.FullName.Length - 4) + "_thumb.png");
        showThumbnail();
        showHighscore();

        GUILayout.EndVertical();
        GUILayout.BeginHorizontal("bottombar");
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Play ", "forwardbackwardbuttonfullwidth"))
        {
            SceneManager.Instance.levelToLoad = new LevelAndType(XMLPath.FullName, LevelLoader.LevelType.Normal);
            SceneManager.Instance.loadScene(SceneManager.Scene.Game);
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
    }

        public void loadLocalHighScores()
    {
        if (loadLocalHighscore)
        {
            loadLocalHighscore = false;
            localPukeHighscore = ScoreController.Instance.getlocalPukeHighscore(hash);
            localTimeHighscore = ScoreController.Instance.getlocalTimeHighscore(hash);
        }
    }

    public override void loadHighScores()
    {
        throw new NotImplementedException();
    }
}

class OnlineLevelObj : Levelobj //online - not downloaded
{
    public String hash = null;
    int onlinelevelid;

    private string thumburl = "";
    private string downloadurl = "";

    private bool inplaylist;
    private bool currentUserIsCreator;
    public string creatorNickname;

    public void init(int id, string onlinehash)
    {
        throw new NotImplementedException();
    }

    public override void loadHighScores()
    {
        localTimeHighscore = ScoreController.Instance.getlocalTimeHighscore(hash);
    }

    public override void LevelInfoGui()
    {
        if (StartLoadingThumb) loadThumbnail(GlobalVars.Instance.CommunityBasePath + thumburl);
        showThumbnail();
        showHighscore();
        GUILayout.EndVertical();
        GUILayout.BeginHorizontal("bottombar");
        GUILayout.FlexibleSpace();
        if (NetworkManager.Instance.GlobalStatus == NetworkManager.LoginStatus.LoggedIn) {

            if (ThumbCurrentlyLoading)
            {
                GUI.enabled = false;
            }

            if(DownloadLevelManager.Instance.GlobalStatus == DownloadLevelManager.DownloadStatus.Downloading)
            {
                GUI.enabled = false;
                GUILayout.Label("Downloading...", "forwardbackwardbuttonfullwidth");
            }
            else if (DownloadLevelManager.Instance.GlobalStatus == DownloadLevelManager.DownloadStatus.Error)
            {
                GUILayout.Label("Download failed", "forwardbackwardbuttonfullwidth");
            }
            else if (DownloadLevelManager.Instance.GlobalStatus == DownloadLevelManager.DownloadStatus.Downloaded)
            {
                inplaylist = true;
                DownloadLevelManager.Instance.reset();
            }else
            {
                if (inplaylist == true)
                {
                    GUILayout.Label("In Playlist", "forwardbackwardbuttonfullwidth");
                }
                else
                {
                    if (GUILayout.Button("Download", "forwardbackwardbuttonfullwidth"))
                    {
                        AddLevelToPlaylistManager.Instance.addToPlaylist(onlinelevelid);
                        DownloadLevelManager.Instance.startDownload(downloadurl, thumburl, thumbnail, name+ "-" + creatorNickname);
                    }
                }
            }
            
            GUI.enabled = true;
        }
        else
        {
            GUILayout.Label("Not logged in.", "bottombarCurLevelLabel");
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
    }

    internal void init(int id, int onlineid, string hash, string name, string thumburl, string dlUrl, bool inplaylist, bool currentUserIsCreator, string creatorNickname)
    {
        this.id = id;
        this.name = name;
        this.hash = hash;
        this.thumburl = thumburl;
        this.onlinelevelid = onlineid;
        this.downloadurl = dlUrl;
        this.inplaylist = inplaylist;
        this.currentUserIsCreator = currentUserIsCreator;
        this.creatorNickname = creatorNickname;
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

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
        levels = new List<Levelobj>();
	}
	
	// Update is called once per frame
	void Update () {
        if (reloadLevels)
        {
            foreach (Levelobj o in levels)
            {
                Destroy(o);
            }
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
                LevelMaxPages = (levels.Count - 1) / LevelsToShow + 1;
                switch (regsiterToShow)
                {
                    case 0: loadCustomLevels(); LevelMaxPages = levels.Count / LevelsToShow + 1; break;
                    case 1: loadOnlineLevelList(); break;
                    case 2: loadLocalLevelList(); break;
                    case 3: loadStoryLevels(); break;
                }
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

        DirectoryInfo dinfo = new DirectoryInfo(BuildManager.Instance.MapsPath + @"downloaded/");

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
        DirectoryInfo dinfo = new DirectoryInfo(BuildManager.Instance.MapsPath + @"custom/");

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
        style.customStyles[11].fixedHeight = screenHeight * 0.3f;

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
                        int l2s = LevelsToShow;
                        if (regsiterToShow == 0)
                        {
                            if (GUILayout.Button("Create new Level", "button"))
                            {
                                SceneManager.Instance.levelToLoad = new LevelAndType("Maps/inaccessable/emptyLevel/emptyLevel", LevelLoader.LevelType.Story);
                                SceneManager.Instance.loadLevelToEdit = true;
                                SceneManager.Instance.loadScene(SceneManager.Scene.Editor);
                            }
                            l2s = LevelsToShow - 1;
                        }
                        if (levels != null && levels.Count != 0)
                        {
                            for (int i = l2s * LevelsCurPage; i < levels.Count && i < l2s * LevelsCurPage + l2s; i++)
                            {
                                if (selectedLevelid == levels[i].id)
                                {
                                    if (levels[i].GetType() == typeof(OnlineLevelObj) && !((OnlineLevelObj)levels[i]).alreadydownloaded)
                                    {
                                        GUI.enabled = false;
                                        GUILayout.Button("<color=" + buttonDisabledColorHexString + ">" + levels[i].name + "-" + ((OnlineLevelObj)levels[i]).creatorNickname + "</color>", "button");
                                        GUI.enabled = true;
                                    }
                                    else
                                    {
                                        if (GUILayout.Button("<color=" + buttonDisabledColorHexString + ">" + levels[i].name + "</color>", "button"))
                                        {
                                            levels[i].StartLevel();
                                        }
                                    }
                                    
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

                        if(selectedLevelid != -1) levels[selectedLevelid].LevelInfoGui();
                        else {
                            GUILayout.BeginVertical("", "infoBox");
                            GUILayout.EndVertical();
                        }

                    GUILayout.EndVertical();

                GUILayout.EndHorizontal();

            GUILayout.EndArea();

        GUILayout.EndVertical();
    }
}

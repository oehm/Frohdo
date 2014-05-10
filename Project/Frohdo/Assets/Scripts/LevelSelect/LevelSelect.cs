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
            Debug.Log(dinfo.GetDirectories().Length);
            foreach (DirectoryInfo d in dinfo.GetDirectories())
            {
                Levelobj obj;
                try
                {
                    obj = new LocalLevelObj(id, d);
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

        for (int i = 0; i < 8; i++)
        {
            Levelobj obj;
            try
            {
                obj = new OnlineLevelObj(i, 0);
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
            Debug.Log(dinfo.GetDirectories().Length);
            foreach (DirectoryInfo d in dinfo.GetDirectories())
            {
                Levelobj obj;
                try
                {
                    obj = new CustomLevelObj(id, d);
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

        float screenHeight = ForceAspectRatio.screenHeight;
        float screenWidth = ForceAspectRatio.screenWidth;

        style.customStyles[0].fixedWidth = screenWidth / 2;
        style.customStyles[0].fixedHeight = screenHeight * 0.9f;

        style.customStyles[1].fixedWidth = screenWidth;
        style.customStyles[1].fixedHeight = screenHeight * 0.1f;

        style.customStyles[2].fixedHeight = screenHeight * 0.1f;
        style.customStyles[2].fixedWidth = screenWidth / 6;
        style.customStyles[2].fontSize = (int)(screenHeight * 0.05f);

        style.customStyles[3].fixedWidth = screenWidth / 2;
        style.customStyles[3].fixedHeight = screenHeight * 0.8f;

        style.customStyles[4].fixedWidth = screenWidth / 2;
        style.customStyles[4].fixedHeight = screenHeight * 0.1f;

        style.customStyles[5].fixedWidth = screenWidth / 6;
        style.customStyles[5].fixedHeight = screenHeight * 0.1f;
        style.customStyles[5].fontSize = (int)(screenHeight * 0.05f);

        style.customStyles[6].fixedWidth = screenWidth / 10;
        style.customStyles[6].fixedHeight = screenHeight * 0.1f;
        style.customStyles[6].fontSize = (int)(screenHeight * 0.05f);

        style.customStyles[7].fixedWidth = screenWidth;
        style.customStyles[7].fixedHeight = screenHeight;

        style.button.fixedWidth = screenWidth / 2;
        style.button.fixedHeight = screenHeight * 0.8f / LevelsToShow;
        style.button.fontSize = (int)((screenHeight * 0.8f / LevelsToShow) * 0.5f);

        style.label.fontSize = (int)((screenHeight * 0.8f / LevelsToShow) * 0.5f);

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
                        GUILayout.Button("<color=" + buttonDisabledColorHexString + ">Eigene Levels</color>", "topbarbutton");
                        GUI.enabled = true;
                    }
                    else
                    {
                        if (GUILayout.Button("Eigene Levels", "topbarbutton"))
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
            
                    GUILayout.BeginVertical("", "infoBox");
                
                    if(selectedLevelid != -1){
                        if (levels[selectedLevelid].GetType() == typeof(CustomLevelObj))
                            GUILayout.Label("EIN CUSTOM LEVEL! - LAUFZEIT TYP-PRÜFUNG!");
                        if (levels[selectedLevelid].GetType() == typeof(OnlineLevelObj))
                            GUILayout.Label("EIN ONLINE LEVEL! - LAUFZEIT TYP-PRÜFUNG!");
                        if (levels[selectedLevelid].GetType() == typeof(StoryLevelObj))
                            GUILayout.Label("EIN STORY LEVEL! - LAUFZEIT TYP-PRÜFUNG!");
                        if (levels[selectedLevelid].GetType() == typeof(LocalLevelObj))
                            GUILayout.Label("EIN LOKAL GESPEICHERTES LEVEL! - LAUFZEIT TYP-PRÜFUNG!");
                        GUILayout.Label("ID: " + levels[selectedLevelid].id);
                    }

                    GUILayout.EndVertical();

                GUILayout.EndHorizontal();
            GUILayout.EndArea();
        GUILayout.EndVertical();
    }
}

abstract class Levelobj
{
    public int id;
    public string name;

    public int onlineID;
    //if there is no online ID in the XML.. We wont show this level in Playlist! (because the level never got uploaded or id got modified and we cant match with database!)
    //gehört noch in die Level class und ins xml.
    //Beim upload holen wir erst ne neue id vom server (wir schicken alle relevanten daten mit, Name und so -- kein hasth.. das macht server!).. vergeben und speichern diese id in der XML .. und machen dann den upload..
    //danach wird der Level geuppt und am server der Hash erzeugt und in die DB abgelegt.. und auch der Pfad zur neuen Datei!
    
    public Texture2D thumbnail;

    public abstract void loadHighScores();
    public abstract void loadThumbnail();

    public Level level;

    protected void loadLocalLevel(DirectoryInfo levelpath)
    {
        FileInfo XMLFile = null;

        foreach (FileInfo f in levelpath.GetFiles())
        {
            if (f.Extension.Equals(".xml"))
            {
                if (XMLFile != null) throw new Exception("More than one XML-File found in Level-Directory!");
                else XMLFile = f;
            }
        }
        if (XMLFile != null)
        {
            try
            {
                Debug.Log("Tryin to Load: " + XMLFile.FullName);
                level = XML_Loader.Load(XMLFile.FullName);
            }
            catch (Exception)
            {
                throw new Exception("Level XML found but failed to Load Level");
            }
        }
        else
        {
            throw new Exception("Level XML not found in Level Directory");
        }
    }
}

class StoryLevelObj : Levelobj
{
    public StoryLevelObj(int id, DirectoryInfo levelpath)
    {
        this.id = id;
        this.name = levelpath.Name;
        loadLocalLevel(levelpath);
    }
    public override void loadHighScores()
    {
        throw new NotImplementedException();
    }

    public override void loadThumbnail()
    {
        throw new NotImplementedException();
    }
}
class CustomLevelObj : Levelobj
{
    public CustomLevelObj(int id, DirectoryInfo levelpath)
    {
        this.id = id;
        this.name = levelpath.Name;
        loadLocalLevel(levelpath);
    }

    public override void loadThumbnail()
    {
        throw new NotImplementedException();
    }

    public override void loadHighScores()
    {
        throw new NotImplementedException();
    }
}

class LocalLevelObj : Levelobj //already on disk
{
    public List<highscore> highscores; //werden abgerufen vom Server oder lokal aus ner txt.

    public LocalLevelObj(int id, DirectoryInfo levelpath)
    {
        this.id = id;
        this.name = levelpath.Name;
        loadLocalLevel(levelpath);
    }

    public override void loadHighScores()
    {
        highscores = new List<highscore>();
    }

    public override void loadThumbnail()
    {
        throw new NotImplementedException();
    }
}

class OnlineLevelObj : Levelobj //online - not downloaded
{
    public List<highscore> highscores; //werden nur vom Server geholt.

    public OnlineLevelObj(int id, int onlineid)
    {
        this.id = id;
        this.name = "Online-Level " + id;
    }

    public override void loadThumbnail()
    {
        throw new NotImplementedException();
    }

    public override void loadHighScores()
    {
        throw new NotImplementedException();
    }
}

class highscore
{
    string user;
    float score;

    public highscore(string user, float score){
        this.user = user;
        this.score = score;
    }
}

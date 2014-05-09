using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelSelect : MonoBehaviour {

	// Use this for initialization

    public int LevelsToShow = 10;
    public GUISkin style;
    public string buttonDisabledColorHexString = "#d35400";

    List<Levelobj> levels;

    int LevelMaxPages;
    int LevelsCurPage;

    int selectedLevelid;

    int regsiterToShow; //0 = own levels; 1 = onlineLevels
    bool reloadLevels;

	void Start () {
        regsiterToShow = 0;
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
                case 0: loadLocalLevelList(); break;
                case 1: loadOnlineLevelList(); break;
            }
            LevelMaxPages = levels.Count / LevelsToShow + 1;
            LevelsCurPage = 0;
        }
	}

    void loadLocalLevelList()
    {
        levels = new List<Levelobj>();

        for (int i = 0; i < 25; i++)
        {
            Levelobj obj = new Levelobj(i, "Level" + (i + 1));
            levels.Add(obj);
        }
    }

    void loadOnlineLevelList()
    {
        levels = new List<Levelobj>();

        for (int i = 0; i < 8; i++)
        {
            Levelobj obj = new Levelobj(i, "Online - Level" + (i + 1));
            levels.Add(obj);
        }
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

        style.customStyles[0].fixedWidth = Screen.width / 2;
        style.customStyles[0].fixedHeight = Screen.height * 0.9f;

        style.customStyles[1].fixedWidth = Screen.width;
        style.customStyles[1].fixedHeight = Screen.height * 0.1f;

        style.customStyles[2].fixedHeight = Screen.height * 0.1f;
        style.customStyles[2].fontSize = (int)(Screen.height * 0.09f);

        style.customStyles[3].fixedWidth = Screen.width / 2;
        style.customStyles[3].fixedHeight = Screen.height * 0.8f;

        style.customStyles[4].fixedWidth = Screen.width / 2;
        style.customStyles[4].fixedHeight = Screen.height * 0.1f;

        style.customStyles[5].fixedWidth = Screen.width / 6;
        style.customStyles[5].fixedHeight = Screen.height * 0.1f;
        style.customStyles[5].fontSize = (int)(Screen.height * 0.09f);

        style.customStyles[6].fixedWidth = Screen.width / 10;
        style.customStyles[6].fixedHeight = Screen.height * 0.1f;
        style.customStyles[6].fontSize = (int)(Screen.height * 0.09f);

        style.button.fixedWidth = Screen.width / 2;
        style.button.fixedHeight = Screen.height * 0.8f / LevelsToShow;
        style.button.fontSize = (int)((Screen.height * 0.8f / LevelsToShow) * 0.9f);

        style.label.fontSize = (int)((Screen.height * 0.8f / LevelsToShow) * 0.9f);

        GUI.skin = style;

        GUILayout.BeginVertical("box");

            GUILayout.BeginHorizontal("", "topbar");
                GUILayout.FlexibleSpace();
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
                    if (levels != null)
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

                    //GUILayout.Button("TESTLKVGDBJKL", "forwardbackwardbutton");
                    //GUILayout.Button("TESTLKVGDBJKL", "forwardbackwardbutton");

                    GUILayout.FlexibleSpace();
                    
                    GUILayout.EndHorizontal();

                GUILayout.EndVertical();
            
                GUILayout.BeginVertical("", "infoBox");
                
                if(selectedLevelid != -1){
                    GUILayout.Label("ID: " + levels[selectedLevelid].id);
                }

                GUILayout.EndVertical();

            GUILayout.EndHorizontal();

        GUILayout.EndVertical();
    }
}

class Levelobj
{
    public int id;
    public string name;
    public SortedList highscores;
    public Levelobj(int id, string name)
    {
        this.id = id;
        this.name = name;
        this.highscores = new SortedList();
    }

    public void loadhighscores()
    {

    }
}

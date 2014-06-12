using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System;

abstract class Levelobj : MonoBehaviour
{
    public int id;
    public new string name;

    public Texture2D thumbnail;

    public FileInfo XMLPath = null;

    protected bool StartLoadingThumb = true;
    protected bool ThumbCurrentlyLoading = false;

    protected WWW thumbdownload;
    const float downloadTimeout = 3.0f;
    float currentDownloadTime = 0.0f;

    public List<Highscore> highscores;

    protected int localPukeHighscore;
    protected int localTimeHighscore;

    public abstract void StartLevel();

    bool highscoresToShow;

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
            if (this.GetType() != typeof(StoryLevelObj))
            {
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

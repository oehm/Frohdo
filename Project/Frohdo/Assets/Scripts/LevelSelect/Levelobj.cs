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

    protected DownloadHighscoreManager.HighscoreType highscoresToShow;
    private string buttonDisabledColorHexString = GlobalVars.Instance.ButtonDisabledHexString;
    private string OwnHighscorehiglightColor = GlobalVars.Instance.OwnHighscoreHighlightColor;

    public abstract void loadLocalHighScores();

    public abstract void loadOnlineHighscores(DownloadHighscoreManager.HighscoreType type);

    bool onlineavailable;

    public void ManUpdate()
    {
        //Debug.Log("Update_" + name);
        onlineavailable = NetworkManager.Instance.GlobalStatus == NetworkManager.LoginStatus.LoggedIn && (this.GetType() == typeof(LocalLevelObj) || this.GetType() == typeof(OnlineLevelObj));
        if (!onlineavailable && (highscoresToShow == DownloadHighscoreManager.HighscoreType.OnlinePuke || highscoresToShow == DownloadHighscoreManager.HighscoreType.OnlineTime))
        {
            if (highscoresToShow == DownloadHighscoreManager.HighscoreType.OnlineTime) highscoresToShow = DownloadHighscoreManager.HighscoreType.LocalTime;
            else if (highscoresToShow == DownloadHighscoreManager.HighscoreType.OnlinePuke) highscoresToShow = DownloadHighscoreManager.HighscoreType.LocalPuke;
            loadLocalHighScores();
        }
    }

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
            //Debug.Log(thumbdownload.error);
        }
    }

    protected void showHighscore()
    {
        bool localavailable = this.GetType() == typeof(StoryLevelObj) || this.GetType() == typeof(LocalLevelObj) || (this.GetType() == typeof(OnlineLevelObj) && ((OnlineLevelObj)this).alreadydownloaded);
        GUILayout.BeginVertical("highscoreBox");
            GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                    if (localavailable)
                    {
                        if (highscoresToShow == DownloadHighscoreManager.HighscoreType.LocalTime || highscoresToShow == DownloadHighscoreManager.HighscoreType.LocalPuke)
                        {
                            GUILayout.Label("<color=" + buttonDisabledColorHexString + ">Local</color>", "highscorebuttonfont");
                        }
                        else
                        {
                            if (SoundButton.newSoundButton("Local", "highscorebuttonfont"))
                            {
                                loadLocalHighScores();
                                if (highscoresToShow == DownloadHighscoreManager.HighscoreType.OnlineTime) highscoresToShow = DownloadHighscoreManager.HighscoreType.LocalTime;
                                else if (highscoresToShow == DownloadHighscoreManager.HighscoreType.OnlinePuke) highscoresToShow = DownloadHighscoreManager.HighscoreType.LocalPuke;
                            }
                        }
                    }
                    if (onlineavailable)
                    {
                        if (highscoresToShow == DownloadHighscoreManager.HighscoreType.OnlineTime || highscoresToShow == DownloadHighscoreManager.HighscoreType.OnlinePuke)
                        {
                            GUILayout.Label("<color=" + buttonDisabledColorHexString + ">Online</color>", "highscorebuttonfont");
                        }
                        else
                        {
                            if (SoundButton.newSoundButton("Online", "highscorebuttonfont"))
                            {
                                if (highscoresToShow == DownloadHighscoreManager.HighscoreType.LocalTime) highscoresToShow = DownloadHighscoreManager.HighscoreType.OnlineTime;
                                else if (highscoresToShow == DownloadHighscoreManager.HighscoreType.LocalPuke) highscoresToShow = DownloadHighscoreManager.HighscoreType.OnlinePuke;
                                loadOnlineHighscores(highscoresToShow);
                            }
                        }
                    }
                GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                    if (onlineavailable || localavailable)
                    {
                        if (highscoresToShow == DownloadHighscoreManager.HighscoreType.OnlineTime || highscoresToShow == DownloadHighscoreManager.HighscoreType.LocalTime)
                        {
                            GUILayout.Label("<color=" + buttonDisabledColorHexString + ">Time</color>", "highscorebuttonfont");
                        }
                        else
                        {
                            if (SoundButton.newSoundButton("Time", "highscorebuttonfont"))
                            {
                                if (highscoresToShow == DownloadHighscoreManager.HighscoreType.LocalPuke) highscoresToShow = DownloadHighscoreManager.HighscoreType.LocalTime;
                                else if (highscoresToShow == DownloadHighscoreManager.HighscoreType.OnlinePuke)
                                {
                                    highscoresToShow = DownloadHighscoreManager.HighscoreType.OnlineTime;
                                    loadOnlineHighscores(highscoresToShow);
                                }
                            }
                        }

                        if (highscoresToShow == DownloadHighscoreManager.HighscoreType.OnlinePuke || highscoresToShow == DownloadHighscoreManager.HighscoreType.LocalPuke)
                        {
                            GUILayout.Label("<color=" + buttonDisabledColorHexString + ">Pukes</color>", "highscorebuttonfont");
                        }
                        else
                        {
                            if (SoundButton.newSoundButton("Pukes", "highscorebuttonfont"))
                            {
                                if (highscoresToShow == DownloadHighscoreManager.HighscoreType.LocalTime) highscoresToShow = DownloadHighscoreManager.HighscoreType.LocalPuke;
                                else if (highscoresToShow == DownloadHighscoreManager.HighscoreType.OnlineTime)
                                {
                                    highscoresToShow = DownloadHighscoreManager.HighscoreType.OnlinePuke;
                                    loadOnlineHighscores(highscoresToShow);
                                }
                            }
                        }
                    }
                GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.BeginVertical();
                if (highscoresToShow == DownloadHighscoreManager.HighscoreType.OnlineTime)
                    {
                        if (highscores.Count == 0)
                        {
                            GUILayout.FlexibleSpace();
                            if (DownloadHighscoreManager.Instance.GlobalStatus == DownloadHighscoreManager.DownloadStatus.Downloading) GUILayout.Label("Loading highscores ...", "highscoreentrybox");
                            else if (DownloadHighscoreManager.Instance.GlobalStatus == DownloadHighscoreManager.DownloadStatus.NotFoundOnServer)
                            {
                                GUILayout.Label("Level not found online!", "highscoreentrybox");
                            }
                            else
                            {
                                GUILayout.Label("No highscores yet!", "highscoreentrybox");
                            }
                            GUILayout.FlexibleSpace();
                        }
                        else
                        {
                            foreach (Highscore score in highscores)
                            {
                                string s = score.score.ToString();
                                while (s.Length < 4)
                                {
                                    s += "0";
                                }
                                s = s.Insert(s.Length - 3, ".");
                                s += " sec";
                                if (score.ownHighscore) GUILayout.Label("<color=" + OwnHighscorehiglightColor + ">" + score.rank + ": " + s + " -- " + score.user + "</color>", "highscoreentryboxleftalign");
                                else GUILayout.Label(score.rank + ": " + s + " -- " + score.user, "highscoreentryboxleftalign");
                            }
                        }
                    }
                else if (highscoresToShow == DownloadHighscoreManager.HighscoreType.OnlinePuke)
                    {
                        if (highscores.Count == 0)
                        {
                            GUILayout.FlexibleSpace();
                            if (DownloadHighscoreManager.Instance.GlobalStatus == DownloadHighscoreManager.DownloadStatus.Downloading) GUILayout.Label("Loading highscores ...", "highscoreentrybox");
                            else if (DownloadHighscoreManager.Instance.GlobalStatus == DownloadHighscoreManager.DownloadStatus.NotFoundOnServer)
                            {
                                GUILayout.Label("Level not found online!", "highscoreentrybox");
                            }
                            else
                            {
                                GUILayout.Label("No highscores yet!", "highscoreentrybox");
                            }
                            GUILayout.FlexibleSpace();
                        }
                        else
                        {
                            foreach (Highscore score in highscores)
                            {
                                string s = score.score.ToString();
                                if (score.score == 1) s += " puke";
                                else s += " pukes";
                                if (score.ownHighscore) GUILayout.Label("<color=" + OwnHighscorehiglightColor + ">" + score.rank + ": " + s + " -- " + score.user + "</color>", "highscoreentryboxleftalign");
                                else GUILayout.Label(score.rank + ": " + s + " -- " + score.user, "highscoreentryboxleftalign");
                            }
                        }
                    }

                    GUILayout.FlexibleSpace();
                    
                    if (highscoresToShow == DownloadHighscoreManager.HighscoreType.LocalTime)
                    {
                        if (localTimeHighscore == -1)
                        {
                            GUILayout.FlexibleSpace();
                            GUILayout.Label("No highscore yet!", "highscoreentrybox");
                            GUILayout.FlexibleSpace();
                        }
                        else
                        {
                            string s = localTimeHighscore.ToString();
                            while (s.Length < 4)
                            {
                                s += "0";
                            }
                            s = s.Insert(s.Length - 3, ".");
                            s += " sec";
                            GUILayout.Label(s, "highscoreentrybox");
                        }
                    }
                else if (highscoresToShow == DownloadHighscoreManager.HighscoreType.LocalPuke)
                    {
                        if (localPukeHighscore == -1)
                        {
                            GUILayout.FlexibleSpace();
                            GUILayout.Label("No highscore yet!", "highscoreentrybox");
                            GUILayout.FlexibleSpace();
                        }
                        else
                        {
                            string s = localPukeHighscore.ToString();
                            if (localPukeHighscore == 1) s += " puke";
                            else s += " pukes";
                            GUILayout.Label(s, "highscoreentrybox");
                        }
                    }
                else if (highscoresToShow == DownloadHighscoreManager.HighscoreType.None)
                    {
                        GUILayout.Label("No Highscores available!", "highscoreentrybox");
                    }
                GUILayout.FlexibleSpace();
            GUILayout.EndVertical();

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

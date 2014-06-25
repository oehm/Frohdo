using UnityEngine;
using System.Collections;
using System;
using System.IO;

class OnlineLevelObj : Levelobj //online - not downloaded
{
    public string hash = null;
    int onlinelevelid;

    private string thumburl = "";
    private string downloadurl = "";

    private bool inplaylist;
    private bool currentUserIsCreator;
    public string creatorNickname;

    private string localReservedUrlForDownload;
    private string localReservedThumbUrlForDownload;

    public bool alreadydownloaded = false;

    public void init(int id, string onlinehash)
    {
        throw new NotImplementedException();
    }

    public override void LevelInfoGui()
    {
        if (StartLoadingThumb)
        {
            if (alreadydownloaded) loadThumbnail("file://" + localReservedThumbUrlForDownload);
            else loadThumbnail(GlobalVars.Instance.CommunityBasePath + thumburl);
        }
        GUILayout.BeginVertical("", "infoBox");
        showThumbnail();
        showHighscore();
        GUILayout.EndVertical();
        GUILayout.BeginHorizontal("bottombar");
        GUILayout.FlexibleSpace();
        if (NetworkManager.Instance.GlobalStatus == NetworkManager.LoginStatus.LoggedIn)
        {

            if (ThumbCurrentlyLoading)
            {
                GUI.enabled = false;
            }

            if (DownloadLevelManager.Instance.GlobalStatus == DownloadLevelManager.DownloadStatus.Downloading)
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
                alreadydownloaded = true;
                DownloadLevelManager.Instance.reset();
            }
            else
            {
                if (alreadydownloaded == true)
                {
                    if (SoundButton.newSoundButton("Play ", "forwardbackwardbuttonfullwidth"))
                    {
                        StartLevel();
                    }
                }
                else
                {
                    if (SoundButton.newSoundButton("Download", "forwardbackwardbuttonfullwidth"))
                    {
                        AddLevelToPlaylistManager.Instance.addToPlaylist(onlinelevelid);
                        DownloadLevelManager.Instance.startDownload(downloadurl, thumburl, thumbnail, name + "-" + creatorNickname);
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

        this.localReservedUrlForDownload = BuildManager.Instance.MapsPath + @"/downloaded/" + name + "-" + creatorNickname + @"/" + name + "-" + creatorNickname + ".xml";
        this.localReservedThumbUrlForDownload = BuildManager.Instance.MapsPath + @"/downloaded/" + name + "-" + creatorNickname + @"/" + name + "-" + creatorNickname + "_thumb.png";

        alreadydownloaded = File.Exists(localReservedUrlForDownload) && File.Exists(localReservedThumbUrlForDownload);

        highscoresToShow = DownloadHighscoreManager.HighscoreType.OnlineTime;
    }

    public override void StartLevel()
    {
        if (DownloadLevelManager.Instance.GlobalStatus != DownloadLevelManager.DownloadStatus.Downloading)
        {
            SceneManager.Instance.previewlevel = false;
            SceneManager.Instance.levelToLoad = new LevelAndType(localReservedUrlForDownload, LevelLoader.LevelType.Normal, thumbnail);
            SceneManager.Instance.loadScene(SceneManager.Scene.Game);
        }
    }

    public override void loadLocalHighScores() //only for Top online Levels which are already downloaded
    {
        //Debug.Log("Load Online Level Highscore");
        //Debug.Log(hash);
        localPukeHighscore = ScoreController.Instance.getlocalPukeHighscore(hash);
        localTimeHighscore = ScoreController.Instance.getlocalTimeHighscore(hash);
    }

    public override void loadOnlineHighscores(DownloadHighscoreManager.HighscoreType type)
    {
        DownloadHighscoreManager.Instance.startDownload(hash, ref highscores, type);
        //throw new NotImplementedException();
    }
}

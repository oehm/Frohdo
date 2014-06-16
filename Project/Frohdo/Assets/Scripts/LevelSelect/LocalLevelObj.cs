using UnityEngine;
using System.Collections;
using System.IO;

class LocalLevelObj : Levelobj //already on disk
{
    public string hash = null;
    public void init(int id, DirectoryInfo levelpath)
    {
        this.id = id;
        this.name = levelpath.Name;
        searchLocalLevel(levelpath);
        if (XMLPath != null) this.hash = ScoreController.Instance.getMD5ofFile(XMLPath.FullName);
        
        if (NetworkManager.Instance.GlobalStatus == NetworkManager.LoginStatus.LoggedIn) highscoresToShow = DownloadHighscoreManager.HighscoreType.OnlineTime;
        else highscoresToShow = DownloadHighscoreManager.HighscoreType.LocalTime;
    }

    public override void LevelInfoGui()
    {
        if (StartLoadingThumb) loadThumbnail("file://" + XMLPath.FullName.Substring(0, XMLPath.FullName.Length - 4) + "_thumb.png");
        GUILayout.BeginVertical("", "infoBox");
        showThumbnail();
        showHighscore();
        GUILayout.EndVertical();

        GUILayout.BeginHorizontal("bottombar");
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Play ", "forwardbackwardbuttonfullwidth"))
        {
            StartLevel();
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
    }

    public override void loadLocalHighScores()
    {
        //Debug.Log("Load Local Level Highscore");
        localPukeHighscore = ScoreController.Instance.getlocalPukeHighscore(hash);
        localTimeHighscore = ScoreController.Instance.getlocalTimeHighscore(hash);
    }

    public override void StartLevel()
    {
        if (DownloadLevelManager.Instance.GlobalStatus != DownloadLevelManager.DownloadStatus.Downloading)
        {
            SceneManager.Instance.previewlevel = false;
            SceneManager.Instance.levelToLoad = new LevelAndType(XMLPath.FullName, LevelLoader.LevelType.Normal, thumbnail);
            SceneManager.Instance.loadScene(SceneManager.Scene.Game);
        }
    }

    public override void loadOnlineHighscores(DownloadHighscoreManager.HighscoreType type)
    {
        if (NetworkManager.Instance.GlobalStatus == NetworkManager.LoginStatus.LoggedIn)
        {
            DownloadHighscoreManager.Instance.startDownload(hash, ref highscores, type);
        }
    }
}

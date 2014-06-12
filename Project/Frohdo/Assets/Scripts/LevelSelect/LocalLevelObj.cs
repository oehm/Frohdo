using UnityEngine;
using System.Collections;
using System.IO;

class LocalLevelObj : Levelobj //already on disk
{
    public string hash = null;
    protected bool loadLocalHighscore = true;
    public void init(int id, DirectoryInfo levelpath)
    {
        this.id = id;
        this.name = levelpath.Name;
        searchLocalLevel(levelpath);
        if (XMLPath != null) this.hash = ScoreController.Instance.getMD5ofFile(XMLPath.FullName);
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

    public void loadLocalHighScores()
    {
        if (loadLocalHighscore)
        {
            loadLocalHighscore = false;
            localPukeHighscore = ScoreController.Instance.getlocalPukeHighscore(hash);
            localTimeHighscore = ScoreController.Instance.getlocalTimeHighscore(hash);
        }
    }

    public override void StartLevel()
    {
        if (DownloadLevelManager.Instance.GlobalStatus != DownloadLevelManager.DownloadStatus.Downloading)
        {
            SceneManager.Instance.levelToLoad = new LevelAndType(XMLPath.FullName, LevelLoader.LevelType.Normal, thumbnail);
            SceneManager.Instance.loadScene(SceneManager.Scene.Game);
        }
    }
}

using UnityEngine;
using System.Collections;
using System.IO;

class CustomLevelObj : Levelobj
{
    public void init(int id, DirectoryInfo levelpath)
    {
        this.id = id;
        this.name = levelpath.Name;
        searchLocalLevel(levelpath);
        highscoresToShow = DownloadHighscoreManager.HighscoreType.None;
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
        if (GUILayout.Button("Play", "forwardbackwardbutton"))
        {
            StartLevel();
        }
        if (GUILayout.Button("Edit", "forwardbackwardbutton"))
        {
            SceneManager.Instance.levelToLoad = new LevelAndType(XMLPath.FullName, LevelLoader.LevelType.Custom, thumbnail);
            SceneManager.Instance.loadLevelToEdit = true;
            SceneManager.Instance.loadScene(SceneManager.Scene.Editor);
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
    }

    public override void StartLevel()
    {
        if (DownloadLevelManager.Instance.GlobalStatus != DownloadLevelManager.DownloadStatus.Downloading)
        {
            SceneManager.Instance.levelToLoad = new LevelAndType(XMLPath.FullName, LevelLoader.LevelType.Custom, thumbnail);
            SceneManager.Instance.loadScene(SceneManager.Scene.Game);
        }
    }

    public override void loadLocalHighScores()
    {
        Debug.Log("No custom level highscores!");
    }

    public override void loadOnlineHighscores(DownloadHighscoreManager.HighscoreType type)
    {
        throw new System.NotImplementedException();
    }
}

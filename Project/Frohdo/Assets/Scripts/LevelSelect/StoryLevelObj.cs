using UnityEngine;
using System.Collections;
using System.IO;

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
        GUILayout.BeginVertical("", "infoBox");
        showThumbnail();
        showHighscore();
        GUILayout.EndVertical();

        GUILayout.BeginHorizontal("bottombar");
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Play", "forwardbackwardbuttonfullwidth"))
        {
            StartLevel();
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
    }

    public override void StartLevel()
    {
        if (DownloadLevelManager.Instance.GlobalStatus != DownloadLevelManager.DownloadStatus.Downloading)
        {
            SceneManager.Instance.levelToLoad = new LevelAndType(respath, LevelLoader.LevelType.Story, thumbnail);
            SceneManager.Instance.loadScene(SceneManager.Scene.Game);
        }
    }
}

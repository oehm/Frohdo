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
        highscoresToShow = DownloadHighscoreManager.HighscoreType.LocalTime;
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
        if (SoundButton.newSoundButton("Play", "forwardbackwardbuttonfullwidth"))
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
            SceneManager.Instance.previewlevel = false;
            SceneManager.Instance.levelToLoad = new LevelAndType(respath, LevelLoader.LevelType.Story, thumbnail);
            SceneManager.Instance.loadScene(SceneManager.Scene.Game);
        }
    }

    public override void loadLocalHighScores()
    {
        //Debug.Log("Load Story Level Highscore");
        localPukeHighscore = ScoreController.Instance.getlocalPukeHighscore(name + "_score");
        localTimeHighscore = ScoreController.Instance.getlocalTimeHighscore(name + "_score");
    }

    public override void loadOnlineHighscores(DownloadHighscoreManager.HighscoreType type)
    {
        throw new System.NotImplementedException();
    }
}

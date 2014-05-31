using UnityEngine;
using System.Collections;

public class RateScreen : MonoBehaviour {

    public GUISkin style;
    
    private string levelHash_;

    private int pukeCount_;
    private int timeCount_;

    private int screenHeight;
    private int screenWidth;

    private int thumbtoShow = 0;

	// Use this for initialization
    void Start()
    {
        levelHash_ = ScoreController.Instance.LevelHash;
        pukeCount_ = ScoreController.Instance.pukeCount;
        timeCount_ = ScoreController.Instance.timeCount;
        //Debug.Log(levelHash_);
        ScoreController.Instance.saveLocalPukeHighscore(levelHash_);
        ScoreController.Instance.saveLocalTimeHighScore(levelHash_);
        thumbtoShow = 0;
    }

    void OnGUI()
    {
        screenHeight = ForceAspectRatio.screenHeight;
        screenWidth = ForceAspectRatio.screenWidth;

        style.customStyles[0].fixedWidth = screenWidth;
        style.customStyles[0].fixedHeight = screenHeight;

        style.textField.fixedWidth = screenWidth / 2f;
        style.textField.fixedHeight = screenHeight / 12;
        style.textField.fontSize = (int)((screenHeight / 12) * 0.7);

        style.label.fontSize = (int)((screenHeight / 12) * 0.7);

        //Center Align Label
        style.customStyles[1].fontSize = (int)((screenHeight / 12) * 0.7);
        style.customStyles[1].fixedWidth = screenWidth / 4;
        style.customStyles[1].fixedHeight = screenHeight / 8;

        //centerAlignDoubleWidth
        style.customStyles[2].fontSize = (int)((screenHeight / 12) * 0.7);
        style.customStyles[2].fixedWidth = screenWidth / 2f;
        style.customStyles[2].fixedHeight = screenHeight / 8;

        //double width button
        style.customStyles[3].fontSize = (int)((screenHeight / 12) * 0.7);
        style.customStyles[3].fixedWidth = screenWidth / 2f;
        style.customStyles[3].fixedHeight = screenHeight / 8;

        //fontsize for login-button ...
        style.button.fontSize = (int)((screenHeight / 12) * 0.7);
        style.button.fixedHeight = screenHeight / 10;
        style.button.fixedWidth = screenWidth / 4;

        //font for visit community button
        style.customStyles[5].fontSize = (int)((screenHeight / 12) * 0.6);
        style.customStyles[5].fixedWidth = screenWidth / 2f;
        style.customStyles[5].fixedHeight = screenHeight / 8;

        //halfbox to split screen into 2 halfs
        style.customStyles[4].fixedWidth = screenWidth / 2f;
        style.customStyles[4].fixedHeight = screenHeight * 0.9f;

        //box with color hexagon puicture
        style.box.fixedHeight = screenHeight;
        style.box.fixedWidth = screenWidth / 2;

        style.customStyles[6].fixedWidth = screenWidth / 2;
        style.customStyles[6].fixedHeight = screenHeight * 0.5f;

        style.customStyles[7].fixedWidth = screenWidth / 2;
        style.customStyles[7].fixedHeight = screenHeight * 0.4f;

        style.customStyles[8].fixedWidth = screenWidth;
        style.customStyles[8].fixedHeight = screenHeight * 0.1f;

        style.customStyles[9].fixedWidth = screenWidth / 2.5f;
        style.customStyles[9].fixedHeight = screenHeight * 0.4f;

        GUI.skin = style;
        GUILayout.BeginArea(new Rect(ForceAspectRatio.xOffset, ForceAspectRatio.yOffset, screenWidth, screenHeight), "", style.customStyles[0]);

            GUILayout.BeginVertical();
        
                GUILayout.BeginHorizontal();        

                    GUILayout.BeginVertical("HalfBox");

                        GUILayout.BeginVertical("TitleBox");
                            GUILayout.FlexibleSpace();

                                GUILayout.Label(SceneManager.Instance.levelToLoad.LevelTitle, "CenterAlignLabelDoubleWidth");

                            GUILayout.FlexibleSpace();
                        GUILayout.EndVertical();

                        GUILayout.BeginVertical("ScoreBox");
                            GUILayout.FlexibleSpace();

                                if (SceneManager.Instance.levelToLoad.type == LevelLoader.LevelType.Story)
                                {
                                    drawStoryLevelGuiLeftMainbody();
                                }

                                if (SceneManager.Instance.levelToLoad.type == LevelLoader.LevelType.Normal)
                                {
                                    drawPlaylistLevelGuiLeftMainbody();
                                }

                                if (SceneManager.Instance.levelToLoad.type == LevelLoader.LevelType.Custom)
                                {
                                    drawCustomLevelGuiLeftMainbody();
                                }

                            GUILayout.FlexibleSpace();

                        GUILayout.EndVertical();

                    GUILayout.EndVertical();

                    GUILayout.BeginVertical("HalfBox");
                        GUILayout.FlexibleSpace();

                            if (SceneManager.Instance.levelToLoad.type == LevelLoader.LevelType.Story)
                            {
                                drawStoryLevelGuiRightHalf();
                            }

                            if (SceneManager.Instance.levelToLoad.type == LevelLoader.LevelType.Normal)
                            {
                                drawPlaylistLevelGuiRightHalf();
                            }

                            if (SceneManager.Instance.levelToLoad.type == LevelLoader.LevelType.Custom)
                            {
                                drawCustomLevelGuiRightHalf();
                            }

                        GUILayout.FlexibleSpace();
                    GUILayout.EndVertical();

                GUILayout.EndHorizontal();

                GUILayout.BeginVertical("ButtonBox");

                    GUILayout.BeginHorizontal();

                        if (SceneManager.Instance.levelToLoad.type == LevelLoader.LevelType.Story)
                        {
                            drawStoryLevelButtons();
                        }

                        if (SceneManager.Instance.levelToLoad.type == LevelLoader.LevelType.Normal)
                        {
                            drawPlaylistLevelButtons();
                        }

                        if (SceneManager.Instance.levelToLoad.type == LevelLoader.LevelType.Custom)
                        {
                            drawCustomLevelButtons();
                        }

                    GUILayout.EndHorizontal();

                GUILayout.EndVertical();

           GUILayout.EndVertical();

        GUILayout.EndArea();
    }

	// Update is called once per frame
	void Update () {
	
	}

    void drawCustomLevelGuiLeftMainbody()
    {
    }

    void drawCustomLevelGuiRightHalf()
    {

    }

    void drawStoryLevelGuiLeftMainbody()
    {
        GUILayout.BeginHorizontal();

            GUILayout.FlexibleSpace();

                GUILayout.BeginVertical();

                    GUILayout.BeginHorizontal();

                        GUILayout.Label("Time: " + timeCount_);

                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();

                        GUILayout.Label("Pukes: " + pukeCount_);

                    GUILayout.EndHorizontal();

                GUILayout.EndVertical();

            GUILayout.FlexibleSpace();

        GUILayout.EndHorizontal();
    }

    void drawStoryLevelGuiRightHalf()
    {
        GUILayout.Label("Highlights", "CenterAlignLabelDoubleWidth");
        GUILayout.BeginHorizontal();

            GUILayout.FlexibleSpace();

                GUILayout.Box(ScreenShotManager.Instance.screens[thumbtoShow], "ScreensBox");

            GUILayout.FlexibleSpace();

        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();

            if (ScreenShotManager.Instance.screens.Count <= 1)
            {
                GUI.enabled = false;
            }

            if (GUILayout.Button("<")) thumbtoShow = (thumbtoShow - 1) % ScreenShotManager.Instance.screens.Count;

            GUILayout.Label("Thumbnail", "CenterAlignLabelDoubleWidth");


            GUI.enabled = true;

        GUILayout.EndHorizontal();

    }

    void drawPlaylistLevelGuiLeftMainbody()
    {

    }

    void drawPlaylistLevelGuiRightHalf()
    {

    }

    void drawCustomLevelButtons()
    {
        if (LevelUploadManager.Instance.GlobalStatus == LevelUploadManager.LevelUploadStatus.Uploading)
        {
            GUI.enabled = false;
            if (GUILayout.Button("continue", style.button))
            {
                SceneManager.Instance.loadScene(SceneManager.Scene.LevelSelect);
            }
            GUI.enabled = true;
        }
        else
        {
            if (GUILayout.Button("continue", style.button))
            {
                SceneManager.Instance.loadScene(SceneManager.Scene.LevelSelect);
            }
        }

        if (NetworkManager.Instance.GlobalStatus != NetworkManager.LoginStatus.LoggedIn)
        {
            GUI.enabled = false;
        }

        switch (LevelUploadManager.Instance.GlobalStatus)
        {
            case LevelUploadManager.LevelUploadStatus.ReadyForUpload:
                if (GUILayout.Button("Upload Level", style.button))
                {
                    LevelUploadManager.Instance.StartUploadLevel();
                }
                break;

            case LevelUploadManager.LevelUploadStatus.Uploading:
                GUILayout.Label("Uploading Level ..." + LevelUploadManager.Instance.UploadProgress, style.label);
                break;

            case LevelUploadManager.LevelUploadStatus.FinishedUploadSuccessful:
                GUILayout.Label("Upload Finished Successfully", style.label);
                break;

            case LevelUploadManager.LevelUploadStatus.FailedOnMD5Check:
                if (GUILayout.Button("Shoot yourself", style.button))
                {
                    SceneManager.Instance.loadScene(SceneManager.Scene.LevelSelect);
                }
                GUILayout.Label("little fucker.", style.label);
                break;

            case LevelUploadManager.LevelUploadStatus.FailedOnUpload:
                if (GUILayout.Button("Retry", style.button))
                {
                    LevelUploadManager.Instance.StartUploadLevel();
                }
                GUILayout.Label("Something went wrong while uploading!", style.label);
                break;

            case LevelUploadManager.LevelUploadStatus.LevelAlreadyAvailableOnline:
                GUILayout.Label("This level already got uploaded to the server", style.label);
                break;
        }
        GUI.enabled = true;
    }

    void drawStoryLevelButtons()
    {
        if (GUILayout.Button("continue", "ButtonDoubleWidth"))
        {
            SceneManager.Instance.loadScene(SceneManager.Scene.LevelSelect);
        }
    }

    void drawPlaylistLevelButtons()
    {

    }
}

using UnityEngine;
using System.Collections;
using System;

public class RateScreen : MonoBehaviour {

    public GUISkin style;

    private string levelHash_;

    private int pukeCount_;
    private int timeCount_;

    private int screenHeight;
    private int screenWidth;

    private int thumbtoShow = 0;

    private LevelLoader.LevelType leveltype;

    private bool rateloadingstarted;

    private int selectedRating, selectedDifficultyRating;

	// Use this for initialization
    void Start()
    {
        selectedRating = -1;
        selectedDifficultyRating = -1;

        levelHash_ = ScoreController.Instance.hash;
        pukeCount_ = ScoreController.Instance.pukeCount;
        timeCount_ = ScoreController.Instance.timeCount;
        Debug.Log(levelHash_);
        leveltype = SceneManager.Instance.levelToLoad.type;
        ScoreController.Instance.saveLocalPukeHighscore(levelHash_);
        ScoreController.Instance.saveLocalTimeHighScore(levelHash_);
        if (leveltype == LevelLoader.LevelType.Normal && NetworkManager.Instance.GlobalStatus == NetworkManager.LoginStatus.LoggedIn)
        {
            HighscoreAndRateManager.Instance.loadCurData(levelHash_);
            rateloadingstarted = true;
        }
        else
        {
            rateloadingstarted = false;
        }

        thumbtoShow = 0;
    }

    void OnGUI()
    {
        screenHeight = ForceAspectRatio.screenHeight;
        screenWidth = ForceAspectRatio.screenWidth;

        style.customStyles[0].fixedWidth = screenWidth;
        style.customStyles[0].fixedHeight = screenHeight;

        style.textField.fixedWidth = screenWidth / 2f;
        style.textField.fixedHeight = screenHeight *0.08f;
        style.textField.fontSize = (int)((screenHeight *0.08f) * 0.7);

        style.label.fontSize = (int)((screenHeight *0.08f) * 0.7);

        //Center Align Label
        style.customStyles[1].fontSize = (int)((screenHeight *0.08f) * 0.7f);
        style.customStyles[1].fixedWidth = screenWidth / 8;
        style.customStyles[1].fixedHeight = screenHeight / 8;

        //centerAlignDoubleWidth
        style.customStyles[2].fontSize = (int)((screenHeight *0.06f) * 0.9f);
        style.customStyles[2].fixedWidth = screenWidth / 2f;
        style.customStyles[2].fixedHeight = screenHeight * 0.06f;

        //double width button
        style.customStyles[3].fontSize = (int)((screenHeight *0.08f) * 0.9f);
        style.customStyles[3].fixedWidth = screenWidth / 2f;
        style.customStyles[3].fixedHeight = screenHeight *0.1f;

        //fontsize for login-button ...
        style.button.fontSize = (int)((screenHeight *0.08f) * 0.9f);
        style.button.fixedHeight = screenHeight / 10;
        style.button.fixedWidth = screenWidth / 4;

        //font for visit community button
        style.customStyles[5].fontSize = (int)((screenHeight *0.08f) * 0.6);
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

        //Buttonbox
        style.customStyles[8].fixedWidth = screenWidth;
        style.customStyles[8].fixedHeight = screenHeight * 0.1f;

        style.customStyles[9].fixedWidth = screenWidth / 2.5f;
        style.customStyles[9].fixedHeight = screenHeight * 0.4f;


        //fw b buttons > < 
        style.customStyles[10].fixedWidth = screenWidth *0.16f;
        style.customStyles[10].fixedHeight = screenHeight * 0.1f;
        style.customStyles[10].fontSize = (int)(screenHeight * 0.065f);

        //save screen button
        style.customStyles[11].fontSize = (int)((screenHeight *0.08f) * 0.7);
        style.customStyles[11].fixedWidth = screenWidth / 8;
        style.customStyles[11].fixedHeight = screenHeight / 8;

        //iconbox
        style.customStyles[12].fontSize = (int)((screenHeight *0.08f) * 0.7);
        style.customStyles[12].fixedWidth = screenWidth / 2 *0.08f;
        style.customStyles[12].fixedHeight = screenHeight / 2 *0.08f;

        //highlightsbox
        style.customStyles[13].fixedWidth = screenWidth / 2;
        style.customStyles[13].fixedHeight = screenHeight *0.6f;

        //ratingsbox
        style.customStyles[14].fixedWidth = screenWidth / 2;
        style.customStyles[14].fixedHeight = screenHeight *0.3f;

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
        drawCurScore();
    }
    void drawPlaylistLevelGuiLeftMainbody()
    {
        drawCurScore();
    }

    void drawStoryLevelGuiLeftMainbody()
    {
        drawCurScore();
    }

    void drawCurScore()
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
        drawHighlights();
    }

    void drawCustomLevelGuiRightHalf()
    {
        GUILayout.Label("Note: On upload, the currently shown Screen is taken as Thumbnail.", "CenterAlignLabelDoubleWidth");
        drawHighlights();
    }

    void drawPlaylistLevelGuiRightHalf()
    {
        drawHighlights();
        drawRatingBox();
    }

    private void drawRatingBox()
    {
        GUILayout.BeginVertical("ratingsbox");
            GUILayout.BeginHorizontal();

                GUILayout.FlexibleSpace();

                    GUILayout.BeginVertical();

                    switch (HighscoreAndRateManager.Instance.GlobalStatus)
                    {
                        case HighscoreAndRateManager.RatingStatus.Unloaded:
                            if (NetworkManager.Instance.GlobalStatus != NetworkManager.LoginStatus.LoggedIn)
                            {
                                GUILayout.Label("Log in for rating and highscore upload!", "CenterAlignLabelDoubleWidth");
                            }
                            else
                            {
                                GUILayout.Label("Please Wait ...", "CenterAlignLabelDoubleWidth");
                                if (!rateloadingstarted)
                                {
                                    rateloadingstarted = true;
                                    HighscoreAndRateManager.Instance.loadCurData(levelHash_);
                                }
                            }
                            break;

                        case HighscoreAndRateManager.RatingStatus.LevelNotFoundOnline:
                            GUILayout.Label("Level not found online!", "CenterAlignLabelDoubleWidth");
                            break;

                        case HighscoreAndRateManager.RatingStatus.Unrated:
                            GUILayout.Label("You can rate this level:", "CenterAlignLabelDoubleWidth");
                            GUILayout.Label("Fun & Look", "CenterAlignLabelDoubleWidth");
                            showRatingpukes(HighscoreAndRateManager.Instance.Rating,ref selectedRating);
                            GUILayout.Label("Difficulty", "CenterAlignLabelDoubleWidth");
                            showRatingpukes(HighscoreAndRateManager.Instance.DifficultyRating,ref selectedDifficultyRating);
                            break;

                        case HighscoreAndRateManager.RatingStatus.Uploading:
                        case HighscoreAndRateManager.RatingStatus.Uploaded:
                        case HighscoreAndRateManager.RatingStatus.Rated:
                            GUILayout.Label("You have already rated:", "CenterAlignLabelDoubleWidth");
                            GUILayout.Label("Fun & Look", "CenterAlignLabelDoubleWidth");
                            GUILayout.Label("<color=" + GlobalVars.Instance.OwnHighscoreHighlightColor + ">" + HighscoreAndRateManager.Instance.Rating.ToString() + "</color>", "CenterAlignLabelDoubleWidth");
                            GUILayout.Label("Difficulty", "CenterAlignLabelDoubleWidth");
                            GUILayout.Label("<color=" + GlobalVars.Instance.OwnHighscoreHighlightColor + ">" + HighscoreAndRateManager.Instance.DifficultyRating.ToString() + "</color>", "CenterAlignLabelDoubleWidth");
                            break;

                        case HighscoreAndRateManager.RatingStatus.LoadingCurData:
                            GUILayout.Label("Loading ratings", "CenterAlignLabelDoubleWidth");
                            break;
                    }

                    GUILayout.EndVertical();

                GUILayout.FlexibleSpace();

            GUILayout.EndHorizontal();

        GUILayout.EndVertical();
    }

    private void showRatingpukes(int p, ref int selrating)
    {
        GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
                int i = 0;
                for (; i < p; i++)
                {
                    if (i+1 == selrating)
                    {
                        GUILayout.Label("<color=" + GlobalVars.Instance.OwnHighscoreHighlightColor + ">" + (i + 1).ToString() + "</color>", "iconbox");
                    }
                    else
                    {
                        if (GUILayout.Button((i + 1).ToString(), "iconbox"))
                        {
                            selrating = i + 1;
                        }
                    }
                }
                for (; i < 10; i++)
                {
                    if (i+1 == selrating)
                    {
                        GUILayout.Label("<color=" + GlobalVars.Instance.OwnHighscoreHighlightColor + ">" + (i + 1).ToString() + "</color>", "iconbox");
                    }
                    else
                    {
                        if (GUILayout.Button((i + 1).ToString(), "iconbox"))
                        {
                            selrating = i + 1;
                        }
                    }
                }
            GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
    }

    void drawCustomLevelButtons()
    {
        if (LevelUploadManager.Instance.GlobalStatus == LevelUploadManager.LevelUploadStatus.Uploading)
        {
            GUI.enabled = false;
            if (GUILayout.Button("continue", "ButtonDoubleWidth"))
            {
                SceneManager.Instance.loadScene(SceneManager.Scene.LevelSelect);
            }
            GUI.enabled = true;
        }
        else
        {
            if (GUILayout.Button("continue", "ButtonDoubleWidth"))
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
                if (GUILayout.Button("Upload Level", "ButtonDoubleWidth"))
                {
                    LevelUploadManager.Instance.StartUploadLevel(ScreenShotManager.Instance.getScreenShot(thumbtoShow));
                }
                break;

            case LevelUploadManager.LevelUploadStatus.Uploading:
                GUILayout.Label("Uploading Level ..." + LevelUploadManager.Instance.UploadProgress, "ButtonDoubleWidth");
                break;

            case LevelUploadManager.LevelUploadStatus.FinishedUploadSuccessful:
                GUILayout.Label("Upload Finished Successfully", "ButtonDoubleWidth");
                ScreenShotManager.Instance.saveScreenshot(thumbtoShow);
                break;

            case LevelUploadManager.LevelUploadStatus.FailedOnMD5Check:
                if (GUILayout.Button("Shoot yourself", "ButtonDoubleWidth"))
                {
                    SceneManager.Instance.loadScene(SceneManager.Scene.LevelSelect);
                }
                GUILayout.Label("little fucker.", "ButtonDoubleWidth");
                break;

            case LevelUploadManager.LevelUploadStatus.FailedOnUpload:
                if (GUILayout.Button("Retry", "ButtonDoubleWidth"))
                {
                    LevelUploadManager.Instance.StartUploadLevel(ScreenShotManager.Instance.getScreenShot(thumbtoShow));
                }
                GUILayout.Label("Something went wrong!", "ButtonDoubleWidth");
                break;

            case LevelUploadManager.LevelUploadStatus.LevelAlreadyAvailableOnline:
                GUILayout.Label("Level already online!", "ButtonDoubleWidth");
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
        if (HighscoreAndRateManager.Instance.GlobalStatus == HighscoreAndRateManager.RatingStatus.Uploading) GUI.enabled = false;
        if (GUILayout.Button("continue", "ButtonDoubleWidth"))
        {
            SceneManager.Instance.loadScene(SceneManager.Scene.LevelSelect);
        }
        GUI.enabled = true;
        if (NetworkManager.Instance.GlobalStatus == NetworkManager.LoginStatus.LoggedIn)
        {
            switch (HighscoreAndRateManager.Instance.GlobalStatus)
            {
                case HighscoreAndRateManager.RatingStatus.Unrated:
                    if (selectedRating == -1 || selectedDifficultyRating == -1)
                    {
                        GUILayout.Label("Rate before uploading score", "ButtonDoubleWidth");
                    }
                    else
                    {
                        if (GUILayout.Button("Upload ratings & scores", "ButtonDoubleWidth"))
                        {
                            HighscoreAndRateManager.Instance.SetNewData(timeCount_, pukeCount_, selectedRating, selectedDifficultyRating);
                        }
                    }
                    GUI.enabled = true;
                    break;

                case HighscoreAndRateManager.RatingStatus.Rated:
                    if (GUILayout.Button("Upload scores", "ButtonDoubleWidth"))
                    {
                        HighscoreAndRateManager.Instance.SetNewData(timeCount_, pukeCount_);
                    }
                    break;

                case HighscoreAndRateManager.RatingStatus.Uploading:
                    GUILayout.Label("Uploading ...", "ButtonDoubleWidth");
                    break;

                case HighscoreAndRateManager.RatingStatus.Uploaded:
                    GUILayout.Label("Uploaded", "ButtonDoubleWidth");
                    break;
            }
        }
    }

    int nfmod(int a, int b)
    {
        return (int)(a - b * Math.Floor((float)a / (float)b));
    }

    void drawHighlights()
    {
        GUILayout.BeginVertical("highlightsbox");
            GUILayout.Label("Highlights", "CenterAlignLabelDoubleWidth");
            GUILayout.BeginHorizontal();

            GUILayout.FlexibleSpace();

            GUILayout.Box(ScreenShotManager.Instance.getScreenShot(thumbtoShow), "ScreensBox");

            GUILayout.FlexibleSpace();

            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();

            if (ScreenShotManager.Instance.screenshotcount <= 1)
            {
                GUI.enabled = false;
            }

            if (GUILayout.Button("<", "ForwardBackwardButton")) thumbtoShow = nfmod(thumbtoShow -1, ScreenShotManager.Instance.screenshotcount);

            GUILayout.FlexibleSpace();

            if(ScreenShotManager.Instance.isThumbnail(thumbtoShow))
                GUILayout.Label("Thumbnail", "CenterAlignLabel");
            else
            {
                if(ScreenShotManager.Instance.GotThumbAlreadySaved(thumbtoShow))
                {
                    GUILayout.Label("saved", "CenterAlignLabel");
                }else{
                    if (GUILayout.Button("Save screen", "SaveScreenButton"))
                    {
                        ScreenShotManager.Instance.saveScreenshot(thumbtoShow, ScreenShotManager.Instance.getScreenShotFolderPath());
                    }
                }
            }

            GUILayout.FlexibleSpace();

            if (GUILayout.Button(">", "ForwardBackwardButton")) thumbtoShow = nfmod(thumbtoShow + 1, ScreenShotManager.Instance.screenshotcount);

            GUI.enabled = true;

            GUILayout.EndHorizontal();
        GUILayout.EndVertical();
    }

}

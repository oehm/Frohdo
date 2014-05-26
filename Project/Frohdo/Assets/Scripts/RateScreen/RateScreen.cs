using UnityEngine;
using System.Collections;

public class RateScreen : MonoBehaviour {

    public GUISkin skin;
    
    private string levelHash_;

    private int pukeCount_;
    private int timeCount_;

	// Use this for initialization
	void Start () {
        levelHash_ = ScoreController.Instance.LevelHash;
        pukeCount_ = ScoreController.Instance.pukeCount;
        timeCount_ = ScoreController.Instance.timeCount;
	}


    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(ForceAspectRatio.xOffset,ForceAspectRatio.yOffset,ForceAspectRatio.screenWidth,ForceAspectRatio.screenHeight));
        GUILayout.BeginVertical();
            GUILayout.Label("LevelHash:",skin.customStyles[0]);
            GUILayout.Label(levelHash_,skin.label);
            //GUILayout.Label("Levelname:", skin.customStyles[0]);
            //GUILayout.Label(SceneManager.Instance.levelToLoad.LevelTitle, skin.label);
            //GUILayout.Label("LevelPath:", skin.customStyles[0]);
            //GUILayout.Label(SceneManager.Instance.levelToLoad.LeveltoLoad, skin.label);
            GUILayout.Space(30);
            GUILayout.BeginHorizontal();
                GUILayout.BeginVertical();
                    GUILayout.Label("Pukes:", skin.customStyles[0]);
                    GUILayout.Label(pukeCount_.ToString(), skin.label);
                GUILayout.EndVertical();
                GUILayout.BeginVertical();    
                    GUILayout.Label("Time in Millisecs:", skin.customStyles[0]);
                    GUILayout.Label(timeCount_.ToString(), skin.label);
                GUILayout.EndVertical();    
            GUILayout.EndHorizontal();
            GUILayout.Space(50);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("continue",skin.button))
            {
                SceneManager.Instance.loadScene(SceneManager.Scene.LevelSelect);
            }
            if (SceneManager.Instance.levelToLoad.type == LevelLoader.LevelType.Custom)
            {
                if (NetworkManager.Instance.GlobalStatus == NetworkManager.LoginStatus.LoggedIn)
                {
                    if (GUILayout.Button("Upload Level", skin.button))
                    {
                        LevelUploadManager.Instance.StartUploadLevel();
                    }
                }
                else
                {
                    GUILayout.Label("You have to be logged in in order to upload a level!", skin.customStyles[0]);
                }
            }
            GUILayout.EndHorizontal();

        GUILayout.EndVertical();
        GUILayout.EndArea();
    }

	// Update is called once per frame
	void Update () {
	
	}
}

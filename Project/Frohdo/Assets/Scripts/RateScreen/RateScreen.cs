using UnityEngine;
using System.Collections;

public class RateScreen : MonoBehaviour {

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
        GUILayout.BeginVertical();
            GUILayout.Label("LevelHash:");
            GUILayout.Label(levelHash_);
            GUILayout.Label("");
            GUILayout.Label("Pukes:");
            GUILayout.Label(pukeCount_.ToString());
            GUILayout.Label("");
            GUILayout.Label("Time in Millisecs:");
            GUILayout.Label(timeCount_.ToString());

            if (GUILayout.Button("continue"))
            {
                SceneManager.Instance.loadScene(SceneManager.Scene.LevelSelect);
            }

        GUILayout.EndVertical();
    }

	// Update is called once per frame
	void Update () {
	
	}
}

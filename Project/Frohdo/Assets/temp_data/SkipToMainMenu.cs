using UnityEngine;
using System.Collections;

public class SkipToMainMenu : MonoBehaviour {
    public SceneDestroyer destroyer;

	// Use this for initialization
	void Start () {
        SceneManager.Instance.loadScene(SceneManager.Scene.MainMenu);

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

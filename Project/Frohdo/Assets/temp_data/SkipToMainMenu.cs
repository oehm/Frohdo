using UnityEngine;
using System.Collections;

public class SkipToMainMenu : MonoBehaviour {
    public SceneDestroyer destroyer;

	// Use this for initialization
	void Start () {
        SceneController.Instance.loadScene(destroyer, 2);

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

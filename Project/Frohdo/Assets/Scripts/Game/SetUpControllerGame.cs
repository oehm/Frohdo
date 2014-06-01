using UnityEngine;
using System.Collections;

public class SetUpControllerGame : MonoBehaviour {

    public LevelLoader levelLoader_;

	// Use this for initialization
	void Start () {
        levelLoader_.load();
	}

}

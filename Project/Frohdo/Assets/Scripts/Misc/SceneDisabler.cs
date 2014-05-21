using UnityEngine;
using System.Collections;

public class SceneDisabler : MonoBehaviour {

	// Use this for initialization
    public GameObject[] sceneObjs;
    public void disable()
    {
        foreach (GameObject g in sceneObjs)
        {
            g.SetActive(false);
        }
    }

    public void enable()
    {
        foreach (GameObject g in sceneObjs)
        {
            g.SetActive(true);
        }
    }
}

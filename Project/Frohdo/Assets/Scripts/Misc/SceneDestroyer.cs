using UnityEngine;
using System.Collections;

public class SceneDestroyer : MonoBehaviour
{
    public GameObject[] sceneObjs;
    public void suicide()
    {
        foreach (GameObject g in sceneObjs)
        {
            Debug.Log("Dest: " + g);
            Destroy(g);
        }
        Destroy(gameObject);
    }
}


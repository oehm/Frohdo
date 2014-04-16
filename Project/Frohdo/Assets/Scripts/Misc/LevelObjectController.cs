using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelObjectController : MonoBehaviour {

    //public
    public List<GameObject> levelObjectPrefabs_;

    public GameObject GetPrefabByName(string name)
    {
        foreach (GameObject levelObjectPrefab in levelObjectPrefabs_)
        {
            if (levelObjectPrefab.name.Equals(name))
            {
                return levelObjectPrefab;
            }
        }

        Debug.Log("LevelObject prefab not found: " + name);
        throw new System.Exception("LevelObject prefab not found: " + name);
    }
}

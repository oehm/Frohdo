using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//use as a Singleton! always use the static instance and dont try to create one
[System.Serializable]
public class LevelObjectController : ScriptableObject {

    private static LevelObjectController instance = null;

    public static LevelObjectController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = (LevelObjectController)Resources.Load("ScriptableObjectInstances/LevelObjectControllerInstance");
                DontDestroyOnLoad(instance); 

                //Debug.Log("No instance of LevelObjectController found. Have you forgotten to instantiate it?");
                //throw new System.Exception("No instance of LevelObjectController found. Have you forgotten to instantiate it?");
            }

            return instance;
        }
    }

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

    public Color GetColor(string color)
    {
        if (color.Equals("W") == true) return W;
        if (color.Equals("R") == true) return R;
        if (color.Equals("G") == true) return G;
        if (color.Equals("B") == true) return B;
        if (color.Equals("Y") == true) return Y;
        if (color.Equals("C") == true) return C;
        if (color.Equals("M") == true) return M;

        Debug.Log("Color not found: " + color);
        throw new System.Exception("Color not found: " + color);
    }

    //color definitions
    public readonly static Color W = new Color(1.0f, 1.0f, 1.0f);
    public readonly static Color R = new Color(1.0f, 0.0f, 0.0f);
    public readonly static Color G = new Color(0.0f, 1.0f, 0.0f);
    public readonly static Color B = new Color(0.0f, 0.0f, 1.0f);
    public readonly static Color Y = new Color(1.0f, 1.0f, 0.0f);
    public readonly static Color C = new Color(0.0f, 1.0f, 1.0f);
    public readonly static Color M = new Color(1.0f, 0.0f, 1.0f);

    void OnEnable()
    {
        if (instance != null)
        {
            Destroy(this);

            Debug.Log("There is a instance of LevelObjectController already. Cant create Another one.");
            throw new System.Exception("There is a instance of LevelObjectController already. Cant create Another one.");
        }
    }
}

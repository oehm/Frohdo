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
    public static Color W = new Color(1.0f, 1.0f, 1.0f);
    public static Color R = new Color(1.0f, 0.0f, 0.0f);
    public static Color G = new Color(0.0f, 1.0f, 0.0f);
    public static Color B = new Color(0.0f, 0.0f, 1.0f);
    public static Color Y = new Color(1.0f, 1.0f, 0.0f);
    public static Color C = new Color(0.0f, 1.0f, 1.0f);
    public static Color M = new Color(1.0f, 0.0f, 1.0f);

    
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//use as a Singleton! always use the static instance and dont try to create one
public class LevelObjectController : MonoBehaviour 
{

    private static LevelObjectController instance = null;

    public static LevelObjectController Instance
    {
        get
        {
            return instance;
        }
    }

    void Start()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        instance = this;
    }

    //public
    public List<GameObject> levelObjectPrefabs_;

    public string[] getColors()
    {
        string[] colors = new string[7];
        colors[0] = "W";
        colors[1] = "R";
        colors[2] = "G";
        colors[3] = "B";
        colors[4] = "Y";
        colors[5] = "C";
        colors[6] = "M";

        return colors;
    }

    public List<GameObject> GetPrefabsByAvailability(int layer, bool editor)
    {
        List<GameObject> list = new List<GameObject>();

        foreach (GameObject levelObjectPrefab in levelObjectPrefabs_)
        {
            Gridable[] gridables = levelObjectPrefab.GetComponentsInChildren<Gridable>(true);
            //i assume here that every level object has only 1 gridable attached
            Gridable gridable = gridables[0];

            if (gridable.availableInLayer[layer])
            {
                if (editor && gridable.editorVersion != null)
                {
                    list.Add(gridable.editorVersion);
                }
                else
                {
                    list.Add(levelObjectPrefab);
                }
            }
        }
        return list;
    }

    public GameObject GetPrefabByName(string name, int layer, bool editor)
    {
        foreach (GameObject levelObjectPrefab in levelObjectPrefabs_)
        {
            Gridable[] gridables = levelObjectPrefab.GetComponentsInChildren<Gridable>(true);
            //i assume here that every level object has only 1 gridable attached
            Gridable gridable = gridables[0];

            if (levelObjectPrefab.name.Equals(name) && gridable.availableInLayer[layer])
            {
                if (editor && gridable.editorVersion != null)
                {
                    GameObject character = gridable.editorVersion;
                    character.name = levelObjectPrefab.name;
                    return character;
                }

                return levelObjectPrefab;
            }
        }

        Debug.Log("LevelObject prefab not found: " + name);
        throw new System.Exception("LevelObject prefab not found: " + name);
    }


    public GameObject GetPrefabByName(string name)
    {
        foreach (GameObject levelObjectPrefab in levelObjectPrefabs_)
        {
            Gridable[] gridables = levelObjectPrefab.GetComponentsInChildren<Gridable>(true);
            //i assume here that every level object has only 1 gridable attached
            Gridable gridable = gridables[0];

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

    public string GetMixColor(string color1, string color2)
    {
        string all = "RMBCGY";

        int idx1 = all.IndexOf(color1);
        int idx2 = all.IndexOf(color2);

        if (color1.Equals("W") || color2.Equals("W") || color1.Equals(color2) ||
            idx2 == capIdx(idx1 + 1) || idx2 == capIdx(idx1 - 1) )
            return color2;

        if (idx2 == capIdx(idx1 + 2))
            return all[capIdx(idx1 + 1)].ToString();

        if (idx2 == capIdx(idx1 - 2))
            return all[capIdx(idx1 - 1)].ToString();
        
        return "W";

    }

    int capIdx(int idx)
    {
        return (6 + (idx % 6)) % 6;
    }

    //color definitions
    public readonly static Color W = new Color(0.93f, 0.94f, 0.95f);
    public readonly static Color R = new Color(0.91f, 0.30f, 0.24f);
    public readonly static Color G = new Color(0.18f, 0.80f, 0.44f);
    public readonly static Color B = new Color(0.20f, 0.60f, 0.86f);
    public readonly static Color Y = new Color(0.95f, 0.77f, 0.06f);
    public readonly static Color C = new Color(0.01f, 0.96f, 0.95f);
    public readonly static Color M = new Color(0.61f, 0.35f, 0.71f);

}
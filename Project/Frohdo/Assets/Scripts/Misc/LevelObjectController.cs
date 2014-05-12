﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//use as a Singleton! always use the static instance and dont try to create one
public class LevelObjectController : ScriptableObject 
{

    private static LevelObjectController instance = null;

    public static LevelObjectController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = (LevelObjectController)Resources.Load("ScriptableObjectInstances/LevelObjectControllerInstance");
                DontDestroyOnLoad(instance); 
            }

            return instance;
        }
    }

    void OnEnable()
    {
        if (instance != null)
        {
            Destroy(this);

            Debug.Log("There is a instance of LevelObjectController already. Cant create Another one.");
            throw new System.Exception("There is a instance of LevelObjectController already. Cant create Another one.");
        }
    }

    //public
    public List<GameObject> levelObjectPrefabs_;
    public GameObject character_;

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

    public GameObject GetPrefabByName(string name)
    {
        foreach (GameObject levelObjectPrefab in levelObjectPrefabs_)
        {
            if (levelObjectPrefab.name.Equals(name))
            {
                return levelObjectPrefab;
            }
        }

        if (name.Equals("Character"))
        {
            return character_;
        }

        Debug.Log("LevelObject prefab not found: " + name);
        throw new System.Exception("LevelObject prefab not found: " + name);
    }
    public GameObject getCharacter()
    {
        return character_;
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
    public readonly static Color W = new Color(1.0f, 1.0f, 1.0f);
    public readonly static Color R = new Color(1.0f, 0.0f, 0.0f);
    public readonly static Color G = new Color(0.0f, 1.0f, 0.0f);
    public readonly static Color B = new Color(0.0f, 0.0f, 1.0f);
    public readonly static Color Y = new Color(1.0f, 1.0f, 0.0f);
    public readonly static Color C = new Color(0.0f, 1.0f, 1.0f);
    public readonly static Color M = new Color(1.0f, 0.0f, 1.0f);

}

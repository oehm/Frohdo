using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GlobalVars : ScriptableObject
{
    private static GlobalVars instance = null;

    public static GlobalVars Instance
    {
        get
        {
            if (instance == null)
            {
                instance = (GlobalVars)Resources.Load("ScriptableObjectInstances/GlobalVarsInstance");
                DontDestroyOnLoad(instance);
            }

            return instance;
        }
    }


    public float animationTime;

    public float mainCamerZ;

    public int LayerCount
    {
        get
        {
            if (layerZPos.Count != layerParallax.Count)
            {
                Debug.Log("Somethings wrong with the layer configuration.");
                throw new System.Exception("Somethings wrong with the layer configuration.");
            }
            return layerZPos.Count;
        }
    }

    public List<float> layerZPos;
    public List<Vector2> layerParallax;


    public int playLayer;

    public Vector2 maxLevelSize;

    public int maxRatios;

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

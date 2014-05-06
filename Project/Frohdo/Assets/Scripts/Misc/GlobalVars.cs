using UnityEngine;
using System.Collections;

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


    public int numberofLayers = 5;

    public float mainCamerZ = -10.0f;

    public float layer1Z = -5.0f;
    public float layer2Z = -2.5f;
    public float layer3Z = 0.0f;
    public float layer4Z = 2.5f;
    public float layer5Z = 5.0f;

    public Vector2 parralax1 = new Vector2(-0.5f, -0.01f);
    public Vector2 parralax2 = new Vector2(0, 0);
    public Vector2 parralax4 = new Vector2(0, 0);
    public Vector2 parralax5 = new Vector2(0.5f, 0);

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

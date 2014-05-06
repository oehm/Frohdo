using UnityEngine;
using UnityEditor;

public class MenuItems : MonoBehaviour {

    [MenuItem("Assets/Create/LevelObjectController")]

    public static void CreateLevelObjectController()
    {
        ScriptableObjectUtility.CreateAsset<LevelObjectController>();
    }

    [MenuItem("Assets/Create/GlobalVars")]

    public static void CreateGlobalVars()
    {
        ScriptableObjectUtility.CreateAsset<GlobalVars>();
    }
}

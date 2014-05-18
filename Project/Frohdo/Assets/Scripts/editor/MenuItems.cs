using UnityEngine;
using UnityEditor;

public class MenuItems : MonoBehaviour {

    [MenuItem("Assets/Create/GlobalVars")]

    public static void CreateGlobalVars()
    {
        ScriptableObjectUtility.CreateAsset<GlobalVars>();
    }
}

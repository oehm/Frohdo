using UnityEngine;
using UnityEditor;

public class LevelObjectControllerAsset : MonoBehaviour {

    [MenuItem("Assets/Create/LevelObjectController")]

    public static void CreateAsset()
    {
        ScriptableObjectUtility.CreateAsset<LevelObjectController>();
    }
}

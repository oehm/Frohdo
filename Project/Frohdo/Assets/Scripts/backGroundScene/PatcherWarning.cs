using UnityEngine;
using System.Collections;

public class PatcherWarning : MonoBehaviour {


    public GUISkin style;

    void OnGUI()
    {
        GUI.skin = style;
        if (!UnityEngine.Debug.isDebugBuild && GlobalVars.Instance.PreventPatcherInBuild == true)
        {
            GUILayout.Label("WARNING: PATCHER DISABLED IN THIS BUILD!!!!!!");
        }
    }
}

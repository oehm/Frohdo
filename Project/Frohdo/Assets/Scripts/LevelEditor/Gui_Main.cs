using UnityEngine;
using System.Collections;

public class Gui_Main : MonoBehaviour {

    public int leftAreaWidth;
    public GUIStyle leftAreaStyle;
    public GUIStyle toolTipStyle;

    public Texture testTex;

    private string levelName = "Enter level name";
    
    private int activeLayer = 1;
    private string[] activeLayerStings;
    private bool[] visibleLayer;
    private Vector2 scrollPos;
    private int selectedLevelobject = 0;
    private GUIContent[] levelObjects_content;

    // Use this for initialization
	void Start () {
        activeLayerStings = new string[GlobalVars.numberofLayers];
        visibleLayer = new bool[GlobalVars.numberofLayers];
        scrollPos = new Vector2(0, 0);
        for (int i = 0; i < GlobalVars.numberofLayers; i++)
        {
            activeLayerStings[i] = (i+1).ToString();
            visibleLayer[i] = true;
        }
        levelObjects_content = new GUIContent[20];

        for(int i=0; i<levelObjects_content.Length; i++){
            levelObjects_content[i] = new GUIContent(testTex,"LevelItem "+i.ToString());
        }
	}
	
	// Update is called once per frame
	void Update () {
        //set the culing mask for main camera
        int cullingMask=0;
        cullingMask += 1 << 0; //default
        cullingMask += 1 << 1; //transparentFX
        cullingMask += 1 << 2; //ignore raycast

        for (int i = 0; i < visibleLayer.Length; i++)
        {
            if (visibleLayer[i]) cullingMask += 1 << i + 8; //layer[i]_mask
        }
        Camera.main.cullingMask = cullingMask;
	}

    void OnGUI ()
    {
        //left Area
        GUILayout.BeginArea(new Rect(0,0,leftAreaWidth,Screen.height),leftAreaStyle);
        levelName = GUILayout.TextField(levelName);
        GUILayout.Label("ACTIVE LAYER");
        activeLayer = GUILayout.Toolbar(activeLayer, activeLayerStings);
        GUILayout.Label("VIVIBLE LAYER");
        GUILayout.BeginHorizontal();
            for (int i = 0; i < visibleLayer.Length; i++)
            {
                visibleLayer[i] = GUILayout.Toggle(visibleLayer[i], (i + 1).ToString());
            }
        GUILayout.EndHorizontal();
        GUILayout.Label("LEVELOBJECT");
        scrollPos = GUILayout.BeginScrollView(scrollPos);
        selectedLevelobject = GUILayout.SelectionGrid(selectedLevelobject, levelObjects_content, 2,GUILayout.Width(leftAreaWidth-24));
        GUILayout.EndScrollView();
        GUILayout.Button("PLAY");
        GUILayout.EndArea();

        GUI.Label(new Rect(leftAreaWidth+10, Screen.height - Input.mousePosition.y, 100, 20), GUI.tooltip, toolTipStyle);
    }
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Gui_Main : MonoBehaviour
{
    private delegate void MenuDelegate();
    private MenuDelegate menuFunction;

    public LevelEditorParser level;
    public EditorObjectPlacement objPlacement;

    public RenderGameObjectToTexture objRenderer;

    public static int leftAreaWidth = 300;

    public GUISkin guiSkin;

    private string levelName = "Enter level name";
    public Vector2 levelSize = new Vector2(100, 100);

    private int activeLayer = 1;
    private string[] activeLayerStings;
    private bool[] visibleLayer;
    private Vector2 scrollPos = new Vector2(0, 0);
    private int selectedLevelobject = -1;
    private GUIContent[] levelObjects_content;

    private GameObject[] levelObjects;

    // Use this for initialization
    void Start()
    {
        level.setSize(levelSize);
        level.setLevelBackground("blue");
        //LevelObject charakter = new LevelObject();
        //charakter.color = "white";
        //charakter.name = "chrakter";
        //charakter.pos = new SerializableVector2(new Vector2(10,10));
        //level.addLevelObject(2,charakter);
        menuFunction = setup;

        activeLayerStings = new string[GlobalVars.numberofLayers];
        visibleLayer = new bool[GlobalVars.numberofLayers];
        for (int i = 0; i < GlobalVars.numberofLayers; i++)
        {
            activeLayerStings[i] = (i + 1).ToString();
            visibleLayer[i] = true;
        }

        List<GUIContent> tempCont = new List<GUIContent>();
        List<GameObject> tempObjs = new List<GameObject>();
        for (int i = 0; i < LevelObjectController.Instance.levelObjectPrefabs_.Count; i++)
        {
            GameObject obj = LevelObjectController.Instance.levelObjectPrefabs_[i];
            if (obj.name == "Character")
            {
                continue;
            }
            tempCont.Add(new GUIContent(objRenderer.renderGameObjectToTexture(obj, 256, 256), obj.name));
            tempObjs.Add(LevelObjectController.Instance.levelObjectPrefabs_[i]);
        }
        levelObjects_content = tempCont.ToArray();
        levelObjects = tempObjs.ToArray();
    }

    // Update is called once per frame
    void Update()
    {
        //set the culing mask for main camera
        int cullingMask = 0;
        cullingMask += 1 << 0; //default
        cullingMask += 1 << 1; //transparentFX
        cullingMask += 1 << 2; //ignore raycast

        for (int i = 0; i < visibleLayer.Length; i++)
        {
            if (visibleLayer[i]) cullingMask += 1 << i + 8; //layer[i]_mask
            if (i == activeLayer)
            {
                if (visibleLayer[i]) cullingMask += 1 << i + 14; //for viewing grid
            }
        }
        Camera.main.cullingMask = cullingMask;
    }

    void OnGUI()
    {
        menuFunction();
    }
    void setup()
    {
        GUILayout.BeginArea(new Rect(ForceAspectRatio.xOffset, ForceAspectRatio.yOffset, leftAreaWidth, ForceAspectRatio.screenHeight));
        levelName = GUILayout.TextField(levelName, guiSkin.textField);
        if (GUILayout.Button("Next", guiSkin.button))
        {
            objPlacement.init(levelSize);
            level.setLevelName(levelName);
            menuFunction = edit;
        }
        GUILayout.EndArea();
    }

    void edit()
    {
        GUILayout.BeginArea(new Rect(ForceAspectRatio.xOffset, ForceAspectRatio.yOffset, leftAreaWidth, ForceAspectRatio.screenHeight));
        GUILayout.Label("ACTIVE LAYER");
        
        int oldActive = activeLayer;
        activeLayer = GUILayout.Toolbar(activeLayer, activeLayerStings);
        if(oldActive != activeLayer)
        {
            objPlacement.setActiveLayer(activeLayer);
        }
        
        GUILayout.Label("VIVIBLE LAYER", guiSkin.label);
        GUILayout.BeginHorizontal();
        for (int i = 0; i < visibleLayer.Length; i++)
        {
            visibleLayer[i] = GUILayout.Toggle(visibleLayer[i], (i + 1).ToString());
        }
        GUILayout.EndHorizontal();
        GUILayout.Label("LEVELOBJECT");
        scrollPos = GUILayout.BeginScrollView(scrollPos, guiSkin.scrollView);
        int oldSelected = selectedLevelobject;
        selectedLevelobject = GUILayout.SelectionGrid(selectedLevelobject, levelObjects_content, 2, guiSkin.customStyles[0]);
        if (oldSelected != selectedLevelobject)
        {
            Debug.Log("place item " + selectedLevelobject);
            objPlacement.selectObject(levelObjects[selectedLevelobject]);
        }
        GUILayout.EndScrollView();
        if (GUILayout.Button("Save"))
        {
            level.saveLevel();
        }
        GUILayout.Button("PLAY");
        GUILayout.EndArea();

        GUI.Label(new Rect(leftAreaWidth + 10, Screen.height - Input.mousePosition.y, 100, 20), GUI.tooltip, guiSkin.label);
    }

    public static bool isMouseOnGui(Vector2 pos)
    {
        return pos.x > leftAreaWidth;
    }
}
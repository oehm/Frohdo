using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Gui_Main : MonoBehaviour
{
    private delegate void MenuDelegate();
    private MenuDelegate menuFunction;

    public LevelEditorParser level;
    public EditorObjectPlacement objPlacement;
    public EditCommandManager commandManger;

    public RenderGameObjectToTexture objRenderer;

    public static int leftAreaWidth = 300;

    public GUISkin guiSkin;

    private string levelName = "Enter level name";
    public Vector2 levelSize = new Vector2(100, 100);

    private int activeLayer = 2;
    private string[] activeLayerStings;

    private bool[] visibleLayer;
    private GUIContent eye;

    private int selectedColor = 0;
    private GUIContent[] colorButtons;

    private Vector2 scrollPos = new Vector2(0, 0);

    private GUIContent[][] levelObjects_content;
    private GUIContent character;
    private string[] colors;
    private int seletedObj = -1;
    private bool showEditScreen = false;


    private GameObject charObj;
    // Use this for initialization
    void Start()
    {
        level.setSize(levelSize);
        level.setLevelBackground("blue");
        menuFunction = edit;

        objPlacement.setActiveLayer(activeLayer);
        objPlacement.init(levelSize);
        level.setLevelName(levelName);

        colors = LevelObjectController.Instance.getColors();
        colorButtons = new GUIContent[colors.Length];
        for (int i = 0; i < colors.Length; i++)
        {
            Texture2D tex = new Texture2D(30, 30);
            Color[] c = tex.GetPixels();
            for (int j = 0; j < c.Length; j++) c[j] = LevelObjectController.Instance.GetColor(colors[i]);
            tex.SetPixels(c);
            tex.Apply();
            colorButtons[i] = new GUIContent(tex);
        }

        activeLayerStings = new string[GlobalVars.Instance.LayerCount];
        visibleLayer = new bool[GlobalVars.Instance.LayerCount];
        eye = new GUIContent("");
        for (int i = 0; i < GlobalVars.Instance.LayerCount; i++)
        {
            activeLayerStings[i] = (i + 1).ToString();
            visibleLayer[i] = true;
        }

        List<GUIContent> tempCont = new List<GUIContent>();
        List<GameObject> tempObjs = new List<GameObject>();

        levelObjects_content = new GUIContent[colors.Length][];

        for (int i = 0; i < LevelObjectController.Instance.levelObjectPrefabs_.Count; i++)
        {
            tempObjs.Add(LevelObjectController.Instance.levelObjectPrefabs_[i]);
        }

        for (int i = 0; i < colors.Length; i++)
        {
            for (int o = 0; o < tempObjs.Count; o++)
            {
                tempCont.Add(new GUIContent(objRenderer.renderGameObjectToTexture(tempObjs[o], 256, 256, colors[i]), tempObjs[o].name));
            }
            levelObjects_content[i] = tempCont.ToArray();
            tempCont.Clear();
        }
        charObj = LevelObjectController.Instance.getCharacter();
        character = new GUIContent(objRenderer.renderGameObjectToTexture(charObj, 256, 256, null));
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
                cullingMask += 1 << i + 14; //for viewing grid
            }
        }
        Camera.main.cullingMask = cullingMask;
    }

    void OnGUI()
    {
        menuFunction();
    }

    void edit()
    {
        GUILayout.BeginArea(new Rect(ForceAspectRatio.xOffset, ForceAspectRatio.yOffset, leftAreaWidth, ForceAspectRatio.screenHeight));//,guiSkin.customStyles[3]);
        GUILayout.Label("LAYER", guiSkin.label);

        //Active LAyer
        int oldActive = activeLayer;
        activeLayer = GUILayout.Toolbar(activeLayer, activeLayerStings, guiSkin.button);
        visibleLayer[activeLayer] = true;
        if (oldActive != activeLayer)
        {
            objPlacement.setActiveLayer(activeLayer);
        }

        //Visible LAyer
        GUILayout.BeginHorizontal();
        for (int i = 0; i < visibleLayer.Length; i++)
        {
            visibleLayer[i] = GUILayout.Toggle(visibleLayer[i], eye, guiSkin.customStyles[2]);
        }
        GUILayout.EndHorizontal();

        //Colors
        int oldSelectedColor = selectedColor;
        selectedColor = GUILayout.Toolbar(selectedColor, colorButtons, guiSkin.customStyles[1]);
        if (oldSelectedColor != selectedColor)
        {
            objPlacement.updateColor(colors[selectedColor]);
        }

        //LevelObjects
        scrollPos = GUILayout.BeginScrollView(scrollPos, guiSkin.scrollView);
        int xCount = 0;
        GUILayout.BeginHorizontal("");
        if (activeLayer == GlobalVars.Instance.playLayer)
        {
            xCount++;
            if (GUILayout.Button(character, guiSkin.customStyles[0]))
            {
                LevelObjectXML obj = new LevelObjectXML();
                obj.name = charObj.name;
                obj.color = colors[selectedColor];
                objPlacement.updateObject(obj);
                seletedObj = 0;
            }

        }
        for (int i = 0; i < levelObjects_content[selectedColor].Length; i++)
        {
            if (xCount >= 2)
            {
                xCount = 0;
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
            }
            xCount++;
            if (GUILayout.Button(levelObjects_content[selectedColor][i], guiSkin.customStyles[0]))
            {
                LevelObjectXML obj = new LevelObjectXML();
                obj.name = levelObjects_content[selectedColor][i].tooltip;
                obj.color = colors[selectedColor];
                objPlacement.updateObject(obj);
                seletedObj = i;
            }
        }
        GUILayout.EndHorizontal();

        GUILayout.EndScrollView();
        GUILayout.BeginHorizontal("box");
        if (GUILayout.Button("Save", guiSkin.button))
        {
            menuFunction = save;
        }
        if (GUILayout.Button("PREVIEW", guiSkin.button))
        {
            //TEST THE LEVEL
        }
        GUILayout.EndHorizontal();

        GUILayout.EndArea();


        //Undo Redo Selected
        GUILayout.BeginArea(new Rect(ForceAspectRatio.screenWidth - 100, ForceAspectRatio.yOffset, 100, 150));//, guiSkin.window);
        if (GUILayout.Button("Undo", guiSkin.button))
        {
            commandManger.undo();
        }

        if (GUILayout.Button("Redo", guiSkin.button))
        {
            commandManger.redo();
        }
        //GUILayout.Label("Selected Object", guiSkin.label);
        if (seletedObj >= 0)
        {
            GUILayout.Box(levelObjects_content[selectedColor][seletedObj], guiSkin.box);
        }
        GUILayout.EndArea();

        if (showEditScreen)
        {
            GUILayout.BeginArea(new Rect(ForceAspectRatio.screenWidth - 400, ForceAspectRatio.screenHeight - 80, 400, 80));//, guiSkin.window);
            GUILayout.BeginHorizontal("");
            if (GUILayout.Button("Delete", guiSkin.button))
            {
                objPlacement.deleteObj();
            }
            //GUILayout.Label("Selected Object", guiSkin.label);
            for (int i = 0; i < colors.Length; i++)
            {
                if (GUILayout.Button(colorButtons[i], guiSkin.customStyles[1]))
                {
                    objPlacement.changeColor(colors[i]);
                }
            }
            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }

        GUI.Label(new Rect(leftAreaWidth + 30, Screen.height - Input.mousePosition.y, 150, 50), GUI.tooltip, guiSkin.label);
    }

    void save()
    {
        GUILayout.BeginArea(new Rect(ForceAspectRatio.xOffset, ForceAspectRatio.yOffset, leftAreaWidth, ForceAspectRatio.screenHeight));//, guiSkin.customStyles[3]);

        levelName = GUILayout.TextField(levelName, guiSkin.textField);

        GUILayout.TextArea("CARE! If another level of you already made has the same, the old level will be overwritten!");

        GUILayout.BeginHorizontal("box");
        if (GUILayout.Button("OKAY", guiSkin.button))
        {
            objPlacement.updateXMLLevelObjects(level);
            level.saveLevel();
            menuFunction = edit;
        }
        if (GUILayout.Button("CANCEL", guiSkin.button))
        {
            menuFunction = edit;
        }
        GUILayout.EndHorizontal();
        GUILayout.EndArea();
    }

    public bool isMouseOnGui(Vector2 pos)
    {
        Rect hist = new Rect(ForceAspectRatio.screenWidth - 100, ForceAspectRatio.screenHeight - ForceAspectRatio.yOffset - 100, 100, 150);
        if (pos.x < leftAreaWidth || hist.Contains(pos))
        {
            return true;
        }
        return false;
    }

    public bool isMouseOnEditScreen(Vector2 pos)
    {
        if (showEditScreen)
        {
            Rect hist = new Rect(new Rect(ForceAspectRatio.screenWidth - 400, 40, 400, 40));
            return hist.Contains(pos);
        }
        return false;
    }

    public void deselectObj()
    {
        seletedObj = -1;
    }

    public void showEditMEnu(bool show)
    {
        showEditScreen = show;
    }
}
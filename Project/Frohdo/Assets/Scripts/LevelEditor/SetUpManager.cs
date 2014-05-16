using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SetUpManager : MonoBehaviour
{
    public GameObject marker;
    public GameObject layerPrefab_;
    public GameObject layerBgPrefab_;
    public Camera renderCam;

    private GameObject sceneController;
    private GameObject sceneObjects;
    public GUISkin skin;

    //Controller to Init
    private GUI_Controller_Editor guiController;
    private LevelEditorParser levelParser;
    private EditorObjectPlacement objectPlacement;
    private LevelLoader Levelloader;
    private RenderGameObjectToTexture renderToTexture;
    //Other Controller
    public StateManager stateManager;

    private Vector2 levelSize;
    private string levelName;

    void Awake()
    {
        sceneController = GameObject.Find("SceneController");
        sceneObjects = GameObject.Find("SceneObjects");
        levelName = "Enter Level Name";
        levelSize = new Vector2(40, 25);

        Editor_Grid.Instance.layerBg_pref = layerBgPrefab_;

        renderToTexture = createController("GameobjectToTextureRenderer", "RenderGameObjectToTexture") as RenderGameObjectToTexture;
        renderToTexture.renderCam = renderCam;

        levelParser = createController("LevelParser", "LevelEditorParser") as LevelEditorParser;

        if (SceneManager.Instance.loadLevelToEdit)
        {
            Levelloader = createController("LevelLoader", "LevelLoader") as LevelLoader;
            Levelloader.layerPrefab_ = layerPrefab_;
            Levelloader.camera_ = Camera.main;
            Levelloader.SceneObjects = sceneObjects;
        }
        else
        {
            setUpEmpyScene();
        }

        guiController = createController("GUI_Controller", "GUI_Controller_Editor") as GUI_Controller_Editor;
        stateManager.guiController = guiController;

        objectPlacement = createController("ObjectPlament", "EditorObjectPlacement") as EditorObjectPlacement;
        objectPlacement.marker = marker;

        InitGUI();
    }

    void Start()
    {
        Camera.main.GetComponent<CameraMovement>().gui = guiController;
    }


    private Component createController(string name, string type)
    {
        GameObject o = new GameObject();
        o.transform.parent = sceneController.transform;
        o.name = name;
        o.AddComponent(type);
        Component c = o.GetComponent(type);
        return c;
    }

    private void InitGUI()
    {
        GUI_SaveAndPreview gui_save = new GUI_SaveAndPreview(new Vector2(0, 0), new Vector2(150, 75), skin);
        gui_save.active = false;
        guiController.addGui(gui_save);
        stateManager.saveAndPreview = gui_save;

        GUI_Selected gui_selected = new GUI_Selected(new Vector2(0, 0), new Vector2(200, 200), skin);
        gui_selected.active = false;
        guiController.addGui(gui_selected);
        stateManager.selected = gui_selected;

        GUI_Commands gui_commands = new GUI_Commands(new Vector2(0, 0), new Vector2(200, 100), skin);
        gui_commands.active = false;
        guiController.addGui(gui_commands);
        stateManager.commands = gui_commands;

        GUI_ObjectToPlace gui_objectToPlace = new GUI_ObjectToPlace(new Vector2(800, 0), new Vector2(200, 100), skin);
        gui_objectToPlace.active = false;
        gui_objectToPlace.guiController = guiController;
        guiController.addGui(gui_objectToPlace);
        stateManager.objToPlace = gui_objectToPlace;

        initGuiObjectSelect();
        initColorSelect();
    }

    private void initColorSelect()
    {
        List<GUI_ContentColor> colorButtons = new List<GUI_ContentColor>();
        guiController.colorButtons = colorButtons;
        string[] colors = LevelObjectController.Instance.getColors();

        for (int i = 0; i < colors.Length; i++)
        {
            Texture2D tex = new Texture2D(30, 30);
            Color[] c = tex.GetPixels();
            for (int j = 0; j < c.Length; j++) c[j] = LevelObjectController.Instance.GetColor(colors[i]);
            tex.SetPixels(c);
            tex.Apply();
            GUI_ContentColor cont = new GUI_ContentColor();
            cont.content = new GUIContent(tex);
            cont.func = stateManager.updateColor;
            cont.color = colors[i];
            colorButtons.Add(cont);
        }

        GUI_ColorSelection gui_color = new GUI_ColorSelection(new Vector2(ForceAspectRatio.xOffset + 300, ForceAspectRatio.yOffset), new Vector2(300, 50), skin);
        gui_color.content = colorButtons;
        guiController.addGui(gui_color);
        stateManager.colorSelection = gui_color; 
    }

    private void initGuiObjectSelect()
    {
        string[] colors = LevelObjectController.Instance.getColors();
        guiController.gui_LevelObjects = new List<GUI_ContentObject>[colors.Length];
        
        for (int i = 0; i < colors.Length; i++)
        {
            guiController.gui_LevelObjects[i] = new List<GUI_ContentObject>();
            for (int o = 0; o < LevelObjectController.Instance.levelObjectPrefabs_.Count; o++)
            {
                GUIContent curCont = new GUIContent(renderToTexture.renderGameObjectToTexture(LevelObjectController.Instance.levelObjectPrefabs_[o], 256, 256, colors[i]), LevelObjectController.Instance.levelObjectPrefabs_[o].name);
                GUI_ContentObject contObj = new GUI_ContentObject();
                contObj.content = curCont;
                contObj.func = stateManager.updateObject;
                contObj.prefab = LevelObjectController.Instance.levelObjectPrefabs_[o];
                guiController.gui_LevelObjects[i].Add(contObj);
            }
        }
        GUI_ObjectSelection gui_objectSelect = new GUI_ObjectSelection(new Vector2(ForceAspectRatio.xOffset, ForceAspectRatio.yOffset), new Vector2(280, ForceAspectRatio.screenHeight), skin);
        gui_objectSelect.guiController = guiController;
        gui_objectSelect.objects = guiController.gui_LevelObjects[0];
        guiController.addGui(gui_objectSelect);
        stateManager.objectSelection = gui_objectSelect;
    }

    private void setUpEmpyScene()
    {
        Editor_Grid.Instance.initGrid(levelSize);
        for (int i = 0; i < GlobalVars.Instance.LayerCount; i++)
        {
            GameObject layerObject = (GameObject)Instantiate(layerPrefab_);
            layerObject.transform.parent = sceneObjects.transform;

            stateManager.layers[i] = layerObject;

            Layer layerScript = layerObject.GetComponent<Layer>();

            Vector3 position = Camera.main.transform.position;
            position.z = GlobalVars.Instance.layerZPos[i];
            Vector2 parallax = GlobalVars.Instance.layerParallax[i];
            bool isPlayLayer = (i == GlobalVars.Instance.playLayer);

            layerObject.name = "Layer " + i;
            layerObject.transform.position = position;
            layerScript.parallaxFactor_ = parallax;
            layerScript.hasColliders_ = isPlayLayer;
            layerScript.camera_ = Camera.main;
        }
        levelParser.initEmpty();
    }
}

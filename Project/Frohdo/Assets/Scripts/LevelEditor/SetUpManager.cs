using UnityEngine;
using System.Collections;

public class SetUpManager : MonoBehaviour
{
    public GameObject marker;
    public GameObject layerPrefab_;

    private GameObject sceneController;
    private GameObject sceneObjects;
    public GUISkin skin;
    
    //Controller
    private GUI_Controller_Editor guiController;
    private LevelEditorParser levelParser;
    private EditorObjectPlacement objectPlacement;
    private EditCommandManager commandManager;
    private LevelLoader Levelloader;


    private Vector2 levelSize;
    private string levelName;

    void Awake()
    {
        sceneController = GameObject.Find("SceneController");
        sceneObjects = GameObject.Find("SceneObjects");
        
        levelName = "Enter Level Name";
        levelSize = new Vector2(40, 25);

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
            //create Empty scene
            for (int i = 0; i < GlobalVars.Instance.LayerCount; i++)
            {
                GameObject layerObject = (GameObject)Instantiate(layerPrefab_);
                layerObject.transform.parent = sceneObjects.transform;
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

        guiController = createController("GUI_Controller", "GUI_Controller_Editor") as GUI_Controller_Editor;
        Camera.main.GetComponent<CameraMovement>().gui = guiController;
        objectPlacement = createController("ObjectPlament", "EditorObjectPlacement") as EditorObjectPlacement;
        objectPlacement.marker = marker;
        
        //commandManager = createController("CommandManager", "EditCommandManager") as EditCommandManager;
    }

    // Use this for initialization
    void Start()
    {          
        InitGUI();
    }


    private Component createController(string name, string type )
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

        GUI_Selected gui_selected = new GUI_Selected(new Vector2(0, 0), new Vector2(200, 200), skin, commandManager);
        gui_selected.active = false;
        guiController.addGui(gui_selected);

        GUI_ColorSelection gui_color = new GUI_ColorSelection(new Vector2(0, 0), new Vector2(300, 50), skin);
        gui_color.active = false;
        guiController.addGui(gui_color);

        GUI_Commands gui_commands = new GUI_Commands(new Vector2(0, 0), new Vector2(200, 100), skin, commandManager);
        gui_commands.active = false;
        guiController.addGui(gui_commands);

        GUI_ObjectSelection gui_objectSelect = new GUI_ObjectSelection(new Vector2(0, 0), new Vector2(300, 300), skin);
        gui_objectSelect.active = false;
        guiController.addGui(gui_objectSelect);
    }
}

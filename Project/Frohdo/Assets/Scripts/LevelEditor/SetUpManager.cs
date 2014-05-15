using UnityEngine;
using System.Collections;

public class SetUpManager : MonoBehaviour
{

    public Gui_Main gui;
    public LevelEditorParser levelParser;
    public EditorObjectPlacement objectPlacement;
    public EditCommandManager commandManager;

    private Vector2 levelSize;
    private string levelName;

    void Awake()
    {
        levelName = "Enter Level Name";
        levelSize = new Vector2(40, 25);
    }

    // Use this for initialization
    void Start()
    {
        objectPlacement.activeLayer = GlobalVars.Instance.playLayer;
        
        if (SceneManager.Instance.loadLevelToEdit)
        {
            levelParser.loadLevel(SceneManager.Instance.levelToEdit);
            SceneManager.Instance.levelToEdit = null;
            commandManager.resetHistory();
            levelName = levelParser.levelName;
        }
        else
        {
            levelParser.initEmpty();
            levelParser.levelName = levelName;
            levelParser.setSize(levelSize);
            levelParser.setLevelBackground("W");

            objectPlacement.init(levelSize);
        }

        SceneManager.Instance.loadLevelToEdit = false;

        gui.levelName = levelName;
        gui.activeLayer = GlobalVars.Instance.playLayer;
    }

}

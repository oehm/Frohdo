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
        
        if (SceneManager.Instance.levelToEdit != null)
        {
            levelParser.loadLevel(SceneManager.Instance.levelToEdit);
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

        gui.levelName = levelName;
        gui.activeLayer = GlobalVars.Instance.playLayer;
    }

}

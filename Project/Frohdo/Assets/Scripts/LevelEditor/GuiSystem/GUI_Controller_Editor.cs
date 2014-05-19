using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GUI_Controller_Editor : MonoBehaviour
{

    public GUISkin skin;
    public StateManager stateManager;
    public RenderGameObjectToTexture renderToTexture;

    public Vector2 colorsp;
    public Vector2 colorsSize;
    public Vector2 Commands;
    public Vector2 commandsSize;
    public Vector2 layer;
    public Vector2 layerSize;
    public Vector2 bg;
    public Vector2 objSelection;
    public Vector2 objSelectionSize;
    public Vector2 saveAndP;
    public Vector2 saveAndPSize;


    public List<GUI_ContentColor> colorButtons;
    public List<GUI_ContentObject>[] gui_LevelObjects;
    public GUI_ContentObject character;


    private List<GUI_Element> guiList;

    private Rect screenSize;

    private GUI_ColorSelection gui_color;
    private GUI_SaveAndPreview gui_save;
    private GUI_Commands gui_commands;
    private GUI_Selected gui_selected;
    private GUI_ObjectSelection gui_objectSelect;

    private Rect topRect;
    private Rect leftRect;

    void Awake()
    {
        guiList = new List<GUI_Element>();
        stateManager.guiController = this;
        gui_color = new GUI_ColorSelection(Vector2.zero, Vector2.zero, skin);
        stateManager.colorSelection = gui_color;
        gui_save = new GUI_SaveAndPreview(Vector2.zero, Vector2.zero, skin);
        stateManager.saveAndPreview = gui_save;
        gui_commands = new GUI_Commands(Vector2.zero, Vector2.zero, skin);
        stateManager.commands = gui_commands;
        gui_selected = new GUI_Selected(Vector2.zero, Vector2.zero, skin);
        stateManager.selectedGui = gui_selected;
        gui_objectSelect = new GUI_ObjectSelection(Vector2.zero, Vector2.zero, skin);
        stateManager.objectSelection = gui_objectSelect;
    }

    void Start()
    {
        screenSize = ForceAspectRatio.screenRect;
        topRect = new Rect(ForceAspectRatio.xOffset, ForceAspectRatio.yOffset, ForceAspectRatio.screenWidth, 80);
        leftRect = new Rect(ForceAspectRatio.xOffset, ForceAspectRatio.yOffset + 80, 300, ForceAspectRatio.screenHeight - 80);

        InitGUI();
        resize();
    }

    private void InitGUI()
    {
        gui_save = new GUI_SaveAndPreview(saveAndP, saveAndPSize, skin);
        gui_save.active = false;
        gui_save.parentRect = topRect;
        stateManager.saveAndPreview = gui_save;
        addGui(gui_save);

        gui_selected = new GUI_Selected(new Vector2(0, 0), new Vector2(100, 100), skin);
        gui_selected.active = false;
        gui_selected.manager = stateManager;
        stateManager.selectedGui = gui_selected;

        gui_commands = new GUI_Commands(Commands, commandsSize, skin);
        gui_commands.active = true;
        gui_commands.parentRect = topRect;
        addGui(gui_commands);

        initGuiObjectSelect();
        initColorSelect();
        initGuiLayerSelect();
    }

    private void initColorSelect()
    {
        List<GUI_ContentColor> colorButtons = new List<GUI_ContentColor>();
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
        gui_color = new GUI_ColorSelection(colorsp, colorsSize, skin);
        gui_color.parentRect = topRect;
        gui_color.content = colorButtons;
        addGui(gui_color);
    }

    private void initGuiObjectSelect()
    {
        gui_objectSelect = new GUI_ObjectSelection(objSelection, objSelectionSize, skin);
        string[] colors = LevelObjectController.Instance.getColors();
        gui_LevelObjects = new List<GUI_ContentObject>[colors.Length];

        GUIContent characterGuiCont = new GUIContent(renderToTexture.renderGameObjectToTexture(LevelObjectController.Instance.GetPrefabByName("Character", GlobalVars.Instance.playLayer, true), 256, 256, ""), LevelObjectController.Instance.GetPrefabByName("Character", GlobalVars.Instance.playLayer, true).name);
        GUI_ContentObject charactercont = new GUI_ContentObject();
        charactercont.content = characterGuiCont;
        charactercont.func = stateManager.updateObject;
        charactercont.prefab = LevelObjectController.Instance.GetPrefabByName("Character", GlobalVars.Instance.playLayer, true);
        Gridable[] g = charactercont.prefab.GetComponentsInChildren<Gridable>(true);
        charactercont.layerMat = g[0].availableInLayer;
        gui_objectSelect.character = charactercont;
        character = charactercont;

        for (int i = 0; i < colors.Length; i++)
        {
            gui_LevelObjects[i] = new List<GUI_ContentObject>();

            for (int o = 0; o < LevelObjectController.Instance.levelObjectPrefabs_.Count; o++)
            {
                if (LevelObjectController.Instance.levelObjectPrefabs_[o].name == "Character") continue;
                
                GUIContent curCont = new GUIContent(renderToTexture.renderGameObjectToTexture(LevelObjectController.Instance.levelObjectPrefabs_[o], 256, 256, colors[i]), LevelObjectController.Instance.levelObjectPrefabs_[o].name);
                GUI_ContentObject contObj = new GUI_ContentObject();
                contObj.content = curCont;
                Gridable[] go = LevelObjectController.Instance.levelObjectPrefabs_[o].GetComponentsInChildren<Gridable>(true);
                contObj.layerMat = go[0].availableInLayer;
                contObj.func = stateManager.updateObject;
                contObj.prefab = LevelObjectController.Instance.levelObjectPrefabs_[o];
                gui_LevelObjects[i].Add(contObj);
            }
        }
        gui_objectSelect.guiController = this;
        gui_objectSelect.parentRect = leftRect;
        gui_objectSelect.objects = gui_LevelObjects[0];
        stateManager.objectSelection = gui_objectSelect;
    }

    private void initGuiLayerSelect()
    {
        GUI_LayerSelect gui_layerSelect = new GUI_LayerSelect(layer, layerSize, skin);
        gui_layerSelect.parentRect = topRect;
        List<GUI_ContentLayer> gui_content = new List<GUI_ContentLayer>();

        for (int i = 0; i < GlobalVars.Instance.LayerCount; i++)
        {
            GUI_ContentLayer c = new GUI_ContentLayer();
            c.content = new GUIContent((i + 1).ToString());
            c.layerIndex = i;
            c.func = stateManager.updateLayer;
            gui_content.Add(c);
        }
        gui_layerSelect.content = gui_content;
        addGui(gui_layerSelect);
        stateManager.layerSelect = gui_layerSelect;
    }

    void Update()
    {
        if (screenSize != ForceAspectRatio.screenRect)
        {
            resize();
        }
    }

    void resize()
    {
        topRect = new Rect(ForceAspectRatio.xOffset, ForceAspectRatio.yOffset, ForceAspectRatio.screenWidth, 80);

        leftRect = new Rect(ForceAspectRatio.xOffset, ForceAspectRatio.yOffset + 80, 300, ForceAspectRatio.screenHeight - 80);

        gui_objectSelect.parentRect = leftRect;
        foreach (GUI_Element g in guiList)
        {
            //g.resize(ForceAspectRatio.screenRect);
        }
    }

    void OnGUI()
    {
        GUILayout.BeginArea(topRect, skin.customStyles[0]);
        foreach (GUI_Element g in guiList)
        {
            g.Draw();
        }
        GUILayout.EndArea();
        GUILayout.BeginArea(leftRect, skin.customStyles[3]);
        gui_objectSelect.Draw();
        GUILayout.EndArea();
        gui_selected.Draw();
    }

    public void addGui(GUI_Element gui)
    {
        guiList.Add(gui);
    }

    public void removeGui(GUI_Element g)
    {
        guiList.Remove(g);
    }

    public bool mouseOnGui(Vector2 pos)
    {
        bool mOnGui = false;
        Vector2 invertedPos = new Vector2(pos.x - ForceAspectRatio.xOffset, ForceAspectRatio.screenHeight - pos.y + ForceAspectRatio.yOffset);
        if (gui_objectSelect.mouseOnGui(invertedPos))
        {
            return true;
        }
        if (gui_selected.mouseOnGui(invertedPos))
        {
            return true;
        }
        foreach (GUI_Element g in guiList)
        {
            if (g.mouseOnGui(invertedPos)) return true;
        }
        return mOnGui;
    }

    public GUIContent getContent(GameObject obj, string color)
    {
        if (obj.name == "Character")
        {
            return character.content;
        }

        string[] colors = LevelObjectController.Instance.getColors();
        int colorIndex = 0;
        for (int i = 0; i < colors.Length; i++)
        {
            if (colors[i] == color)
            {
                colorIndex = i;
                break;
            }
        }

        GUIContent cont = new GUIContent();
        foreach (GUI_ContentObject g in gui_LevelObjects[colorIndex])
        {
            if (g.prefab.name == obj.name)
            {
                return g.content;
            }
        }
        return cont;
    }

}

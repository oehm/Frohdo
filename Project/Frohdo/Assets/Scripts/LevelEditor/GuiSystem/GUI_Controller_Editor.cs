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
    public Vector2 backgroundColor;
    public Vector2 backgroundColorSize;

    public string levelName { get; set; }

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
    private GUI_Background gui_background;
    private GUI_SaveScreen gui_SaveScreen;
    private GUI_LayerSelect gui_layerSelect;

    private Rect topRect;
    private Rect leftRect;
    private Rect bgRect;
    private Rect saveRect;

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
        gui_background = new GUI_Background(new Vector2(0,0),new Vector2(1,1),skin);
        stateManager.backgroundgui = gui_background;
        gui_SaveScreen = new GUI_SaveScreen(new Vector2(0,0), new Vector2(1, 1), skin);
        stateManager.guiSaveScreen = gui_SaveScreen;
        gui_layerSelect = new GUI_LayerSelect(Vector2.zero,Vector2.zero, skin);
        stateManager.layerSelect = gui_layerSelect;

    }

    void Start()
    {
        screenSize = ForceAspectRatio.screenRect;
        topRect = new Rect(ForceAspectRatio.xOffset, ForceAspectRatio.yOffset, ForceAspectRatio.screenWidth, 80);
        leftRect = new Rect(ForceAspectRatio.xOffset, ForceAspectRatio.yOffset + 80, 300, ForceAspectRatio.screenHeight - 80);
        bgRect = new Rect(ForceAspectRatio.xOffset, ForceAspectRatio.yOffset, backgroundColorSize.x * topRect.width, backgroundColorSize.y * topRect.height);
        saveRect = screenSize;

        levelName = "Enter Level Name";

        InitGUI();
        resize();
    }

    private void InitGUI()
    {
        gui_save = new GUI_SaveAndPreview(saveAndP, saveAndPSize, skin);
        gui_save.active = false;
        gui_save.parentRect = topRect;
        stateManager.saveAndPreview = gui_save;
        gui_save.manager = stateManager;
        addGui(gui_save);

        gui_selected = new GUI_Selected(new Vector2(0, 0), new Vector2(100, 100), skin);
        gui_selected.active = false;
        gui_selected.manager = stateManager;
        stateManager.selectedGui = gui_selected;

        gui_commands = new GUI_Commands(Commands, commandsSize, skin);
        gui_commands.active = true;
        gui_commands.parentRect = topRect;
        addGui(gui_commands);

        gui_SaveScreen.parentRect = saveRect;
        gui_SaveScreen.active = false;
        gui_SaveScreen.levelName = levelName;
        gui_SaveScreen.manger = stateManager;
        stateManager.guiSaveScreen = gui_SaveScreen;


        initGuiObjectSelect();
        initColorSelect();
        initGuiLayerSelect();
    }

    private void initColorSelect()
    {
        List<GUI_ContentColor> colorButtons = new List<GUI_ContentColor>();
        List<GUI_ContentColor> backgroundButtons = new List<GUI_ContentColor>();
        string[] colors = LevelObjectController.Instance.getColors();

        for (int i = 0; i < colors.Length; i++)
        {
            Texture2D tex = new Texture2D(30, 30);
            Color[] c = tex.GetPixels();
            for (int j = 0; j < c.Length; j++) c[j] = LevelObjectController.Instance.GetColor(colors[i]);
            tex.SetPixels(c);
            tex.Apply();
            GUI_ContentColor cont = new GUI_ContentColor();
            GUI_ContentColor contBG = new GUI_ContentColor();
            cont.content = new GUIContent(tex);
            cont.func = stateManager.updateColor;
            cont.color = colors[i];
            colorButtons.Add(cont);

            contBG.content = new GUIContent(tex);
            contBG.func = stateManager.changeBackgroundColor;
            contBG.color = colors[i];
            backgroundButtons.Add(contBG);
        }
        gui_color = new GUI_ColorSelection(colorsp, colorsSize, skin);
        gui_color.parentRect = topRect;
        gui_color.content = colorButtons;

        gui_background.content = backgroundButtons;
        gui_background.parentRect = bgRect;

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
        gui_layerSelect = new GUI_LayerSelect(layer, layerSize, skin);
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

        bgRect = new Rect(topRect.x + topRect.width * backgroundColor.x, topRect.y + topRect.height * backgroundColor.y, backgroundColorSize.x * topRect.width,  skin.customStyles[7].fixedHeight * 7);

        saveRect = ForceAspectRatio.screenRect;

        gui_background.parentRect = bgRect;
        gui_objectSelect.parentRect = leftRect;
        gui_SaveScreen.parentRect = saveRect;
        foreach (GUI_Element g in guiList)
        {
            g.parentRect = topRect;
        }
        screenSize = ForceAspectRatio.screenRect;
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
        GUILayout.BeginArea(bgRect);
        gui_background.Draw();
        GUILayout.EndArea();
        gui_selected.Draw();
        GUILayout.BeginArea(saveRect);
        gui_SaveScreen.Draw();
        GUILayout.EndArea();
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
        Vector2 invertedPos = new Vector2(pos.x, ForceAspectRatio.screenHeight - pos.y + 2* ForceAspectRatio.yOffset);
        if (leftRect.Contains(invertedPos))
        {
            return true;
        }
        if (topRect.Contains(invertedPos))
        {
            return true;
        }
        //foreach (GUI_Element g in guiList)
        //{
        //    if (g.mouseOnGui(invertedPos)) return true;
        //}
        if (gui_background.mouseOnGui(invertedPos))
        {
            return true;
        }
        if(gui_selected.mouseOnGui(invertedPos))
        {
            return true;
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

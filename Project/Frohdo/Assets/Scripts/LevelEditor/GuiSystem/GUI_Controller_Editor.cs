using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GUI_Controller_Editor : MonoBehaviour {

    public GUISkin skin;

    public List<GUI_ContentColor> colorButtons;
    public List<GUI_ContentObject>[] gui_LevelObjects;
    public GUI_ContentObject character;
    public Rect rect {get;set;}

    private List<GUI_Element> guiList;

    private Rect screenSize;
    void Awake()
    {
        guiList = new List<GUI_Element>();
        screenSize = ForceAspectRatio.screenRect;
    }

    void Start()
    {
        resize();
    }

    void Update()
    {
        if(screenSize != ForceAspectRatio.screenRect)
        {
            resize();
        }
    }

    void resize()
    {
        foreach (GUI_Element g in guiList)
        {
            //g.resize(ForceAspectRatio.screenRect);
        }
    }

    void OnGUI()
    {
        GUILayout.BeginArea(rect, skin.customStyles[0]);
        foreach(GUI_Element g in guiList)
        {
            g.Draw();
        }
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
        Vector2 invertedPos = new Vector2(pos.x, ForceAspectRatio.screenHeight - pos.y + ForceAspectRatio.yOffset);
        foreach(GUI_Element g in guiList)
        {
            if (g.mouseOnGui(invertedPos)) return true;
        }
        return mOnGui;
    }

    public GUIContent getContent (GameObject obj, string color)
    {
        if(obj.name == "CharacterEditor")
        {
            return character.content;
        }
        
        string[] colors = LevelObjectController.Instance.getColors();
        int colorIndex = 0;
        for(int i=0; i< colors.Length; i++)
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
            if(g.prefab.name == obj.name)
            {
                return g.content;
            }
        }
        return cont;
    }

}

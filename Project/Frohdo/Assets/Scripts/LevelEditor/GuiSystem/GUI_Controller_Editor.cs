using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GUI_Controller_Editor : MonoBehaviour {

    public GUISkin skin;

    public List<GUI_ContentColor> colorButtons;
    public List<GUI_ContentObject>[] gui_LevelObjects;
    public GUI_ContentObject character;

    private List<GUI_Element> guiList;


    void Awake()
    {
        guiList = new List<GUI_Element>();
    }

    void Start () {
	
	}

    void OnGUI()
    {
        foreach(GUI_Element g in guiList)
        {
            g.Draw();
        }
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

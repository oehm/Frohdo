using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GUI_Controller_Editor : MonoBehaviour {

    public GUISkin skin;
    
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
        foreach(GUI_Element g in guiList)
        {
            if (g.mouseOnGui(pos)) return true;
        }
        return mOnGui;
    }
}

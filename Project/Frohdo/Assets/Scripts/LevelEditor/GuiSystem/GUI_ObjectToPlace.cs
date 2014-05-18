using UnityEngine;
using System.Collections;

public class GUI_ObjectToPlace : GUI_Element
{
    public GUIContent content { get; set; }
    public GUI_Controller_Editor guiController;

    private Vector2 scrollPos;

    public GUI_ObjectToPlace(Vector2 pos, Vector2 s, GUISkin sk)
    {
        position = pos;
        size = s;
        skin = sk;

        scrollPos = new Vector2(0, 0);
        active = true;
    }
    public override void Draw()
    {
        _rect = GUILayout.Window(47, _rect, windowFunc, "", skin.customStyles[5]);  
    }

    public void selectObj(GameObject obj, string color)
    {
        content = guiController.getContent(obj, color);
    }


    void windowFunc(int winID)
    {
        GUILayout.Label("OBJECT TO PLACE", skin.label);
        if (active)
        {
            GUILayout.Box(content, skin.box);
        }
        GUI.DragWindow(new Rect(0, 0, 10000, 10000));
    }

    public override bool mouseOnGui(Vector2 pos)
    {
        return _rect.Contains(pos);
    }
}

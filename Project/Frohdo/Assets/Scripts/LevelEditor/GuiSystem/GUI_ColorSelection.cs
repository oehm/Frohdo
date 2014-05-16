using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GUI_ColorSelection : GUI_Element {

public List<GUI_ContentColor> content { get; set; }

    private Vector2 scrollPos;

    public GUI_ColorSelection(Vector2 pos, Vector2 s, GUISkin sk)
    {
        position = pos;
        size = s;
        skin = sk;

        active = true;
        scrollPos = new Vector2(0, 0);
        content = new List<GUI_ContentColor>();
    }

    public override void Draw()
    {
        if (!active) return;
        _rect = GUILayout.Window(1, _rect, windowFunc, "",skin.window);
    }

    private void windowFunc(int winId)
    {
        GUILayout.BeginHorizontal("");
        foreach(GUI_ContentColor g in content)
        {
            if(GUILayout.Button(g.content,skin.button))
            {
                g.func(g.color);
            }
        }
        GUILayout.EndHorizontal();
        GUI.DragWindow(new Rect(0, 0, 10000, 10000));
    }
}

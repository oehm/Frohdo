﻿using UnityEngine;
using System.Collections;

public class GUI_SaveAndPreview : GUI_Element
{
    public GUI_SaveAndPreview(Vector2 pos, Vector2 s, GUISkin sk)
    {
        position = pos;
        size = s;
        skin = sk;
        active = true;
    }

    public override void Draw()
    {
        if (!active) return;
        _rect = GUILayout.Window(2, _rect, windowFunc, "",skin.window);
    }

    private void windowFunc(int winId)
    {
        GUI.DragWindow(new Rect(0, 0, 10000, 10000));
        if (GUILayout.Button("SAVE", skin.button))
        {
            
        }
        if (GUILayout.Button("PREVIEW", skin.button))
        {
               
        }
    }
}

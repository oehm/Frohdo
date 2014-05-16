using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GUI_ObjectSelection : GUI_Element {

    public GUI_Controller_Editor guiController;

    private Vector2 scrollPos;

    public GUI_ObjectSelection(Vector2 pos, Vector2 s, GUISkin sk)
    {
        position = pos;
        size = s;
        skin = sk;

        scrollPos = new Vector2(0, 0);
        active = true;
    }

    public override void Draw()
    {
        if (!active) return;
        GUILayout.BeginArea(_rect);
        scrollPos = GUILayout.BeginScrollView(scrollPos, skin.scrollView);
        {
            int xCount = 0;
            GUILayout.BeginHorizontal("");
            foreach(GUI_ContentObject g in guiController.gui_LevelObjects[1])
            {
                if (xCount >= 2)
                {
                    xCount = 0;
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                }
                xCount++;
                if(GUILayout.Button(g.content,skin.customStyles[0]))
                {
                    g.func(g.prefab);
                }
            }
            GUILayout.EndHorizontal();
        }
        GUILayout.EndScrollView();
        GUILayout.EndArea();
    }

}

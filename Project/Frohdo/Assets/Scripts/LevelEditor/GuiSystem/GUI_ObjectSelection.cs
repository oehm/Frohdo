using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GUI_ObjectSelection : GUI_Element {

    public List<GUI_ContentObject> content { get; set; }

    private Vector2 scrollPos;


    public GUI_ObjectSelection(Vector2 pos, Vector2 s, GUISkin sk)
    {
        position = pos;
        size = s;
        skin = sk;

        scrollPos = new Vector2(0, 0);
        content = new List<GUI_ContentObject>();
        active = true;
    }

    public override void Draw()
    {
        if (!active) return;
        GUILayout.BeginArea(_rect);
        scrollPos = GUILayout.BeginScrollView(scrollPos, skin.scrollView);
        {
            foreach(GUI_ContentObject g in content)
            {
                if(GUILayout.Button(g.content,skin.button))
                {
                    g.func(g.prefab);
                }
            }
        }
        GUILayout.EndScrollView();
        GUILayout.EndArea();
    }

}

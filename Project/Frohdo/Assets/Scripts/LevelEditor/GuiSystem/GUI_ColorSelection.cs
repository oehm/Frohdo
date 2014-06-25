using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GUI_ColorSelection : GUI_Element {

public List<GUI_ContentColor> content { get; set; }

    public GUI_ColorSelection(Vector2 pos, Vector2 s, GUISkin sk)
    {
        position = pos;
        size = s;
        skin = sk;

        active = true;
        content = new List<GUI_ContentColor>();
    }

    public override void Draw()
    {
        if (!active) return;
        GUILayout.BeginArea(_rect);
        GUILayout.BeginHorizontal();
        //GUILayout.Label("COLORS", skin.label);
        foreach (GUI_ContentColor g in content)
        {
            if (SoundButton.newSoundButton(g.content, skin.customStyles[1]))
            {
                g.func(g.color);
            }
        }
        GUILayout.EndHorizontal();
        GUILayout.EndArea();
    }

}

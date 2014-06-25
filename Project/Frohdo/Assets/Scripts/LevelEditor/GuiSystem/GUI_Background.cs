using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GUI_Background : GUI_Element
{

    public List<GUI_ContentColor> content { get; set; }

    private int selected;
    private Rect _parentRect;
    public bool popUp;


    public GUI_Background(Vector2 pos, Vector2 s, GUISkin sk)
    {
        position = pos;
        size = s;
        skin = sk;

        active = true;
        popUp = false;
        selected = 0;
        content = new List<GUI_ContentColor>();
    }


    public override bool mouseOnGui(Vector2 pos)
    {
        if (!popUp)
        {
            return false;
        }
        else
        {
            return parentRect.Contains(pos);
        }
    }

    public override void Draw()
    {
        if (!active) return;

        GUILayout.BeginArea(_rect);
        if (!popUp)
        {
            if (SoundButton.newSoundButton(content[selected].content, skin.customStyles[7]))
            {
                popUp = true;
            }
        }
        else
        {
            //GUILayout.BeginVertical();
            for (int i = 0; i < content.Count; i++)
            {
                if (SoundButton.newSoundButton(content[i].content, skin.customStyles[7]))
                {
                    selected = i;
                    content[i].func(content[i].color);
                    popUp = false;
                }
            }
            //GUILayout.EndVertical();
        }
        GUILayout.EndArea();

    }
}

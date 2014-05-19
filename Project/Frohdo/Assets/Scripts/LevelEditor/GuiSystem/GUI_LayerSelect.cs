using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GUI_LayerSelect : GUI_Element
{
    private List<GUI_ContentLayer> _content;

    public List<GUI_ContentLayer> content
    {
        get
        {
            return _content;
        }
        set
        {
            _content = value;
            pureContent = new GUIContent[_content.Count];
            for (int i = 0; i < _content.Count; i++)
            {
                pureContent[i] = _content[i].content;
            }
        }
    }

    private int selected;

    private GUIContent[] pureContent;

    public GUI_LayerSelect(Vector2 pos, Vector2 s, GUISkin sk)
    {
        position = pos;
        size = s;
        skin = sk;

        active = true;
        content = new List<GUI_ContentLayer>();

        selected = GlobalVars.Instance.playLayer;
    }
    public override void Draw()
    {
        if (!active) return;
        GUILayout.BeginArea(_rect);
        GUILayout.BeginHorizontal();
        GUILayout.Label("LAYER", skin.label);
        int oldSelected = selected;
        selected = GUILayout.Toolbar(selected, pureContent, skin.button);
        if (oldSelected != selected)
        {
            content[selected].func(content[selected].layerIndex);
        }
        GUILayout.EndHorizontal();
        GUILayout.EndArea();
    }


    public void select(int layerIndex)
    {
        selected = layerIndex;
    }
}

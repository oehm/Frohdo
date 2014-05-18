using UnityEngine;
using System.Collections;

public class GUI_Commands : GUI_Element
{


    public GUI_Commands(Vector2 pos, Vector2 s, GUISkin sk)
    {
        position = pos;
        size = s;
        skin = sk;

        active = true;
    }

    public override void Draw()
    {
        if (!active) return;
        GUILayout.BeginArea(_rect);
        GUILayout.BeginHorizontal("");
        if (GUILayout.Button("UNDO", skin.button))
        {
            EditCommandManager.Instance.undo();
        }
        if (GUILayout.Button("REDO", skin.button))
        {
            EditCommandManager.Instance.redo();
        }
        GUILayout.EndHorizontal();
        GUILayout.EndArea();
    }
}

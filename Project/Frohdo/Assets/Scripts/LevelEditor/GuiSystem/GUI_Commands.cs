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
        GUILayout.BeginHorizontal();
        GUILayout.Label("UNDO/REDO", skin.label);
        if (SoundButton.newSoundButton("<", skin.customStyles[2]))
        {
            EditCommandManager.Instance.undo();
        }
        if (SoundButton.newSoundButton(">", skin.customStyles[2]))
        {
            EditCommandManager.Instance.redo();
        }
        GUILayout.EndHorizontal();
        GUILayout.EndArea();
    }
}

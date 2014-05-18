using UnityEngine;
using System.Collections;

public class GUI_Commands : GUI_Element {

    
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
        _rect = GUILayout.Window(0, _rect, windowFunc, "",skin.customStyles[8]);
    }

    private void windowFunc(int winId)
    {
        if (GUILayout.Button("UNDO", skin.button))
        {
            EditCommandManager.Instance.undo();
        }
        if (GUILayout.Button("REDO", skin.button))
        {
            EditCommandManager.Instance.redo();
        }
        GUI.DragWindow(new Rect(0, 0, 10000, 10000));
    }
}

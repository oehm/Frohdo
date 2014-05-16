using UnityEngine;
using System.Collections;

public class GUI_Commands : GUI_Element {

    private EditCommandManager manager;
    
    public GUI_Commands(Vector2 pos, Vector2 s, GUISkin sk, EditCommandManager m)
    {
        position = pos;
        size = s;
        manager = m;
        skin = sk;

        active = true;
    }

    public override void Draw()
    {
        if (!active) return;
        _rect = GUILayout.Window(0, _rect, windowFunc, "",skin.window);
    }

    private void windowFunc(int winId)
    {
        GUI.DragWindow(new Rect(0, 0, 10000, 10000));
        if (GUILayout.Button("UNDO", skin.button))
        {
            manager.undo();
        }
        if (GUILayout.Button("REDO", skin.button))
        {
            manager.redo();
        }
    }
}

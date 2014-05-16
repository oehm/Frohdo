using UnityEngine;
using System.Collections;

public class GUI_Selected : GUI_Element {

    private EditCommandManager manager;
    public GUI_ContentObject content;
    
    public GUI_Selected(Vector2 pos, Vector2 s, GUISkin sk, EditCommandManager m)
    {
        position = pos;
        size = s;
        skin = sk;
        manager = m;
        content = null;

        active = true;
    }

    public override void Draw()
    {
        if (!active) return;
        _rect = GUILayout.Window(3, _rect, windowFunc, "",skin.window);
    }

    private void windowFunc(int winId)
    {
        GUI.DragWindow(new Rect(0, 0, 10000, 10000));
        GUILayout.Label("SELECTED",skin.label);
        if (content != null)
        {
            GUILayout.Box(content.content, skin.box);
        }
        if(GUILayout.Button("DELETE",skin.button))
        {
            DeleteObj command = new DeleteObj();
            command.setUpCommand(content.prefab,0);
            manager.executeCommand(command);
        }
    }
}

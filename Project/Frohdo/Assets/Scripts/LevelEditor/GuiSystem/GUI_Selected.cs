using UnityEngine;
using System.Collections;

public class GUI_Selected : GUI_Element {


    public GUI_ContentObject content;
    
    public GUI_Selected(Vector2 pos, Vector2 s, GUISkin sk)
    {
        position = pos;
        size = s;
        skin = sk;
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
        GUILayout.Label("SELECTED",skin.label);
        if (content != null)
        {
            GUILayout.Box(content.content, skin.box);
        }
        if(GUILayout.Button("DELETE",skin.button))
        {
            DeleteObj command = new DeleteObj();
            command.setUpCommand(content.prefab,0);
            EditCommandManager.Instance.executeCommand(command);
        }
        GUI.DragWindow(new Rect(0, 0, 10000, 10000));
    }
}

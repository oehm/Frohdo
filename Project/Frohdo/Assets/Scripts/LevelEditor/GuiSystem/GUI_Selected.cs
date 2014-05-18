using UnityEngine;
using System.Collections;

public class GUI_Selected : GUI_Element {


    public GUI_ContentObject content;
    public StateManager manager;

    public GameObject obj { get; set; }
    public int layerIdx { get; set; }
    
    public bool varnishable{get;set;}


    public GUI_Selected(Vector2 pos, Vector2 s, GUISkin sk)
    {
        position = pos;
        size = s;
        skin = sk;
        content = null;

        active = true;
        varnishable = false;
    }

    public override void Draw()
    {
        if (!active) return;
        _rect = GUILayout.Window(11, _rect, windowFunc, "",skin.customStyles[4]);
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
            manager.deleteObject(obj, layerIdx);
        }
        if(varnishable)
        {
            if(GUILayout.Button("VARNISH",skin.button))
            {
                VarnishObj command = new VarnishObj();
                command.setUpCommand(obj);
                Debug.Log( EditCommandManager.Instance.executeCommand(command));
            }
            if (GUILayout.Button("UNVARNISH", skin.button))
            {
                UnvarnishObj command = new UnvarnishObj();
                command.setUpCommand(obj);
                Debug.Log(EditCommandManager.Instance.executeCommand(command));
            }
        }
        GUI.DragWindow(new Rect(0, 0, 10000, 10000));
    }
}

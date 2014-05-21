using UnityEngine;
using System.Collections;

public class GUI_Selected : GUI_Element {


    public GUI_ContentObject content;
    public StateManager manager;

    private Vector2 _pos;

    public new Rect parentRect { get; set; } //force override of base class
    public Vector2 pos
    {
        get
        {
            return _pos;
        }
        set
        {
            _pos = value;
            _rect = new Rect(_pos.x, _pos.y, size.x, size.y);
        }
    }//force override of base class
    public new Vector2 size { get;set;}//force override of base class

    public GameObject obj { get; set; }
    public int layerIdx { get; set; }

    public GUI_Selected(Vector2 p, Vector2 s, GUISkin sk)
    {
        pos = pos;
        size = s;
        skin = sk;
        content = null;

        active = true;
    }

    public override void Draw()
    {
        if (!active) return;
        GUILayout.BeginArea(_rect);
        if (GUILayout.Button("", skin.customStyles[6]))
        {
            manager.deleteObject(obj, layerIdx);
        }
        GUILayout.EndArea();
    }

    public void setPos(GameObject obj)
    {
        Gridable g = obj.GetComponentInChildren<Gridable>();
        Vector2 objPos = Camera.main.WorldToScreenPoint(obj.transform.position + new Vector3(g.width/2,g.height/2));
        objPos.y = ForceAspectRatio.screenHeight - objPos.y + ForceAspectRatio.yOffset*2;
        pos = new  Vector2(objPos.x,objPos.y);
    }
}

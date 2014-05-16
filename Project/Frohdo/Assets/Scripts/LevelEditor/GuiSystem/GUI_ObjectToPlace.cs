using UnityEngine;
using System.Collections;

public class GUI_ObjectToPlace : GUI_Element
{
    public GUIContent content { get; set; }
    public GUI_Controller_Editor guiController;

    private Vector2 scrollPos;

    public GUI_ObjectToPlace(Vector2 pos, Vector2 s, GUISkin sk)
    {
        position = pos;
        size = s;
        skin = sk;

        scrollPos = new Vector2(0, 0);
        active = true;
    }
    public override void Draw()
    {
        if(!active) return;
        GUILayout.BeginArea(_rect);
        GUILayout.Box(content,skin.box);
        GUILayout.EndArea();
    }

    public void selectObj(GameObject obj, string color)
    {
        content = guiController.getContent(obj, color);
    }
}

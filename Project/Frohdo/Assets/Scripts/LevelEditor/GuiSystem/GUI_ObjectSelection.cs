using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GUI_ObjectSelection : GUI_Element
{

    public GUI_Controller_Editor guiController;
    public List<GUI_ContentObject> objects;
    public GUI_ContentObject character;

    public int layerIndex { get;set;}

    private Vector2 scrollPos;
    private bool showCharacter_;
    private bool showMarked;

    private GUI_ContentObject toMark;
    public GUI_ObjectSelection(Vector2 pos, Vector2 s, GUISkin sk)
    {
        position = pos;
        size = s;
        skin = sk;

        scrollPos = new Vector2(0, 0);
        active = true;
        showCharacter_ = true;
        showMarked = false;
        toMark = null;
    }

    public override void Draw()
    {
        if (!active) return;
        GUILayout.BeginArea(_rect);

        scrollPos = GUILayout.BeginScrollView(scrollPos, false, false);
        //scrollPos = GUILayout.BeginScrollView(scrollPos, false, false, new GUIStyle(), new GUIStyle());
        {
            int xCount = 0;
            GUILayout.BeginHorizontal("");
            if (showCharacter_)
            {
                xCount++;
                if (character == toMark && showMarked)
                {
                    if (GUILayout.Button(character.content, skin.customStyles[5]))
                    {
                        character.func(character.prefab);
                    }
                }
                else
                {
                    if (GUILayout.Button(character.content, skin.customStyles[4]))
                    {
                        character.func(character.prefab);
                        toMark = character;
                    }
                }
            }
            foreach (GUI_ContentObject g in objects)
            {
                if (!g.layerMat[layerIndex]) continue;
                if (xCount >= 2)
                {
                    xCount = 0;
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                }
                xCount++;
                if (showMarked && g == toMark)
                {
                    if (GUILayout.Button(g.content, skin.customStyles[5]))
                    {
                        g.func(g.prefab);
                    }
                }
                else
                {
                    if (GUILayout.Button(g.content, skin.customStyles[4]))
                    {
                        g.func(g.prefab);
                        toMark = g;
                    }
                }
            }
            GUILayout.EndHorizontal();
        }
        GUILayout.EndScrollView();
        GUILayout.EndArea();
    }

    public void changeColor(string color)
    {
        string[] colors = LevelObjectController.Instance.getColors();
        int colorIndex = 0;
        for (int i = 0; i < colors.Length; i++)
        {
            if (colors[i] == color)
            {
                colorIndex = i;
                break;
            }
        }
        //find current markedObject in the set of new objects
        foreach(GUI_ContentObject g in guiController.gui_LevelObjects[colorIndex])
        {
            if(g.prefab.name == toMark.prefab.name)
            {
                toMark = g;
            }
        }

        objects = guiController.gui_LevelObjects[colorIndex];

    }

    public void showCharacter(bool show)
    {
        showCharacter_ = show;
    }

    public void markObject(bool show)
    {
        showMarked = show;
    }
}

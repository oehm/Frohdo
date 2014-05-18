using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GUI_ObjectSelection : GUI_Element
{

    public GUI_Controller_Editor guiController;
    public List<GUI_ContentObject> objects;
    public GUI_ContentObject character;

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
        _rect.height = ForceAspectRatio.screenHeight;
        GUILayout.BeginArea(_rect);
        scrollPos = GUILayout.BeginScrollView(scrollPos, skin.scrollView);
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
                    if (GUILayout.Button(character.content, skin.customStyles[0]))
                    {
                        character.func(character.prefab);
                        toMark = character;
                    }
                }
            }
            foreach (GUI_ContentObject g in objects)
            {
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
                    if (GUILayout.Button(g.content, skin.customStyles[0]))
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

        objects = guiController.gui_LevelObjects[colorIndex];
    }

    public void showCharacter(bool show)
    {
        showCharacter_ = show;
    }

    public override void resize(Rect screenRect)
    {
        base.resize(screenRect);
        _rect.x = ForceAspectRatio.xOffset;
    }

    public void markObject(bool show)
    {
        showMarked = show;
    }
}

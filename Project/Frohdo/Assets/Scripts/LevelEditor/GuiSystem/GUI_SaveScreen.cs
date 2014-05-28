using UnityEngine;
using System.Collections;

public class GUI_SaveScreen : GUI_Element
{
    public bool preview;

    public StateManager manger;

    private string warning;

    public GUI_SaveScreen(Vector2 pos, Vector2 s, GUISkin sk)
    {
        position = pos;
        size = s;
        skin = sk;
        preview = false;

        warning = "CARE!!! IF A CUSTOM LEVEL WIHT SAME NAME ALREADY EXISTS IT WILL BE OVERRITTEN!!";
    }


    public override void Draw()
    {
        if (!active) return;
        GUILayout.BeginArea(_rect, skin.box);
        GUILayout.TextArea(warning, skin.label);
        //if (SceneManager.Instance.levelToLoad == null)
        //{
        //    SceneManager.Instance.levelToLoad = new LevelAndType();
        //}
        SceneManager.Instance.levelToLoad.LevelTitle = GUILayout.TextField(SceneManager.Instance.levelToLoad.LevelTitle, skin.textField);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("OKAY!",skin.button))
        {
            LevelEditorParser.Instance.saveLevel();
            if(preview)
            {
                SceneManager.Instance.loadScene(SceneManager.Scene.Game);
            }
            else
            {
                State_Default newState = new State_Default();
                newState.manager = manger;
                manger.changeState(newState);
            }
        }
        if (GUILayout.Button("CANCEL!",skin.button))
        {
            State_Default newState = new State_Default();
            newState.manager = manger;
            manger.changeState(newState);
        }
        GUILayout.EndHorizontal();
        GUILayout.EndArea();
    }

    public override bool mouseOnGui(Vector2 pos)
    {
        if (active)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

using UnityEngine;
using System.Collections;

public class GUI_SaveAndPreview : GUI_Element
{
    public StateManager manager { get; set; }
    
    public GUI_SaveAndPreview(Vector2 pos, Vector2 s, GUISkin sk)
    {
        position = pos;
        size = s;
        skin = sk;
        active = true;
    }

    public override void Draw()
    {
        if (!active) return;
        GUILayout.BeginArea(_rect);
        GUILayout.BeginHorizontal();
        if (SoundButton.newSoundButton("SAVE", skin.button))
        {
            manager.saveLevel(false);
            //LevelEditorParser.Instance.levelName = "TEST";            
            //LevelEditorParser.Instance.saveLevel();
        }
        if (SoundButton.newSoundButton("PREV", skin.button))
        {
            manager.saveLevel(true);
            //LevelEditorParser.Instance.levelName = "TEST";
            //LevelEditorParser.Instance.saveLevel();
            //SceneManager.Instance.loadScene(SceneManager.Scene.Game);
        }
        GUILayout.EndHorizontal();
        GUILayout.EndArea();
    }
}

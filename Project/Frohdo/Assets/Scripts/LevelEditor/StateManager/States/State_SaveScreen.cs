using UnityEngine;
using System.Collections;

public class State_SaveScreen : Editor_State  {
    public StateManager manager;

    public void init()
    {
        manager.guiSaveScreen.active = true;
        manager.objectSelection.active = false;
        manager.colorSelection.active = false;
        manager.layerSelect.active = false;
        manager.backgroundgui.active = false;
        manager.commands.active = false;
        //throw new System.NotImplementedException();
    }

    public void update()
    {
        //throw new System.NotImplementedException();
    }

    public void leftMouseDown()
    {
        Debug.Log("Left mouse down");
        //throw new System.NotImplementedException();
    }

    public void leftMouseUp()
    {
        //throw new System.NotImplementedException();
    }

    public void mouseMove(Vector2 pos)
    {
        //throw new System.NotImplementedException();
    }

    public void updateColor(string color)
    {
        //throw new System.NotImplementedException();
    }

    public void updateObject(GameObject obj)
    {
        //throw new System.NotImplementedException();
    }
}

using UnityEngine;
using System.Collections;

public class State_ObjectSelected : Editor_State
{
    public StateManager manager;
    public void init()
    {
        manager.colorSelection.active = true;
        manager.selected.active = true;
        manager.commands.active = true;
        Debug.Log("State: ObjectSelected");
    }

    public void update()
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

    public void leftMouseDown()
    {
   
    }

    public void leftMouseUp()
    {
    
    }

    public void mouseMove(Vector2 pos)
    {
    
    }
}

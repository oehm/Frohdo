using UnityEngine;
using System.Collections;

public class State_SetObject : Editor_State {

    public StateManager manager { get; set; }

    private GameObject objToSet;

    public void init()
    {
        manager.colorSelection.active = true;
        manager.selected.active = false;
        manager.commands.active = true;
        manager.objToPlace.active = true;
        Debug.Log("State: SetObject");
    }

    public void update()
    {

    }

    public void leftMouseDown()
    {
        //show object and drag it
    }

    public void leftMouseUp()
    {
        //try placing it
    }

    public void mouseMove(Vector2 pos)
    {

    }
}

using UnityEngine;
using System.Collections;

public class State_ObjectSelected : Editor_State
{
    public StateManager manager;

    public GameObject selected;
    public void init()
    {
        manager.colorSelection.active = true ;
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
        manager.currentColor = color;
        ChangeColor command = new ChangeColor();
        command.setUpCommand(selected, color);

        EditCommandManager.Instance.executeCommand(command);
    }

    public void updateObject(GameObject obj)
    {
        manager.currentGameObject = obj;

        State_SetObject newState = new State_SetObject();
        newState.manager = manager;
        manager.changeState(newState);
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

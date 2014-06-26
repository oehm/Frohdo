using UnityEngine;
using System.Collections;

public class State_Move : Editor_State {


    public StateManager manager;

    public GameObject objectToMove;

    private Vector2 mousePos_;
    private Vector2 oldPos_;
    private Command_ChangePosition command_;
    private int matLayer_;

    public void init()
    {
        oldPos_ = objectToMove.transform.localPosition;
        command_ = new Command_ChangePosition();
    }

    public void update()
    {
        
    }

    public void leftMouseDown()
    {
        Debug.Log("Dafuq should not even be possible?!");
    }

    public void leftMouseUp()
    {
        if(!manager.guiController.mouseOnGui(mousePos_))
        {
            command_.setUpCommand(oldPos_, objectToMove.transform.localPosition, manager.currentLayer, objectToMove);
            if(!EditCommandManager.Instance.executeCommand(command_))
            {
                Debug.Log("Move fail!");
                objectToMove.transform.localPosition = oldPos_;
            }
        }
        else
        {
            objectToMove.transform.localPosition = oldPos_;
        }
        //Debug.Log("State change!");
        State_ObjectSelected newState = new State_ObjectSelected();
        newState.manager = manager;
        newState.selected = objectToMove;
        manager.changeState(newState);
    }

    public void mouseMove(Vector2 pos)
    {
        mousePos_ = pos;
        Gridable grid = manager.currentGameObject.GetComponent<Gridable>();
        objectToMove.transform.localPosition = EditorHelper.getLocalObjectPosition(mousePos_, manager.layers[manager.currentLayer], grid);
    }

    public void updateColor(string color)
    {
        throw new System.NotImplementedException();
    }

    public void updateObject(GameObject obj)
    {
        throw new System.NotImplementedException();
    }
}

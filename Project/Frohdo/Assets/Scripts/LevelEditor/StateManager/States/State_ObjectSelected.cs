using UnityEngine;
using System.Collections;

public class State_ObjectSelected : Editor_State
{
    public StateManager manager;

    public GameObject selected;

    private Vector2 mousePos;
    public void init()
    {
        manager.colorSelection.active = true ;
        manager.selected.active = true;
        manager.commands.active = true;
        Debug.Log("State: ObjectSelected");

        manager.selected.obj = selected;
        manager.conditionVarnishable.selectedObject = selected;
        manager.conditionVarnishable.checkCondition();
        manager.selected.varnishable = manager.conditionVarnishable.isFullfilled;

        mousePos = new Vector2(0, 0);
    }

    public void update()
    {
       
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
        //try to select sth
        Vector2 matIndex = EditorHelper.getMatIndex(EditorHelper.localMouseToLocalLayer(mousePos, GameObject.Find("SceneObjects").GetComponentsInChildren<Layer>()[manager.currentLayer].gameObject, true), Editor_Grid.Instance.planeSizes[manager.currentLayer]);
        if (matIndex.x > 0 && matIndex.y > 0 && matIndex.x < Editor_Grid.Instance.planeSizes[manager.currentLayer].x && matIndex.y < Editor_Grid.Instance.planeSizes[manager.currentLayer].y)
        {
            GameObject select = Editor_Grid.Instance.levelGrid[manager.currentLayer][(int)matIndex.x][(int)matIndex.y];
            if (select != null)
            {
                selected = select;
            }
            //Wanna switch to default or not?!
            else if(!manager.guiController.mouseOnGui(mousePos))
            {
                State_Default newState = new State_Default();
                newState.manager = manager;
                manager.changeState(newState);
            }
        }
    }

    public void leftMouseUp()
    {

    }

    public void mouseMove(Vector2 pos)
    {
        mousePos = pos;
    }
}

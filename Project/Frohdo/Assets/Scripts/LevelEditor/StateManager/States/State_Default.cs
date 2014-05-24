using UnityEngine;
using System.Collections;

public class State_Default : Editor_State {

    public StateManager manager { get; set; }

    private Vector2 mousePos;

    public void init()
    {
        manager.colorSelection.active = true;
        manager.selectedGui.active = false;
        manager.commands.active = true;
        manager.objectSelection.markObject(false);
        manager.objectSelection.active = true;
        manager.backgroundgui.popUp = false;
        manager.guiSaveScreen.active = false;
        manager.backgroundgui.active = true;
        manager.layerSelect.active = true;
        mousePos = new Vector2(0, 0);
    }

    public void update()
    {

    }

    public void leftMouseDown()
    {
        if (manager.layerSelect.mouseOnGui(mousePos))
        //if (manager.guicontroller.mouseOnGui(mousePos))
        {
            Debug.Log("MOUSE ON LAYER SELECT!");
            return;
        }
        Vector2 matIndex = EditorHelper.getMatIndex(EditorHelper.localMouseToLocalLayer(mousePos, GameObject.Find("SceneObjects").GetComponentsInChildren<Layer>()[manager.currentLayer].gameObject,true),Editor_Grid.Instance.planeSizes[manager.currentLayer]);
        if(matIndex.x >= 0 && matIndex.y >= 0 && matIndex.x < Editor_Grid.Instance.planeSizes[manager.currentLayer].x &&  matIndex.y < Editor_Grid.Instance.planeSizes[manager.currentLayer].y)
        {
            GameObject select = Editor_Grid.Instance.levelGrid[manager.currentLayer][(int)matIndex.x][(int)matIndex.y];
            if (select != null)
            {
                State_ObjectSelected newState = new State_ObjectSelected();
                newState.manager = manager;
                newState.selected = select;
                manager.changeState(newState);
                return;
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


    public void updateColor(string color)
    {
    }

    public void updateObject(GameObject obj)
    {
        State_SetObject newState = new State_SetObject();
        newState.manager = manager;
        manager.changeState(newState);

    }
}

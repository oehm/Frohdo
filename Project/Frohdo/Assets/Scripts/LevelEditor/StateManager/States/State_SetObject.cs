﻿using UnityEngine;
using System.Collections;

public class State_SetObject : Editor_State {

    public StateManager manager { get; set; }

    private GameObject objToSet;
    private Vector2 mousePos;

    public void init()
    {
        manager.colorSelection.active = true;
        manager.selected.active = false;
        manager.commands.active = true;
        manager.objToPlace.active = true;
        manager.objToPlace.selectObj(manager.currentGameObject, manager.currentColor);
        manager.objectSelection.changeColor(manager.currentColor);
        Debug.Log("State: SetObject");

        mousePos = new Vector2(0, 0);
        objToSet = null;
    }

    public void update()
    {

    }

    public void leftMouseDown()
    {
        //try to select sth
        Vector2 matIndex = EditorHelper.getMatIndex(EditorHelper.localMouseToLocalLayer(mousePos, GameObject.Find("SceneObjects").GetComponentsInChildren<Layer>()[manager.currentLayer].gameObject, true), Editor_Grid.Instance.planeSizes[manager.currentLayer]);
        if (matIndex.x >= 0 && matIndex.y >= 0 && matIndex.x < Editor_Grid.Instance.planeSizes[manager.currentLayer].x && matIndex.y < Editor_Grid.Instance.planeSizes[manager.currentLayer].y)
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


        if (manager.guiController.mouseOnGui(mousePos) ) return;
        Gridable grid = manager.currentGameObject.GetComponent<Gridable>();
        Layer layer = manager.layers[manager.currentLayer].GetComponent<Layer>();
        objToSet = layer.AddLevelObjectByName(manager.currentGameObject.name, manager.currentColor, EditorHelper.getLocalObjectPosition(mousePos, layer.gameObject, grid), manager.currentLayer, true);
    }

    public void leftMouseUp()
    {
        if (objToSet == null) return;
        //try placing it
        InsertObject command = new InsertObject();
        command.setUpCommand(objToSet, manager.layers[manager.currentLayer], manager.currentLayer);
        EditCommandManager.Instance.executeCommand(command);

        GameObject.Destroy(objToSet);
        objToSet = null;
    }

    public void mouseMove(Vector2 pos)
    {
        mousePos = pos;
        Gridable grid = manager.currentGameObject.GetComponent<Gridable>();        
        if(objToSet != null)
        {
            objToSet.transform.localPosition = EditorHelper.getLocalObjectPosition(mousePos, manager.layers[manager.currentLayer],grid);
        }
    }


    public void updateColor(string color)
    {
        manager.currentColor = color;
        manager.objectSelection.changeColor(color);
        manager.objToPlace.selectObj(manager.currentGameObject, color);
    }

    public void updateObject(GameObject obj)
    {
        manager.currentGameObject = obj;

        State_SetObject newState = new State_SetObject();
        newState.manager = manager;
        manager.changeState(newState);
    }
}
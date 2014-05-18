﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StateManager : MonoBehaviour
{
    public GUI_Controller_Editor guiController;
    public GUI_ColorSelection colorSelection;
    public GUI_ObjectSelection objectSelection;
    public GUI_SaveAndPreview saveAndPreview;
    public GUI_Commands commands;
    public GUI_Selected selected;
    public GUI_ObjectToPlace objToPlace;
    public GUI_LayerSelect layerSelect;

    public string currentColor;
    public GameObject currentGameObject;
    public int currentLayer;

    public GameObject[] layers;


    private List<SCondition> conditions;
    public SCondition_CharacterSet conditionCharacterSet;
    public SCondition_Varnishable conditionVarnishable;
    private Editor_State curState { get; set; }

    void Awake()
    {
        layers = new GameObject[GlobalVars.Instance.LayerCount];      

        State_Default state = new State_Default();
        state.manager = this;
        curState = state;

        currentColor = "W";
        currentGameObject = null;
        currentLayer = GlobalVars.Instance.playLayer;
        //Conditions
        conditions = new List<SCondition>();
        conditionCharacterSet = new SCondition_CharacterSet();
        conditions.Add(conditionCharacterSet);
        conditionVarnishable = new SCondition_Varnishable();
    }
    void Start()
    {
        conditionCharacterSet.playLayer = layers[GlobalVars.Instance.playLayer];
        curState.init();
        updateLayer(currentLayer);
    }

    void Update()
    {
        foreach(SCondition c in conditions)
        {
            c.checkCondition();
        }
        saveAndPreview.active = conditionCharacterSet.isFullfilled;
        objectSelection.showCharacter((!conditionCharacterSet.isFullfilled && currentLayer == GlobalVars.Instance.playLayer) );
        curState.update();
    }

    public void changeState(Editor_State newState)
    {
        curState = newState;
        curState.init();
    }

    public void updateColor(params object[] parameter)
    {
        string color = parameter[0] as string;
        curState.updateColor(color);
    }

    public void updateObject(params object[] paramter)
    {
        GameObject obj = paramter[0] as GameObject;
        curState.updateObject(obj);
    }

    //The selcted object is soted in the state. delete this not the prefab..
    public void deleteObject(GameObject obj, int layerIndex)
    {
        DeleteObj command = new DeleteObj();
        command.setUpCommand(obj, layerIndex);
        EditCommandManager.Instance.executeCommand(command);
        State_Default newState = new State_Default();
        newState.manager = this;
        changeState(newState);
    }

    public void updateLayer(params object[] paramter)
    {
        GameObject.Find("grid" + currentLayer.ToString()).GetComponent<Renderer>().enabled = false;
        
        int? layerIndex = paramter[0] as int?;
        currentLayer = layerIndex.Value;

        GameObject.Find("grid" + currentLayer.ToString()).GetComponent<Renderer>().enabled = true;
    }

    public void updateVisibility(params object[] paramter)
    {
        bool? newValue = paramter[0] as bool?;
        int? layerIndex = paramter[1] as int?;

        foreach (Renderer r in layers[layerIndex.Value].GetComponentsInChildren<Renderer>())
        {
            r.enabled = newValue.Value;
        }

    }

    public void leftMouseDown()
    {
        curState.leftMouseDown();
    }

    public void leftMouseUp()
    {
        curState.leftMouseUp();
    }

    public void mouseMove(Vector2 pos)
    {
        curState.mouseMove(pos);
    }


}
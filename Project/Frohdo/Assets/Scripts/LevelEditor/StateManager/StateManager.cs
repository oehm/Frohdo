using UnityEngine;
using System.Collections;

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

    private Editor_State curState { get; set; }

    void Awake()
    {
        State_Default state = new State_Default();
        state.manager = this;
        curState = state;

        currentColor = "W";
        currentGameObject = null;
        currentLayer = 2;

        layers = new GameObject[GlobalVars.Instance.LayerCount];
    }

    void Start()
    {
        curState.init();
        updateLayerMask();
    }

    void Update()
    {
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

    public void updateLayer(params object[] paramter)
    {
        int? layerIndex = paramter[0] as int?;
        currentLayer = layerIndex.Value;
        
        updateLayerMask();
    }

    public void updateVisibility(params object[] paramter)
    {
        bool? newValue = paramter[0] as bool?;
        int? layerIndex = paramter[1] as int?;

        updateLayerMask();
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

    private void updateLayerMask()
    {
        //Generate CullingMask for camera:
        int cullingMask = 0;
        cullingMask += 1 << 0; //Default;
        cullingMask += 1 << 1; //transparentFX;
        cullingMask += 1 << 2; //ignore Raycast;

        //iterate over visible layer
        for(int i=0; i< GlobalVars.Instance.LayerCount; i++)
        {
            if (layerSelect.visible[i].visible) cullingMask += 1 << i + 8; //set layer[i] visible
            if(i == currentLayer)
            {
                cullingMask += 1 << i + 14; //for viewing grig                
            }
            Camera.main.cullingMask = cullingMask;
        }
    }
}

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
        Debug.Log("Selected Color: " + color);
        curState.updateColor(color);
    }

    public void updateObject(params object[] paramter)
    {
        GameObject obj = paramter[0] as GameObject;
        Debug.Log("Selected Obj: " + obj);
        curState.updateObject(obj);
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

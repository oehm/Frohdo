using UnityEngine;
using System.Collections;

public class BasicGui : MonoBehaviour {

    public GameObjectController gm;
    public GameObject samples;
    
    private string levelName = "Enter level name";
    public string sizeX = "10";
    public string sizeY = "10";

    private int toolbarInt = 1;
    bool[] layerVisible;
    private string[] toolbarStrings = { "Layer 1", "Layer 2", "Layer 3" };

	private int buttonW = 100;
	private int buttonH = 100;

    private Vector2 scrollViewVector = Vector2.zero;
    //Styles
    
    // Use this for initialization
	void Start () {
        layerVisible = new bool[3];
        layerVisible[0] = true;
        layerVisible[1] = true;
        layerVisible[2] = true;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI()
    {
		GUI.BeginGroup(new Rect(0,0,250,Screen.height));
		//Boxes
	        GUI.Box(new Rect(0, 0, 250, Screen.height),"");
	        //Text
	        levelName = GUI.TextArea(new Rect(5, 2, 240, 20), levelName);	
	        //CheckBox
            layerVisible[0] = GUI.Toggle(new Rect(25, 100, 70, 30), layerVisible[0], new GUIContent("L1"));
            layerVisible[1] = GUI.Toggle(new Rect(95, 100, 70, 30), layerVisible[1], new GUIContent("L2"));
            layerVisible[2] = GUI.Toggle(new Rect(170, 100, 70, 30), layerVisible[2], new GUIContent("L3"));
            
            toolbarInt = GUI.Toolbar(new Rect(5, 70, 240, 30), toolbarInt, toolbarStrings);
            
            gm.setActiveLayer(toolbarInt);
            gm.setVisible(layerVisible);
            //label
			//GUI.Label (new Rect (5, 130, 240, 20), "Objectlist of placeable Items");
	        //ScrollView
	        scrollViewVector = GUI.BeginScrollView(new Rect(5, 145, 240, 450), scrollViewVector, new Rect(0, 0, 200, 10*(buttonH/2+5)));
			int curX = 5;
			int curY = 5;
			Transform [] samplesItems = samples.GetComponentsInChildren<Transform>();
            for (int i = 1; i < samplesItems.Length; i++)
            {
                GameObject t = samplesItems[i].gameObject;
                if (GUI.Button(new Rect(curX, curY, buttonW, buttonH), new GUIContent(t.name)))
                {
                    gm.setElement(t);
                    //gm.setElement().prefab);
                }
                if (curX + 2 * buttonW + 5 > 240)
                {
                    curX = 5;
                    curY += buttonH + 5;
                }
                else
                {
                    curX += buttonW + 5;
                }
            }
	        GUI.EndScrollView();
		GUI.EndGroup();
    }

}

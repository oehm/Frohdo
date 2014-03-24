using UnityEngine;
using System.Collections;

public class BasicGui : MonoBehaviour {

    public LayerController layerController;
    
    private string levelName = "Enter level name";
    public string sizeX = "10";
    public string sizeY = "10";

    private int toolbarInt = 0;
    private string[] toolbarStrings = { "Layer 1", "Layer 2", "Layer 3" };

	private int stringCount = 20;
	private int buttonW = 100;
	private int buttonH = 100;

    private Vector2 scrollViewVector = Vector2.zero;
    //Styles
    
    // Use this for initialization
	void Start () {
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
			GUI.Label (new Rect (5, 20, 100, 20), "level width");
			GUI.Label (new Rect (105, 20, 100, 20), "level height");
			sizeX = GUI.TextArea(new Rect(5, 40, 100, 20), sizeX);
	        sizeY = GUI.TextArea(new Rect(105, 40, 100, 20), sizeY);
			
	        //Toolbar
	        toolbarInt = GUI.Toolbar(new Rect(5, 70, 240, 60), toolbarInt, toolbarStrings);
	        //label
			//GUI.Label (new Rect (5, 130, 240, 20), "Objectlist of placeable Items");
	        //ScrollView
	        scrollViewVector = GUI.BeginScrollView(new Rect(5, 145, 240, 450), scrollViewVector, new Rect(0, 0, 200, stringCount*(buttonH/2+5)));
			int curX = 5;
			int curY = 5;
			for (int i=0; i<stringCount; i++) {
				GUI.Button(new Rect(curX,curY,buttonW,buttonH),"Grid "+i);
				if(curX + 2* buttonW + 5 > 240){
					curX = 5;
					curY += buttonH + 5;
				}
				else{
					curX += buttonW +5 ;
				}
			}
			
	        GUI.EndScrollView();
		GUI.EndGroup();

        layerController.SetActiveLayer(toolbarInt);
    }


}

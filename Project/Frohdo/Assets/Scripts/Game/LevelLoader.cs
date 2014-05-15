using UnityEngine;
using System.Collections;
using System;

public class LevelLoader : MonoBehaviour {


    public GameObject SceneObjects;
    
    public GameObject layerPrefab_;

    public Camera camera_;

    public InputControllerGame inputController_;


	// Use this for initialization
	void Start () {
	    //LoadLevel(Application.dataPath+"/Levels/Custom/test.xml");
        //string path = Environment.GetEnvironmentVariable("SelectedLevel", EnvironmentVariableTarget.Process);
        string path = SceneManager.Instance.levelToLoad;
        LoadLevel(path); 
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void LoadLevel(string path)
    {
        LevelXML levelXML = XML_Loader.Load(path);

        if (levelXML.layers.Count != GlobalVars.Instance.LayerCount)
        {
            string error = "Unable to load level: " + levelXML.layers.Count + " != " + GlobalVars.Instance.LayerCount;
            Debug.Log(error);
            throw new System.Exception(error);
        }

        SceneManager.Instance.background.GetComponent<Colorable>().colorString = levelXML.backgroundColor;

        for (int i = 0; i < levelXML.layers.Count; i++)
        {
            GameObject layerObject = (GameObject)Instantiate(layerPrefab_);
            layerObject.transform.parent = SceneObjects.transform;
            Layer layerScript = layerObject.GetComponent<Layer>();
            LayerXML layerXML = levelXML.layers[i];

            Vector3 position = camera_.transform.position;
            position.z = GlobalVars.Instance.layerZPos[i];
            Vector2 parallax = GlobalVars.Instance.layerParallax[i];
            bool isPlayLayer = (i == GlobalVars.Instance.playLayer);

            layerObject.name = "Layer " + i;
            layerObject.transform.position = position;
            layerScript.parallaxFactor_ = parallax;
            layerScript.hasColliders_ = isPlayLayer;
            layerScript.camera_ = camera_;

            for (int j = 0; j < layerXML.levelObjects.Count; j++)
            {
                LevelObjectXML levelObjectXML = layerXML.levelObjects[j];
                

                GameObject levelObjectObject =  layerScript.AddLevelObjectByName(levelObjectXML.name, levelObjectXML.color, levelObjectXML.pos.Vector2);
               
            }

            CharacterObjectXML characterXML = layerXML.Character;

            if (characterXML != null)
            {
                GameObject characterObject = layerScript.AddCharacter(characterXML.pos.Vector2);

                inputController_.character_ = characterObject.GetComponentInChildren<Character>();

                camera_.GetComponent<CameraMovementGame>().character_ = characterObject;
                continue;
            }

        }
    }

}

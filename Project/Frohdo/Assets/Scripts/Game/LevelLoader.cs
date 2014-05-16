﻿using UnityEngine;
using System.Collections;
using System;

public class LevelLoader : MonoBehaviour {


    public GameObject SceneObjects;
    
    public GameObject layerPrefab_;

    public Camera camera_;

    public InputControllerGame inputController_;


	// Use this for initialization
	void Start () {
        string path = SceneManager.Instance.levelToLoad;
        bool editor = SceneManager.Instance.loadLevelToEdit;
        LoadLevel(path, editor);
        SceneManager.Instance.loadLevelToEdit = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void LoadLevel(string path, bool editor)
    {
        LevelXML levelXML = XML_Loader.Load(path);

        if (levelXML.layers.Count != GlobalVars.Instance.LayerCount)
        {
            string error = "Unable to load level: " + levelXML.layers.Count + " != " + GlobalVars.Instance.LayerCount;
            Debug.Log(error);
            throw new System.Exception(error);
        }

        SceneManager.Instance.background.GetComponent<Colorable>().colorString = levelXML.backgroundColor;

        if (editor)
        {
            Editor_Grid.Instance.initGrid(levelXML.size.Vector2);
        }
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
            if(editor)
            {
                GameObject bg = GameObject.Instantiate(Editor_Grid.Instance.layerBg_pref, new Vector3(0, 0, GlobalVars.Instance.layerZPos[i] + 0.02f), Quaternion.identity) as GameObject;
                bg.transform.localScale = Editor_Grid.Instance.planeSizes[i];
                bg.transform.parent = GameObject.Find("SceneObjects").GetComponentsInChildren<Layer>()[i].transform;
                bg.layer = 14 + i;
            }

            for (int j = 0; j < layerXML.levelObjects.Count; j++)
            {
                LevelObjectXML levelObjectXML = layerXML.levelObjects[j];
                

                GameObject levelObjectObject =  layerScript.AddLevelObjectByName(levelObjectXML.name, levelObjectXML.color, levelObjectXML.pos.Vector2);
                if(editor)
                {
                    InsertObject command = new InsertObject();
                    command.setUpCommand(levelObjectXML, layerObject.GetComponentInChildren<Layer>(), i);
                    EditCommandManager.Instance.executeCommand(command);
                }
                //this is ugly but i dont know better atm
                //if (levelObjectXML.name.Equals("Character"))
                //{
                //    inputController_.character_ = levelObjectObject.GetComponentInChildren<Character>();

                //    camera_.GetComponent<CameraMovementGame>().character_ = levelObjectObject;
                //    continue;
                //}
               
            }

            CharacterObjectXML characterXML = layerXML.Character;

            if (characterXML != null)
            {
                GameObject characterObject = layerScript.AddCharacter(characterXML.pos.Vector2,editor);
                if (!editor)
                {
                    inputController_.character_ = characterObject.GetComponentInChildren<Character>();

                    camera_.GetComponent<CameraMovementGame>().character_ = characterObject;
                    continue;
                }
            }

        }
    }

}

﻿using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class LevelLoader : MonoBehaviour
{
    public enum LevelType { Story, Custom, Normal }

    public GameObject SceneObjects;

    public GameObject layerPrefab_;

    public CameraMovementGame cameraMovementGame_;

    private Camera camera_;

    public InputControllerGame inputController_;

    public void load()
    {
        string path = SceneManager.Instance.levelToLoad.LeveltoLoad;
        bool editor = SceneManager.Instance.loadLevelToEdit;
        LevelType type = SceneManager.Instance.levelToLoad.type;
        camera_ = ForceAspectRatio.CameraMain;
        LoadLevel(path, editor, type);
        SceneManager.Instance.loadLevelToEdit = false;
    }


    private void LoadLevel(string path, bool editor, LevelType type)
    {
        ScreenShotManager.Instance.reset(); //resets manager. removes all old screens from last level.. 
        LevelXML levelXML;
        Vector2 characterPos = new Vector2(0, 0);
        if (type == LevelType.Story)
        {
            levelXML = XML_Loader.LoadFromResources(path); //for story levels!!
        }
        else
        {
            levelXML = XML_Loader.Load(path);
        }

        if (levelXML.layers.Count != GlobalVars.Instance.LayerCount)
        {
            string error = "Unable to load level: " + levelXML.layers.Count + " != " + GlobalVars.Instance.LayerCount;
            Debug.Log(error);
            throw new System.Exception(error);
        }

        SceneManager.Instance.background.GetComponentInChildren<Colorable>().colorString = levelXML.backgroundColor;


        for (int i = 0; i < levelXML.layers.Count; i++)
        {
            GameObject layerObject = (GameObject)Instantiate(layerPrefab_);
            layerObject.transform.parent = SceneObjects.transform;
            Layer layerScript = layerObject.GetComponent<Layer>();
            LayerXML layerXML = levelXML.layers[i];
            
            Vector3 position = camera_.gameObject.transform.position;
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

                if (!editor)
                {
                    layerScript.AddLevelObjectByName(levelObjectXML.name, levelObjectXML.color, levelObjectXML.pos.Vector2, i, editor);
                }
                else
                {
                    InsertObject command = new InsertObject();
                    command.setUpCommand(levelObjectXML, layerObject.GetComponentInChildren<Layer>(), i);
                    EditCommandManager.Instance.executeCommand(command);
                }
            }
            if(editor)
            {
                EditCommandManager.Instance.resetHistory();
            }
            CharacterObjectXML characterXML = layerXML.Character;
            if (characterXML != null)
            {
                characterPos = characterXML.pos.Vector2;
                if (!editor)
                {
                    GameObject characterObject = layerScript.AddCharacter(characterXML.pos.Vector2, i, editor);


                    inputController_.character_ = characterObject.GetComponentInChildren<Character>();

                    cameraMovementGame_.character_ = characterObject;
                }
                else
                {
                    InsertObject command = new InsertObject();
                    LevelObjectXML l = new LevelObjectXML();
                    l.color = "";
                    l.name = "Character";
                    l.pos = characterXML.pos;
                    command.setUpCommand(l, layerObject.GetComponentInChildren<Layer>(), i);
                    EditCommandManager.Instance.executeCommand(command);
                }
            }

            if (i < GlobalVars.Instance.playLayer)
            {
                moveToLayer(layerObject, LayerMask.NameToLayer("BackgroundDrawings"));
            }
        }

        if (SceneManager.Instance.levelToLoad.currentThumb != null) ScreenShotManager.Instance.addCurrentThumb(SceneManager.Instance.levelToLoad.currentThumb);

        ScoreController.Instance.reset(SceneManager.Instance.levelToLoad, !editor);

        if (!editor)
        {
            if (type == LevelType.Custom && SceneManager.Instance.levelToLoad.currentThumb == null)
            {
                Camera.main.transform.position = new Vector3(characterPos.x, characterPos.y, GlobalVars.Instance.mainCamerZ);
                ScreenShotManager.Instance.takeScreenShot(false);
            }
        }
    }


    private void moveToLayer(GameObject root, int layer)
    {
        root.layer = layer;
        foreach (Transform child in root.transform)
            moveToLayer(child.gameObject, layer);
    }
}

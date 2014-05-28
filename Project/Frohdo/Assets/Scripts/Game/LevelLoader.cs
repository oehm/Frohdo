using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class LevelLoader : MonoBehaviour
{
    public enum LevelType { Story, Custom, Normal }

    public GameObject SceneObjects;

    public GameObject layerPrefab_;

    public Camera camera_;

    public InputControllerGame inputController_;

    // Use this for initialization
    void Awake()
    {
        string path = SceneManager.Instance.levelToLoad.LeveltoLoad;
        bool editor = SceneManager.Instance.loadLevelToEdit;
        if (editor)
        {
            SceneObjects = GameObject.Find("SceneObjects");
            camera_ = Camera.main;
        }
        LoadLevel(path, editor, SceneManager.Instance.levelToLoad.type);
        SceneManager.Instance.loadLevelToEdit = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LoadLevel(string path, bool editor, LevelType type)
    {
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

        if (editor)
        {
            Editor_Grid.Instance.initGrid(GlobalVars.Instance.maxLevelSize);
            LevelEditorParser.Instance.initEmpty();
            StateManager manager = GameObject.Find("SceneController").GetComponentInChildren<StateManager>() as StateManager;
            manager.changeBackgroundColor(levelXML.backgroundColor);
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
            if (editor)
            {
                StateManager manager = GameObject.Find("SceneController").GetComponentInChildren<StateManager>() as StateManager;
                manager.layers[i] = layerObject;
                GameObject bg = GameObject.Instantiate(Editor_Grid.Instance.layerBg_pref, new Vector3(0, 0, GlobalVars.Instance.layerZPos[i] + 0.02f), Quaternion.identity) as GameObject;
                bg.transform.localScale = Editor_Grid.Instance.planeSizes[i];
                bg.transform.parent = GameObject.Find("SceneObjects").GetComponentsInChildren<Layer>()[i].transform;
                bg.name = "grid"+i.ToString();
                bg.GetComponent<Renderer>().enabled = false;
                bg.GetComponent<Renderer>().material.mainTextureScale = Editor_Grid.Instance.planeSizes[i];
            }

            for (int j = 0; j < layerXML.levelObjects.Count; j++)
            {
                LevelObjectXML levelObjectXML = layerXML.levelObjects[j];

                if (!editor)
                {
                    //GameObject levelObjectObject =
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

                    camera_.GetComponent<CameraMovementGame>().character_ = characterObject;
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

        if (!editor && type != LevelType.Story)
        {
            ScoreController.Instance.LevelHash = ScoreController.Instance.getMD5ofFile(path); //just temporary, calculate it later

            WWW thumbdownload = new WWW(SceneManager.Instance.levelToLoad.thumbpath);

            if ( thumbdownload.error != "" && type == LevelType.Custom)
            {
                Camera.main.transform.position = new Vector3(characterPos.x, characterPos.y, GlobalVars.Instance.mainCamerZ);
                ScreenShotManager.Instance.takeScreenShot();
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

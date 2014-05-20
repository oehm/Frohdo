using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SetUpManager : MonoBehaviour
{
    //GUISizes
    public GameObject layerPrefab_;
    public GameObject layerBgPrefab_;

    public string savePath;

    private GameObject sceneController;
    private GameObject sceneObjects;
    public GUISkin skin;

    //Controller to Init
    private GUI_Controller_Editor guiController;
    private LevelLoader Levelloader;
    private RenderGameObjectToTexture renderToTexture;
    //Other Controller
    public StateManager stateManager;
    private string levelName;
    void Awake()
    {
        sceneController = GameObject.Find("SceneController");
        sceneObjects = GameObject.Find("SceneObjects");
        levelName = "Enter Level Name";
        LevelEditorParser.Instance.savePath = savePath;
        Editor_Grid.Instance.layerBg_pref = layerBgPrefab_;

        if (SceneManager.Instance.loadLevelToEdit)
        {
            Levelloader = createController("LevelLoader", "LevelLoader") as LevelLoader;
            Levelloader.layerPrefab_ = layerPrefab_;
            Levelloader.camera_ = Camera.main;
            Levelloader.SceneObjects = sceneObjects;
            Levelloader.LoadLevel(SceneManager.Instance.levelToLoad, true);
            SceneManager.Instance.loadLevelToEdit = false;
            Levelloader.gameObject.SetActive(false);
        }
        else
        {
            setUpEmpyScene();
        }

    }


    private Component createController(string name, string type)
    {
        GameObject o = new GameObject();
        o.transform.parent = sceneController.transform;
        o.name = name;
        o.AddComponent(type);
        Component c = o.GetComponent(type);
        return c;
    }

    private void setUpEmpyScene()
    {
        Editor_Grid.Instance.initGrid(GlobalVars.Instance.maxLevelSize);
        for (int i = 0; i < GlobalVars.Instance.LayerCount; i++)
        {
            GameObject layerObject = (GameObject)Instantiate(layerPrefab_);
            layerObject.transform.parent = sceneObjects.transform;

            stateManager.layers[i] = layerObject;

            Layer layerScript = layerObject.GetComponent<Layer>();

            Vector3 position = Camera.main.transform.position;
            position.z = GlobalVars.Instance.layerZPos[i];
            Vector2 parallax = GlobalVars.Instance.layerParallax[i];
            bool isPlayLayer = (i == GlobalVars.Instance.playLayer);

            layerObject.name = "Layer " + i;
            layerObject.transform.position = position;
            layerScript.parallaxFactor_ = parallax;
            layerScript.hasColliders_ = isPlayLayer;
            layerScript.camera_ = Camera.main;

            GameObject bg = GameObject.Instantiate(Editor_Grid.Instance.layerBg_pref, new Vector3(0, 0, GlobalVars.Instance.layerZPos[i] + 0.02f), Quaternion.identity) as GameObject;
            bg.transform.localScale = Editor_Grid.Instance.planeSizes[i];
            bg.GetComponent<Renderer>().material.mainTextureScale =Editor_Grid.Instance.planeSizes[i];
            bg.transform.parent = GameObject.Find("SceneObjects").GetComponentsInChildren<Layer>()[i].transform;
            bg.name = "grid"+i.ToString();
            bg.GetComponent<Renderer>().enabled = false;
        }
        LevelEditorParser.Instance.initEmpty();
    }
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SetUpManager : MonoBehaviour
{
    public GameObject layerPrefab_;
    public GameObject layerBgPrefab_;

    //public string savePath;

    public GUISkin skin;

    //Other Controller
    public StateManager stateManager;
    public LevelLoader levelLoader;

    void Awake()
    {
        //LevelEditorParser.Instance.savePath = savePath;
        LevelEditorParser.Instance.initEmpty();

        Editor_Grid.Instance.layerBg_pref = layerBgPrefab_;
        Editor_Grid.Instance.initGrid(GlobalVars.Instance.maxLevelSize);

    }

    void Start()
    {
        levelLoader.load();

        setUpGrid();

        stateManager.init();

        stateManager.changeBackgroundColor(SceneManager.Instance.background.GetComponent<Colorable>().colorString);
    }

    private void setUpGrid()
    {
        for (int i = 0; i < GlobalVars.Instance.LayerCount; i++)
        {
            GameObject layerObject = GameObject.Find("Layer " + i);

            stateManager.layers[i] = layerObject;

            GameObject bg = GameObject.Instantiate(Editor_Grid.Instance.layerBg_pref, new Vector3(0, 0, GlobalVars.Instance.layerZPos[i] + 0.02f), Quaternion.identity) as GameObject;
            bg.transform.localScale = Editor_Grid.Instance.planeSizes[i];
            bg.GetComponent<Renderer>().material.mainTextureScale = Editor_Grid.Instance.planeSizes[i];
            bg.transform.parent = layerObject.transform;
            bg.name = "grid" + i.ToString();
            bg.GetComponent<Renderer>().enabled = false;
        }
    }

    //private void setUpEmpyScene()
    //{
    //    Editor_Grid.Instance.initGrid(GlobalVars.Instance.maxLevelSize);
    //    for (int i = 0; i < GlobalVars.Instance.LayerCount; i++)
    //    {
    //        GameObject layerObject = (GameObject)Instantiate(layerPrefab_);
    //        layerObject.transform.parent = sceneObjects.transform;

    //        stateManager.layers[i] = layerObject;

    //        Layer layerScript = layerObject.GetComponent<Layer>();

    //        Vector3 position = Camera.main.transform.position;
    //        position.z = GlobalVars.Instance.layerZPos[i];
    //        Vector2 parallax = GlobalVars.Instance.layerParallax[i];
    //        bool isPlayLayer = (i == GlobalVars.Instance.playLayer);

    //        layerObject.name = "Layer " + i;
    //        layerObject.transform.position = position;
    //        layerScript.parallaxFactor_ = parallax;
    //        layerScript.hasColliders_ = isPlayLayer;
    //        layerScript.camera_ = Camera.main;

    //        GameObject bg = GameObject.Instantiate(Editor_Grid.Instance.layerBg_pref, new Vector3(0, 0, GlobalVars.Instance.layerZPos[i] + 0.02f), Quaternion.identity) as GameObject;
    //        bg.transform.localScale = Editor_Grid.Instance.planeSizes[i];
    //        bg.GetComponent<Renderer>().material.mainTextureScale =Editor_Grid.Instance.planeSizes[i];
    //        bg.transform.parent = GameObject.Find("SceneObjects").GetComponentsInChildren<Layer>()[i].transform;
    //        bg.name = "grid"+i.ToString();
    //        bg.GetComponent<Renderer>().enabled = false;
    //    }
    //    LevelEditorParser.Instance.initEmpty();
    //}
}

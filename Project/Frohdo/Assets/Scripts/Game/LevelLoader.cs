using UnityEngine;
using System.Collections;

public class LevelLoader : MonoBehaviour {


    public GameObject SceneObjects;
    
    public GameObject layerPrefab_;

    public Camera camera_;

    public InputControllerGame inputController_;


	// Use this for initialization
	void Start () {
	    LoadLevel(Application.dataPath+"/Levels/Custom/test.xml");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void LoadLevel(string path)
    {
        Level levelXML = XML_Loader.Load(path);

        if (levelXML.layers.Count != GlobalVars.Instance.LayerCount)
        {
            string error = "Unable to load level: " + levelXML.layers.Count + " != " + GlobalVars.Instance.LayerCount;
            Debug.Log(error);
            throw new System.Exception(error);
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

            for (int j = 0; j < layerXML.levelObjects.Count; j++)
            {
                LevelObject levelObjectXML = layerXML.levelObjects[j];

                GameObject levelObjectObject =  layerScript.AddLevelObjectByName(levelObjectXML.name, levelObjectXML.color, levelObjectXML.pos.Vector2);

                //this is ugly but i dont know better atm
                if (levelObjectXML.name.Equals("Character"))
                {
                    inputController_.character_ = levelObjectObject.GetComponentInChildren<CharacterMovement>();

                    camera_.GetComponent<CameraMovementGame>().player_ = levelObjectObject;
                }
            }

        }
    }

    //void createTestXML()
    //{
    //    Level levelXML = new Level();
    //    levelXML.backgroundColor = "B";
    //    levelXML.size = new SerializableVector2(new Vector2(64, 64));
    //    for (int l = 0; l < GlobalVars.Instance.LayerCount; l++)
    //    {
    //        LayerXML layerXML = new LayerXML();
    //        layerXML.layerId = l;
    //        for (int x = -16; x < 16; x++)
    //        {
    //            for (int y = -16; y < 16; y++)
    //            {

    //                if (x + y < -15 || y == -16 || x == -16 || x == 15)//x + y < -15 || 
    //                {
    //                    if (l == GlobalVars.Instance.playLayer)
    //                    {
    //                        if ((x * y) % 2 == 0)
    //                        {
    //                            LevelObject levelObjectXML = new LevelObject();
    //                            levelObjectXML.name = "1x1Tile_Test";
    //                            levelObjectXML.color = "R";
    //                            levelObjectXML.pos = new SerializableVector2(new Vector2(x, y));
    //                            layerXML.levelObjects.Add(levelObjectXML);
    //                        }
    //                        else
    //                        {
    //                            LevelObject levelObjectXML = new LevelObject();
    //                            levelObjectXML.name = "1x1Tile_Test";
    //                            levelObjectXML.color = "G";
    //                            levelObjectXML.pos = new SerializableVector2(new Vector2(x, y));
    //                            layerXML.levelObjects.Add(levelObjectXML);
    //                        }
    //                    }
    //                    else
    //                    {
    //                        if ((x * y) % 5 == 0)
    //                            if ((x * y) % 2 == 0)
    //                            {
    //                                LevelObject levelObjectXML = new LevelObject();
    //                                levelObjectXML.name = "1x1Tile_Test";
    //                                levelObjectXML.color = "B";
    //                                levelObjectXML.pos = new SerializableVector2(new Vector2(x, y));
    //                                layerXML.levelObjects.Add(levelObjectXML);
    //                            }
    //                            else
    //                            {
    //                                LevelObject levelObjectXML = new LevelObject();
    //                                levelObjectXML.name = "1x1Tile_Test";
    //                                levelObjectXML.color = "Y";
    //                                levelObjectXML.pos = new SerializableVector2(new Vector2(x, y));
    //                                layerXML.levelObjects.Add(levelObjectXML);
    //                            }
    //                    }
    //                }
    //            }
    //        }
    //        if (l == GlobalVars.Instance.playLayer)
    //        {
    //            LevelObject levelObjectXML = new LevelObject();
    //            levelObjectXML.name = "Character";
    //            levelObjectXML.color = "C";
    //            levelObjectXML.pos = new SerializableVector2(new Vector2(0, 0));
    //            layerXML.levelObjects.Add(levelObjectXML);
    //        }
    //        levelXML.layers.Add(layerXML);
    //    }

    //    XML_Loader.Save(Application.dataPath + "/Levels/Custom/test.xml", levelXML);
    //}
}

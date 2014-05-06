using UnityEngine;
using System.Collections;

public class LevelLoader : MonoBehaviour {


    public GameObject layerPrefab_;

    public Camera camera_;

    public InputControllerGame inputController_;


	// Use this for initialization
	void Start () {
	    LoadLevel(Application.dataPath+"Levels/Custom/test.xml");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void LoadLevel(string path)
    {
        Level levelXML = XML_Loader.Load(path);

        if (levelXML.layers.Capacity != GlobalVars.Instance.LayerCount)
        {
            string error = "Unable to load level: ";
            Debug.Log(error);
            throw new System.Exception(error);
        }

        for (int i = 0; i < levelXML.layers.Capacity; i++)
        {
            GameObject layerObject = (GameObject)Instantiate(layerPrefab_);
            Layer layerScript = layerObject.GetComponent<Layer>();
            LayerXML layerXML = levelXML.layers[i];

            Vector3 position = camera_.transform.position;
            position.z = GlobalVars.Instance.layerZPos[i];
            Vector2 parallax = GlobalVars.Instance.layerParallax[i];
            bool isPlayLayer = (i == GlobalVars.Instance.playLayer);

            layerScript.camera_ = camera_;
            layerObject.transform.position = position;
            layerScript.parallaxFactor_ = parallax;
            layerScript.hasColliders_ = isPlayLayer;

            for (int j = 0; j < layerXML.levelObjects.Capacity; j++)
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

}

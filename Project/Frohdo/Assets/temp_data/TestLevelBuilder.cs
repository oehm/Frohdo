using UnityEngine;
using System.Collections;

public class TestLevelBuilder : MonoBehaviour {

    public GameObject prefab_;

    public Camera camera_;

    public InputControllerGame inputController_;

	// Use this for initialization
	void Start () {
        BuildLayer(false, 15, new Vector2(  -0.5f, -0.01f));
        BuildLayer(false, 12, new Vector2(  0.0f,   0.00f));
        BuildLayer(true, 10, new Vector2(   0.0f,   0.0f));
        BuildLayer(false, 8, new Vector2(   0.0f,   0.0f));
        BuildLayer(false, 5, new Vector2(   0.5f,   0.0f));
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void BuildLayer(bool isPlayLayer, float distance, Vector2 parallaxFactor)
    {
        GameObject layerObject = (GameObject)Instantiate(prefab_);

        Layer layer = layerObject.GetComponent<Layer>();

        Vector3 position = camera_.transform.position;
        position.z += distance;

        layerObject.transform.position = position;
        layer.camera_ = camera_;
        layer.parallaxFactor_ = parallaxFactor;
        layer.hasColliders_ = isPlayLayer;

        for (int x = -16; x < 16; x++)
        {
            for (int y = -16; y < 16; y++)
            {

                if (x + y < -15 || y == -16 || x == -16 || x == 15)//x + y < -15 || 
                {
                    if (isPlayLayer)
                    {
                        if ((x * y) % 2 == 0)
                        {
                            layer.AddLevelObjectByName("1x1Tile_Test2", "R", new Vector2(x, y));
                        }
                        else
                        {
                            layer.AddLevelObjectByName("1x1Tile_Test", "G", new Vector2(x, y));
                        }
                    }
                    else
                    {
                        if((x * y) % 5 == 0)
                        if ((x * y) % 2 == 0)
                        {
                            layer.AddLevelObjectByName("1x1Tile_Test2", "B", new Vector2(x, y));
                        }
                        else
                        {
                            layer.AddLevelObjectByName("1x1Tile_Test", "Y", new Vector2(x, y));
                        }
                    }
                }
            }
        }
        if (isPlayLayer)
        {
            GameObject character = layer.AddLevelObjectByName("Character", "C", new Vector2(0, 0));

            inputController_.character_ = character.GetComponentInChildren<CharacterMovement>();

            camera_.GetComponent<CameraMovementGame>().player_ = character;
        }
    }
}

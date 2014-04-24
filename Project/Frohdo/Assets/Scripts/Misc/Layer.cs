using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Layer : MonoBehaviour {

    //public
    public Camera camera_;

    //parallaxFactor_ has to be ]-1.0, inf[
    //parallaxFactor_ < 0 means simulation of farther away
    //parallaxFactor_ > 0 means simulation of closer
    public Vector2 parallaxFactor_;
    public bool hasColliders_;

    //private
    //private List<GameObject> levelObjects_;//not sure if i need this


	// Use this for initialization
	void Start () {
	    //levelObjects_ = new List<GameObject>();

        //float distanceToCamera = transform.position.z - camera_.transform.position.z;

        //test setup
        for (int x = -16; x < 16; x ++)
        {
            for (int y = -16; y < 16; y ++)
            {

                if (x + y < -15 || y == -16 || x == -16 || x == 15)
                {
                    if (hasColliders_)
                    {
                        if ((x * y) % 2 == 0)
                        {
                            AddLevelObjectByName("1x1Tile_Test", "R", new Vector2(x, y));
                        }
                        else
                        {
                            AddLevelObjectByName("1x1Tile_Test", "G", new Vector2(x, y));
                        }
                    }
                    else
                    {
                        if ((x * y) % 2 == 0)
                        {
                            AddLevelObjectByName("1x1Tile_Test", "B", new Vector2(x, y));
                        }
                        else
                        {
                            AddLevelObjectByName("1x1Tile_Test", "Y", new Vector2(x, y));
                        }
                    }
                }
            }
        }
	}


    // Update is called once per frame
    void Update()
    {
        //adjust the position according to the parallaxFactor and the camera
        Vector3 newPosition = transform.position;
        newPosition.x = -parallaxFactor_.x * camera_.transform.position.x;
        newPosition.y = -parallaxFactor_.y * camera_.transform.position.y;
        transform.position = newPosition;
	}


    public void AddLevelObjectByName(string prefabName, string colorName, Vector2 position)
    {
        GameObject prefab;
        Color color;
        try
        {
            prefab = LevelObjectController.Instance.GetPrefabByName(prefabName);
            color = LevelObjectController.Instance.GetColor(colorName);
        }
        catch
        {
            Debug.Log("LevelObject not added to Layer: " + prefabName);
            return;
        }

        GameObject levelObject = (GameObject)Instantiate(prefab);
        levelObject.name = prefabName;
        levelObject.transform.parent = transform;
        levelObject.transform.localPosition = position;
        levelObject.renderer.material.color = color;
        levelObject.GetComponent<Collider2D>().enabled = hasColliders_;

        //levelObjects_.Add(levelObject);
    }
}

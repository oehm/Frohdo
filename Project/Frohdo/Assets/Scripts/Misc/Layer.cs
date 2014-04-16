using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Layer : MonoBehaviour {

    //public
    public LevelObjectController levelObjectController_;
    public Camera camera_;

    public Vector2 parallaxFactor_;
    public bool hasColliders_;

    //private
    //private List<GameObject> levelObjects_;//not sure if i need this


	// Use this for initialization
	void Start () {
	    //levelObjects_ = new List<GameObject>();

        //float distanceToCamera = transform.position.z - camera_.transform.position.z;

        for (int x = -6; x <= 6; x += 2)
        {
            for (int y = -6; y <= 6; y += 2)
            {
                AddLevelObjectByName("1x1Tile_Test", new Vector2(x, y));
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

        //test
        //if(Input.GetButton("Jump"))
        //{
        //    AddLevelObjectByName("TestPrefab");
        //}
	}


    public void AddLevelObjectByName(string prefabName, Vector2 position)
    {
        GameObject prefab;
        try
        {
            prefab = levelObjectController_.GetPrefabByName(prefabName);
        }
        catch
        {
            Debug.Log("LevelObject prefab not added to Layer: " + prefabName);
            return;
        }

        GameObject levelObject = (GameObject)Instantiate(prefab);
        levelObject.name = prefabName;
        levelObject.transform.parent = transform;
        levelObject.transform.localPosition = position;
        levelObject.GetComponent<Collider2D>().enabled = hasColliders_;

        //levelObjects_.Add(levelObject);
    }
}

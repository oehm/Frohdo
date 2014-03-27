using UnityEngine;
using System.Collections;

public class ParralxView : MonoBehaviour {

    public float parallaxParam = 0.1f;
    private float paraX = 0;
    // Use this for initialization
	void Start () {
        //paraX = Camera.main.transform.position.x;
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 pos = transform.position;
        pos.x = (paraX - Camera.main.transform.position.x) / parallaxParam;
        transform.position = pos;
	}
}

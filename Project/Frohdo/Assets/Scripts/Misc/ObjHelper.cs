using UnityEngine;
using System.Collections;

public class ObjHelper : MonoBehaviour {

    public string Objname = "";
    public string color = "";
    public Vector2 pos;
    public int height;
    public int width;
    public bool[] hitMat;
  
    // Use this for initialization
	void Start () {
        height = (int)transform.localScale.y;
        width = (int)transform.localScale.x;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

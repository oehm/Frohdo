using UnityEngine;
using System.Collections;

public class LayerController : MonoBehaviour {

    public Transform bg;
    public Transform mg;
    public Transform fg;

    public Grid grid;

    // Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetActiveLayer(int id)
    {
        bg.transform.position = new Vector3(0, 0, 1 - id);
        mg.transform.position = new Vector3(0, 0, -id);
        fg.transform.position = new Vector3(0, 0, -1 - id);

        grid.SetActiveLayer(id);
    }
}

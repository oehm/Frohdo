using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

    private GameObject cam;
	private Vector2 lastmousePos = new Vector2(0,0);
	private bool leftDown = false;

    public float scrollSpeed = 2.0f;
    public GUI_Controller_Editor gui;

    void Start()
    {
        cam = ForceAspectRatio.CameraMain.gameObject;
    }
	
	// Update is called once per frame
	void FixedUpdate(){

		//CameraDrag
		if(leftDown){
            cam.transform.position = cam.transform.position - new Vector3(Input.mousePosition.x - lastmousePos.x, Input.mousePosition.y - lastmousePos.y, 0) / 30;
		}
		lastmousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
		//CameraZoom
        if (Input.GetAxis("Mouse ScrollWheel") < 0 && !gui.mouseOnGui(Input.mousePosition) && cam.camera.fieldOfView < 140)
            cam.camera.fieldOfView += scrollSpeed;
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && !gui.mouseOnGui(Input.mousePosition) && cam.camera.fieldOfView > 10)
            cam.camera.fieldOfView -= scrollSpeed;

        cam.GetComponentsInChildren<Camera>()[1].fieldOfView = cam.camera.fieldOfView;
        cam.GetComponentsInChildren<Camera>()[2].fieldOfView = cam.camera.fieldOfView;
	}

	void Update () {
		if (Input.GetMouseButtonDown (1))
			leftDown = true;
		if (Input.GetMouseButtonUp (1))
			leftDown = false;
	}
}

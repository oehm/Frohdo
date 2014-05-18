using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

	private Vector2 lastmousePos = new Vector2(0,0);
	private bool leftDown = false;

    public float scrollSpeed = 2.0f;
    public GUI_Controller_Editor gui;

	
	// Update is called once per frame
	void FixedUpdate(){

		//CameraDrag
		if(leftDown){
			transform.position = transform.position - new Vector3(Input.mousePosition.x - lastmousePos.x, Input.mousePosition.y -lastmousePos.y,0)/30;
		}
		lastmousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
		//CameraZoom
        if (Input.GetAxis("Mouse ScrollWheel") < 0 && !gui.mouseOnGui(Input.mousePosition) && camera.fieldOfView < 140) 
            camera.fieldOfView += scrollSpeed;
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && !gui.mouseOnGui(Input.mousePosition) && camera.fieldOfView > 10)
            camera.fieldOfView -= scrollSpeed;

        gameObject.GetComponentsInChildren<Camera>()[1].fieldOfView = camera.fieldOfView;
	}

	void Update () {
		if (Input.GetMouseButtonDown (1))
			leftDown = true;
		if (Input.GetMouseButtonUp (1))
			leftDown = false;
	}
}

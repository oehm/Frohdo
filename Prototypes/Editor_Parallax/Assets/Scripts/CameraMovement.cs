using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

    public float scrollSpeed = 2.0f;

	void FixedUpdate(){

        ////CameraZoom
        if (Input.GetAxis("Mouse ScrollWheel") < 0 && Input.mousePosition.x > 250) // back
            camera.orthographicSize += scrollSpeed;
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && Input.mousePosition.x > 250) // forward
            camera.orthographicSize -= scrollSpeed;

	}
}
using UnityEngine;
using System.Collections;

public class CameraMovementGame : MonoBehaviour {

    public GameObject character_ { get; set; }

    public float cameraMaxSpeed_;

    private GameObject cam;

	// Use this for initialization
	void Start ()
    {
        cam = ForceAspectRatio.CameraMain.gameObject;
	
	}

    // FixedUpdate is called once per physic frame
    void FixedUpdate()
    {
        Vector3 desiredPos = new Vector3(character_.transform.position.x, character_.transform.position.y, cam.transform.position.z);

        Vector3 desiredMove = desiredPos - cam.transform.position;

        //if (desiredMove.magnitude > cameraMaxSpeed_)
        //{
        //    desiredMove.Normalize();
        //    desiredMove *= cameraMaxSpeed_;
        //}

        desiredMove *= cameraMaxSpeed_;

        Vector3 newPos = cam.transform.position + desiredMove;

        cam.transform.position = newPos;
	}
}

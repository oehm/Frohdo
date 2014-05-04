using UnityEngine;
using System.Collections;

public class CameraMovementGame : MonoBehaviour {

    public GameObject player_;

    public float cameraMaxSpeed_;

	// Use this for initialization
	void Start () {
	
	}

    // FixedUpdate is called once per physic frame
    void FixedUpdate()
    {
        Vector3 desiredPos = new Vector3(player_.transform.position.x, player_.transform.position.y, transform.position.z);

        Vector3 desiredMove = desiredPos - transform.position;

        //if (desiredMove.magnitude > cameraMaxSpeed_)
        //{
        //    desiredMove.Normalize();
        //    desiredMove *= cameraMaxSpeed_;
        //}

        desiredMove *= cameraMaxSpeed_;

        Vector3 newPos = transform.position + desiredMove;

        transform.position = newPos;
	}
}

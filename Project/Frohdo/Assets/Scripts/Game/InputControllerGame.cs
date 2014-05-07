using UnityEngine;
using System.Collections;

public class InputControllerGame : MonoBehaviour {

    public CharacterMovement character_;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        float inputX = Input.GetAxis("Horizontal");
        inputX = inputX == 0.0f ? 0 : Mathf.Sign(inputX);

        float inputY = Input.GetAxis("Vertical");
        inputY = inputY == 0.0f ? 0 : Mathf.Sign(inputY);

        Vector2 input = new Vector2(inputX, inputY);
        character_.InputMovement(input, Input.GetButton("Jump"), Input.GetButton("Puke"));
	}
}

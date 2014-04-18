using UnityEngine;
using System.Collections;

public class InputControllerGame : MonoBehaviour {

    public CharacterMovement character_;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        character_.InputMovement(input, Input.GetButton("Jump"));
	}
}

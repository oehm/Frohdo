using UnityEngine;
using System.Collections;

public class CharacterMovement : MonoBehaviour {

    public GameObject Character_;

    //private
    public float maxSpeedX_;
    public float accelX_;
    public float accelJump_;

    private Vector2 inputAxis_;
    private bool shouldJump_;

    private bool canJump_;


    public void InputMovement(Vector2 axis, bool jump)
    {
        inputAxis_ = axis;
        shouldJump_ = jump;
    }

	// Use this for initialization
	void Start () {
        inputAxis_ = new Vector2(0.0f, 0.0f);
        shouldJump_ = false;
        canJump_ = true;
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    // FixedUpdate is called once per physic frame
    void FixedUpdate()
    {

        Vector2 movementForce2 = new Vector2(inputAxis_.x * accelX_, 0.0f);

        if (shouldJump_ && canJump_)
        {
            movementForce2.y = accelJump_;
        }

        Character_.rigidbody2D.AddForce(movementForce2);

        Vector2 velocity2 = Character_.rigidbody2D.velocity;

        if (Mathf.Abs(velocity2.x) > maxSpeedX_)
        {
            velocity2.x = Mathf.Sign(velocity2.x) * maxSpeedX_;
        }

        Character_.rigidbody2D.velocity = velocity2;
    }
}

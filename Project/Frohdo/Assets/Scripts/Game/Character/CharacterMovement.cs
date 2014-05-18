﻿using UnityEngine;
using System.Collections;

public class CharacterMovement : MonoBehaviour {


    //public
    public float runMaxSpeed_;
    public float runForce_;
    public float jumpMaxSpeed_;
    public float jumpForce_;

    //private
    private Character character_;

    private Vector2 runInput_;
    private bool jumpInput_;


	// Use this for initialization
	void Start () {
        character_ = gameObject.GetComponent<Character>();

        runInput_ = new Vector2(0.0f, 0.0f);
        jumpInput_ = false;

	}


    public void InputMovement(Vector2 run, bool jump)
    {
        runInput_ = run;
        jumpInput_ = jump;
    }

    // FixedUpdate is called once per physic frame
    void FixedUpdate()
    {
        //add run and jump force
        Vector2 movementForce2 = new Vector2(runInput_.x * runForce_, 0.0f);
        if (jumpInput_ && canJump())
        {
            //jump
            movementForce2.y = jumpForce_;
        }
        character_.gameObject.rigidbody2D.AddForce(movementForce2);

        //limit run velocity
        Vector2 velocity2 = character_.gameObject.rigidbody2D.velocity;

        velocity2.x = Mathf.Clamp(velocity2.x, -runMaxSpeed_, runMaxSpeed_);
        velocity2.y = Mathf.Clamp(velocity2.y, - jumpMaxSpeed_, jumpMaxSpeed_);

        character_.gameObject.rigidbody2D.velocity = velocity2;
    }

    bool canJump()
    {
        int layerMask = 1 << LayerMask.NameToLayer("Solids");

        Vector2 startPoint = character_.gameObject.transform.position;
        startPoint += new Vector2(-0.5f, -2.1f);
        Vector2 endPoint = character_.gameObject.transform.position;
        endPoint +=new Vector2(0.5f, -2.1f);

        RaycastHit2D hit = Physics2D.Linecast(startPoint, endPoint, layerMask);

        return hit.collider != null;
    }
}
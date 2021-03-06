﻿using UnityEngine;
using System.Collections;

public class CharacterMovement : MonoBehaviour {


    //public
    public float runMaxSpeed_;
    public float runForce_;
    public float jumpMaxSpeed_;
    public float jumpForce_;

    public Vector2 floorRayStart_;
    public Vector2 floorRayMid_;
    public Vector2 floorRayEnd_;

    //private
    private Character character_;
    private Animator animator_;

    private Vector2 runInput_;
    private bool jumpInput_;


	// Use this for initialization
	void Start () {
        character_ = gameObject.GetComponent<Character>();
        animator_ = character_.gameObject.GetComponentInChildren<Animator>();

        runInput_ = new Vector2(0.0f, 0.0f);
        jumpInput_ = false;

	}


    public void InputMovement(Vector2 run, bool jump)
    {
        runInput_ = run;
        jumpInput_ = jump;
    }

    void Update()
    {
        animator_.SetBool("lookLeft", character_.lookLeft);
        animator_.SetBool("jumps", !canJump());

        //if (!canJump()) { 
        //    Debug.Log("In the air!"); 
        //    //Debug.Break();
        //}

        float velocity = Mathf.Abs( character_.gameObject.rigidbody2D.velocity.x / runMaxSpeed_);
        animator_.SetFloat("velocity", velocity);
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
        startPoint += floorRayStart_;

        Vector2 midPoint = character_.gameObject.transform.position;
        midPoint += floorRayMid_;

        Vector2 endPoint = character_.gameObject.transform.position;
        endPoint += floorRayEnd_;

        RaycastHit2D hit1 = Physics2D.Linecast(startPoint, new Vector2(midPoint.x + 0.2f, midPoint.y), layerMask);
        RaycastHit2D hit2 = Physics2D.Linecast(endPoint, new Vector2(midPoint.x - 0.2f, midPoint.y), layerMask);

        Debug.DrawLine(startPoint, new Vector2(midPoint.x + 0.3f, midPoint.y), Color.green);
        Debug.DrawLine(endPoint, new Vector2(midPoint.x - 0.3f, midPoint.y), Color.green);

        return hit1.collider != null || hit2.collider != null;
    }
}

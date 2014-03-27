using UnityEngine;
using System.Collections;

public class CharacterController2D : MonoBehaviour {


    private float hor = 0;
    private float jump = 0;

    public float horMoveForce = 10;
    public float jumpForce = 3;
    public float minDistToGround = 0.001f;

    public float slowParama = 0.5f;

    private bool grounded = false;
    public float minJumpTimeOff = 0.1f;
    private float deltaJumpTime = 1;

    // Use this for initialization
	void Start ()  {
        //GetComponent<Rigidbody2D>().
    }
	
	// Update is called once per frame
	void Update () {
        //groundcheck
        deltaJumpTime += Time.deltaTime;
        grounded = false;
        RaycastHit2D hit1 = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y)+ new Vector2(0.5f,-0.501f), new Vector2(0, -1), minDistToGround);
        RaycastHit2D hit2 = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y) + new Vector2(-0.5f, -0.501f), new Vector2(0, -1), minDistToGround);

        if (hit1.collider != null || hit2.collider != null)
        {
            grounded = true;
        }
        //movement
        hor = Input.GetAxis("Horizontal");
        jump = Input.GetAxis("Jump");

        float dt = Time.deltaTime;

        if (hor > 0) hor = 1;
        else if (hor < 0) hor = -1;

        Vector2 force = new Vector2(0, 0);

        force.x = hor * dt * horMoveForce;
        if (grounded && deltaJumpTime > minJumpTimeOff)
        {
            force.y = jumpForce * jump;
            if (jump > 0)
            {
                deltaJumpTime = 0;
            }
        }

        GetComponent<Rigidbody2D>().AddForce(force);

	}

    void FixedUpdate()
    {
        if (hor == 0)
        {
            Vector2 curVel = GetComponent<Rigidbody2D>().velocity;
            curVel.x = curVel.x * slowParama;
            GetComponent<Rigidbody2D>().velocity = curVel;
        }
    }
}

using UnityEngine;
using System.Collections;

public class CharacterMovement : MonoBehaviour {


    //public
    public float runMaxSpeed_;
    public float runForce_;
    public float jumpMaxSpeed_;
    public float jumpForce_;
    public float pukeForceSide_;
    public float pukeForceUp_;
    public float pukeForceDown_;

    public GameObject pukePrefab_;

    public Vector2 pukeOffSide_;
    public Vector2 pukeOffUp_;
    public Vector2 pukeOffDown_;


    //private
    private GameObject character_;

    private bool lookLeft_;
    private bool lookUp_;
    private bool lookDown_;

    private Vector2 runInput_;
    private bool jumpInput_;
    private bool pukeInput_;

    private bool jumpDisabled_;
    private bool pukeDisabled_;


	// Use this for initialization
	void Start () {
        character_ = transform.parent.gameObject;

        lookLeft_ = false;

        runInput_ = new Vector2(0.0f, 0.0f);
        jumpInput_ = false;
        pukeInput_ = false;

        jumpDisabled_ = false;
        pukeDisabled_ = false;
        
	}


    public void InputMovement(Vector2 run, bool jump, bool puke)
    {
        runInput_ = run;
        jumpInput_ = jump;
        pukeInput_ = puke;

        if (run.x < 0)
        {
            lookLeft_ = true;
        }
        if (run.x > 0)
        {
            lookLeft_ = false;
        }
        lookUp_ = run.y > 0;
        lookDown_ = run.y < 0;
    }

    // FixedUpdate is called once per physic frame
    void FixedUpdate()
    {
        //puke
        if (pukeInput_ && !pukeDisabled_)
        {
            GameObject pukeObject = (GameObject)Instantiate(pukePrefab_);

            pukeObject.GetComponent<PukeBall>().color_ = "M";
            pukeObject.GetComponentInChildren<Renderer>().material.color = LevelObjectController.Instance.GetColor("M");

            Vector2 pukePos = character_.transform.position;

            if (lookUp_) {
                pukePos += pukeOffUp_;
                pukeObject.rigidbody2D.AddForce(new Vector2(0.0f, pukeForceUp_));
            }
            else if (lookDown_)
            {
                pukePos += pukeOffDown_;
                pukeObject.rigidbody2D.AddForce(new Vector2(0.0f, -pukeForceDown_));
            }
            else
            {
                if (lookLeft_)
                {
                    pukePos.x -= pukeOffSide_.x;
                    pukePos.y += pukeOffSide_.y;
                    pukeObject.rigidbody2D.AddForce(new Vector2(-pukeForceSide_, 0.0f));
                }
                else
                {
                    pukePos += pukeOffSide_;
                    pukeObject.rigidbody2D.AddForce(new Vector2(pukeForceSide_, 0.0f));
                }
            }


            pukeObject.transform.position = pukePos;
            pukeObject.transform.parent = character_.transform.parent;

            pukeObject.GetComponent<Rigidbody2D>().velocity = character_.GetComponent<Rigidbody2D>().velocity;


            pukeDisabled_ = true;
        }
        if (!pukeInput_)
        {
            pukeDisabled_ = false;
        }


        //add run and jump force
        Vector2 movementForce2 = new Vector2(runInput_.x * runForce_, 0.0f);
        if (jumpInput_ && canJump() && !jumpDisabled_)
        {
            //jump
            movementForce2.y = jumpForce_;

            jumpDisabled_ = true;
        }
        if (!jumpInput_)
        {
            jumpDisabled_ = false;
        }
        character_.rigidbody2D.AddForce(movementForce2);

        //limit run velocity
        Vector2 velocity2 = character_.rigidbody2D.velocity;

        velocity2.x = Mathf.Clamp(velocity2.x, -runMaxSpeed_, runMaxSpeed_);
        velocity2.y = Mathf.Clamp(velocity2.y, - jumpMaxSpeed_, jumpMaxSpeed_);

        character_.rigidbody2D.velocity = velocity2;
    }

    bool canJump()
    {
        int layerMask = 1 << LayerMask.NameToLayer("Environment");

        Vector2 startPoint = character_.transform.position;
        startPoint += new Vector2(-0.5f, -2.1f);
        Vector2 endPoint = character_.transform.position;
        endPoint +=new Vector2(0.5f, -2.1f);

        RaycastHit2D hit = Physics2D.Linecast(startPoint, endPoint, layerMask);

        return hit.collider != null;
    }
}

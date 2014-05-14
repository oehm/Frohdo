using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour {

    public new GameObject gameObject { get { return gameObject_; } } //hides inherited member of GameObject
    public CharacterMovement movement { get { return movement_; } }
    public CharacterPuke puke { get { return puke_; } }

    public bool lookLeft { get { return lookLeft_; } }
    public bool lookUp { get { return lookUp_; } }
    public bool lookDown { get { return lookDown_; } }

    //private
    private GameObject gameObject_;
    private CharacterMovement movement_;
    private CharacterPuke puke_;
    private CharacterPickup pickup_;

    private bool lookLeft_;
    private bool lookUp_;
    private bool lookDown_;


    void Awake()
    {
        gameObject_ = transform.parent.gameObject;
        movement_ = gameObject_.GetComponentInChildren<CharacterMovement>();
        puke_ = gameObject_.GetComponentInChildren<CharacterPuke>();
        pickup_ = gameObject_.GetComponentInChildren<CharacterPickup>();

        lookLeft_ = false;
        lookUp_ = false;
        lookDown_ = false;
	}


    public void InputMovement(Vector2 run, bool jump, bool puke)
    {
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

        pickup_.Enabled = lookDown_;

        movement_.InputMovement(run, jump);
        puke_.InputMovement(puke);
    }

    public void pickUp(GameObject pickUp)
    {
        //Debug.Log("picked up: " + pickUp.name);
        if (pickUp.name.Equals("ColorRatio"))
        {
            string color = pickUp.GetComponent<Colorable>().colorString;
            puke_.AddRatio(color);
            Destroy(pickUp);
        }
    }

    // FixedUpdate is called once per physic frame
    void FixedUpdate()
    {
	}
}

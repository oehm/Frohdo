using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour {

    public new GameObject gameObject { get { return gameObject_; } } //hides inherited member of GameObject

    public bool lookLeft { get { return lookLeft_; } }
    public bool lookUp { get { return lookUp_; } }
    public bool lookDown { get { return lookDown_; } }

    //private
    private GameObject gameObject_;
    public CharacterMovement Movement{ get; private set;}
    public CharacterPuke Puke { get; private set; }
    public CharacterPickup Pickup { get; private set; }
    public CharacterTooltip Tooltip { get; private set; }
    public Colorable Colorable { get; private set; }

    private bool lookLeft_;
    private bool lookUp_;
    private bool lookDown_;

    private bool isKilling_;

    void Awake()
    {
        gameObject_ = transform.parent.gameObject;
        Movement = gameObject_.GetComponentInChildren<CharacterMovement>();
        Puke = gameObject_.GetComponentInChildren<CharacterPuke>();
        Pickup = gameObject_.GetComponentInChildren<CharacterPickup>();
        Tooltip = gameObject_.GetComponentInChildren<CharacterTooltip>();
        Colorable = gameObject_.GetComponentInChildren<Colorable>();

        lookLeft_ = false;
        lookUp_ = false;
        lookDown_ = false;

        isKilling_ = false;
	}


    public void InputMovement(Vector2 run, bool jump, bool puke, bool kill)
    {
        if (!isKilling_)
        {
            if (kill)
            {
                isKilling_ = kill;
                //start killing yourself
                Movement.InputMovement(Vector2.zero, false);
                SceneManager.Instance.loadScene(SceneManager.Scene.Game);
            }
            else
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

                Pickup.EnabledDown = lookDown_;
                Pickup.EnabledUp = lookUp_;

                Movement.InputMovement(run, jump);
                Puke.InputMovement(puke);

            }

        }
    }

    public void pickUp(GameObject pickUpThing)
    {
        //Debug.Log("picked up: " + pickUp.name);

        if (pickUpThing.GetComponent<Collectable>().behaviour_ == Collectable.Behaviour.ColorRatio)
        {
            string color = pickUpThing.GetComponent<Colorable>().colorString;
            Puke.AddRatio(color);
            Destroy(pickUpThing);
        }
    }

    public void use(GameObject usedThing)
    {
        //Debug.Log("used: " + thing.name);

        if (usedThing.GetComponent<Usable>().behaviour_ == Usable.Behaviour.Door)
        {
            ScoreController.Instance.isRunning = false;
            SceneManager.Instance.loadScene(SceneManager.Scene.RateScreen);
        }
        if (usedThing.GetComponent<Usable>().behaviour_ == Usable.Behaviour.ColorRatio)
        { 
            Tooltip.setTooltip(2, usedThing.GetComponent<Colorable>().colorString);
        }
    }
}

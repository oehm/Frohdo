﻿using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour {

    public new GameObject gameObject { get { return gameObject_; } } //hides inherited member of GameObject

    public bool lookLeft { get { return lookLeft_; } }
    public bool lookUp { get { return lookUp_; } }
    public bool lookDown { get { return lookDown_; } }

    //private
    private GameObject gameObject_;
    private CharacterMovement movement_;
    private CharacterPuke puke_;
    private CharacterPickup pickup_;
    private Tooltip tooltip_;

    private bool lookLeft_;
    private bool lookUp_;
    private bool lookDown_;

    private bool isKilling_;

    void Awake()
    {
        gameObject_ = transform.parent.gameObject;
        movement_ = gameObject_.GetComponentInChildren<CharacterMovement>();
        puke_ = gameObject_.GetComponentInChildren<CharacterPuke>();
        pickup_ = gameObject_.GetComponentInChildren<CharacterPickup>();
        tooltip_ = gameObject_.GetComponentInChildren<Tooltip>();

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
                movement_.InputMovement(Vector2.zero, false);
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

                pickup_.EnabledDown = lookDown_;
                pickup_.EnabledUp = lookUp_;

                movement_.InputMovement(run, jump);
                puke_.InputMovement(puke);

            }

        }
    }

    public void pickUp(GameObject pickUpThing)
    {
        //Debug.Log("picked up: " + pickUp.name);

        if (pickUpThing.GetComponent<Collectable>().behaviour_ == Collectable.Behaviour.ColorRatio)
        {
            string color = pickUpThing.GetComponent<Colorable>().colorString;
            puke_.AddRatio(color);
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
            tooltip_.setTooltip(2, usedThing.GetComponent<Colorable>().colorString);
        }
    }
}

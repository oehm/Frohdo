﻿using UnityEngine;
using System.Collections;

public class CharacterPuke : MonoBehaviour
{

    //public
    public float pukeForceSide_;
    public float pukeForceUp_;
    public float pukeForceDown_;

    public GameObject pukePrefab_;

    public Vector2 pukeOffSide_;
    public Vector2 pukeOffUp_;
    public Vector2 pukeOffDown_;


    //private
    private Character character_;

    private bool pukeInput_;

	// Use this for initialization
    void Start()
    {
        character_ = gameObject.GetComponent<Character>();
	}


    public void InputMovement(bool puke)
    {
        pukeInput_ = puke;
    }

    // FixedUpdate is called once per physic frame
    void FixedUpdate()
    {
        if (pukeInput_)
        {
            GameObject pukeObject = (GameObject)Instantiate(pukePrefab_);

            pukeObject.GetComponent<PukeBall>().color_ = "M";
            pukeObject.GetComponentInChildren<Renderer>().material.color = LevelObjectController.Instance.GetColor("M");

            Vector2 pukePos = character_.transform.position;

            if (character_.lookUp)
            {
                pukePos += pukeOffUp_;
                pukeObject.rigidbody2D.AddForce(new Vector2(0.0f, pukeForceUp_));
            }
            else if (character_.lookDown)
            {
                pukePos += pukeOffDown_;
                pukeObject.rigidbody2D.AddForce(new Vector2(0.0f, -pukeForceDown_));
            }
            else
            {
                if (character_.lookLeft)
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
            pukeObject.transform.parent = character_.gameObject.transform.parent;

            pukeObject.GetComponent<Rigidbody2D>().velocity = character_.gameObject.GetComponent<Rigidbody2D>().velocity;
        }
    }
}

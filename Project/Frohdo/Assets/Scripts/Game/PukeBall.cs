﻿using UnityEngine;
using System.Collections;

public class PukeBall : MonoBehaviour {

    public string color_;

    void OnCollisionEnter2D(Collision2D coll)
    {
        if(coll.gameObject.tag.Equals("LevelObject"))
        {
            coll.gameObject.GetComponentInChildren<Renderer>().material.color = LevelObjectController.Instance.GetColor(color_);

            Destroy(gameObject);
        }
    }
}
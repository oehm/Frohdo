﻿using UnityEngine;
using System.Collections;

public class InputController : MonoBehaviour {

    public EditorObjectPlacement obj_placement;
    
    private bool leftMouseDown = false;
    private Vector2 mousePos = new Vector2(0,0);
    
    // Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        bool oldLeftDown = leftMouseDown;
        leftMouseDown = Input.GetMouseButton(0);
        if (oldLeftDown != leftMouseDown) leftButtonChaged(oldLeftDown, leftMouseDown);

        Vector2 oldMousePos = mousePos;
        mousePos = Input.mousePosition;
        if (mousePos != oldMousePos) mouseMove();


	}

    private void leftButtonChaged(bool oldButton, bool newButton)
    {
        //Left mouse buttonwas pressed
        if(oldButton == false)
        {
            obj_placement.mouseDown();
        }
        //left mouse button was released
        else
        {
            obj_placement.mouseUp();
        }
    }

    private void mouseMove()
    {
        obj_placement.mouseMove(mousePos);
    }

}
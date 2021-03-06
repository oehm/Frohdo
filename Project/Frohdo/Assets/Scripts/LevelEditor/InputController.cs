﻿using UnityEngine;
using System.Collections;

public class InputController : MonoBehaviour {

    public StateManager stateManager;
    
    private bool leftMouseDown = false;
    private Vector3 mousePos = new Vector3(0,0,0);
    
	
	// Update is called once per frame
	void Update () 
    {
        
        bool oldLeftDown = leftMouseDown;
        leftMouseDown = Input.GetMouseButton(0);
        if (oldLeftDown != leftMouseDown)
        {
            leftButtonChaged(oldLeftDown, leftMouseDown);
        }

        Vector3 oldMousePos = mousePos;
        mousePos = Input.mousePosition;
        if (mousePos != oldMousePos) mouseMove();

	}

    private void leftButtonChaged(bool oldButton, bool newButton)
    {
        //Left mouse buttonwas pressed
        if(oldButton == false)
        {
            stateManager.leftMouseDown();
        }
        //left mouse button was released
        else
        {
            stateManager.leftMouseUp();
        }
    }

    private void mouseMove()
    {
        stateManager.mouseMove(mousePos);
    }

}

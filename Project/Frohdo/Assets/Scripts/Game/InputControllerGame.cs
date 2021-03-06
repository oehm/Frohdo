﻿using UnityEngine;
using System.Collections;

public class InputControllerGame : MonoBehaviour
{

    public Character character_;

    private bool jumpDisable_;
    private bool pukeDisable_;

    // Use this for initialization
    void Start()
    {
        jumpDisable_ = false;
        pukeDisable_ = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float inputX = Input.GetAxis("Horizontal");
        inputX = inputX == 0.0f ? 0 : Mathf.Sign(inputX);
        float inputY = Input.GetAxis("Vertical");
        inputY = inputY == 0.0f ? 0 : Mathf.Sign(inputY);

        Vector2 input = new Vector2(inputX, inputY);

        bool jump = Input.GetButton("Jump") && !jumpDisable_;
        jumpDisable_ = Input.GetButton("Jump");

        bool puke = Input.GetButton("Puke") && !pukeDisable_;
        pukeDisable_ = Input.GetButton("Puke");

        bool kill = Input.GetButton("Kill");

        character_.InputMovement(input, jump, puke, kill);

        if (!ScoreController.Instance.isRunning && (input.x != 0.0f || input.y != 0.0f || jump || puke))
        {
            ScoreController.Instance.isRunning = true;
        }


        if (Input.GetButton("ScreenShot"))
        {
             ScreenShotManager.Instance.takeScreenShot(false ,ScreenShotManager.Instance.getScreenShotFolderPath());
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : CharacterController
{
    //Touch Control
    private Vector3 firstTouchPosition;
    private Vector3 lastTouchPosition;
    public bool isTouched;

    public bool GamePaused;

    
    private void Start()
    {
        // playerSettings.moveSpeed = 12.5f;
    }

    void Update()
    {
        InputType();

        if (!isTouched || direction == Vector3.zero || GamePaused == true)
        {
            return;
        }
        
        RotateToDirection();

    }

    private void FixedUpdate()
    {
       
        base.Movement();
        
    }
    

    private void InputType()
    {
#if UNITY_EDITOR

        if (!Input.GetMouseButton(0) && !Input.GetMouseButtonUp(0))
        {
            isTouched = false;
        }
        else
        {
            isTouched = true;

            if (Input.GetMouseButtonDown(0))
            {
                firstTouchPosition = Input.mousePosition;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                direction = Vector3.zero;
                lastTouchPosition = Input.mousePosition;
            }
            else
            {
                Vector3 screenDirection = Input.mousePosition - firstTouchPosition;
                direction = new Vector3(screenDirection.x, 0, screenDirection.y);
                direction.Normalize();
            }
        }

#else
        if (Input.touchCount > 0)
        {
            isTouched = true;
            var inputTouch = Input.GetTouch(0);

            if (inputTouch.phase == TouchPhase.Began)
            {
                firstTouchPosition = inputTouch.position;
            }
            else if (inputTouch.phase == TouchPhase.Moved || inputTouch.phase == TouchPhase.Stationary)
            {
                Vector3 screenDirection = (Vector3)inputTouch.position - firstTouchPosition;
                direction = new Vector3(screenDirection.x, 0, screenDirection.y);
                direction.Normalize();
            }
            else
            {
                direction = Vector3.zero;
                lastTouchPosition = inputTouch.position;
            }
        }
        else
        {
            isTouched = false;
        }

#endif
    }
}
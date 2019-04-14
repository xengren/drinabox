using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSwipeDirection : MonoBehaviour
{
    private bool _getMouseDown;
    public Transform origin;

    private Vector2 fingerUpCamera;
    private Vector2 fingerDownCamera;

    public bool detectSwipeOnlyAfterRelease = false;
    public float SWIPE_THRESHOLD = 0.1f;

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (!_getMouseDown)
            {
                var pos1 = Input.mousePosition;
                pos1.z = 10;
                pos1 = Camera.main.ScreenToWorldPoint(pos1);
                fingerUpCamera = pos1;
                fingerDownCamera = pos1;
                _getMouseDown = true;
            }

            var pos = Input.mousePosition;
            pos.z = 10;
            pos = Camera.main.ScreenToWorldPoint(pos);

            transform.position = pos;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            var pos2 = Input.mousePosition;
            pos2.z = 10;
            pos2 = Camera.main.ScreenToWorldPoint(pos2);        
            fingerDownCamera = pos2;
            checkSwipe();
        }


        if (Vector3.Distance(origin.transform.position, this.transform.position) > 0.05f)
        {
            if (!detectSwipeOnlyAfterRelease)
            {
                var pos3 = Input.mousePosition;
                pos3.z = 10;
                pos3 = Camera.main.ScreenToWorldPoint(pos3);
                fingerDownCamera = pos3;
                checkSwipe();
            }
        }
    }


    void checkSwipe()
    {
        //Check if Vertical swipe
        if (verticalMove() > SWIPE_THRESHOLD && verticalMove() > horizontalValMove())
        {
            //Debug.Log("Vertical");
            if (fingerDownCamera.y - fingerUpCamera.y > 0)//up swipe
            {
                OnSwipeUp();
            }
            else if (fingerDownCamera.y - fingerUpCamera.y < 0)//Down swipe
            {
                OnSwipeDown();
            }
            fingerUpCamera = fingerDownCamera;
        }

        //Check if Horizontal swipe
        else if (horizontalValMove() > SWIPE_THRESHOLD && horizontalValMove() > verticalMove())
        {
            //Debug.Log("Horizontal");
            if (fingerDownCamera.x - fingerUpCamera.x > 0)//Right swipe
            {
                OnSwipeRight();
            }
            else if (fingerDownCamera.x - fingerUpCamera.x < 0)//Left swipe
            {
                OnSwipeLeft();
            }
            fingerUpCamera = fingerDownCamera;
        }

        //No Movement at-all
        else
        {
            //Debug.Log("No Swipe!");
        }
    }


    float verticalMove()
    {
        return Mathf.Abs(fingerDownCamera.y - fingerUpCamera.y);
    }

    float horizontalValMove()
    {
        return Mathf.Abs(fingerDownCamera.x - fingerUpCamera.x);
    }

    //////////////////////////////////CALLBACK FUNCTIONS/////////////////////////////
    void OnSwipeUp()
    {
        Debug.Log("Swipe UP");
    }

    void OnSwipeDown()
    {
        Debug.Log("Swipe Down");
    }

    void OnSwipeLeft()
    {
        Debug.Log("Swipe Left");
    }

    void OnSwipeRight()
    {
        Debug.Log("Swipe Right");
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCubeVelocity : MonoBehaviour
{
    private bool _getMouseDown;
    public Transform origin;

    private Vector2 fingerUpCamera;
    private Vector2 fingerDownCamera;

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
            _getMouseDown = false;
        }


        if (Vector3.Distance(origin.transform.position, this.transform.position) > 0.6f)
        {
            float distX = this.transform.position.x - origin.transform.position.x;
            float distY = this.transform.position.y - origin.transform.position.y;            

            if (Mathf.Abs(distX) > Mathf.Abs(distY))
            {
                if (this.transform.position.x < origin.position.x)
                {
                    print("move left");
                }

                if (this.transform.position.x > origin.position.x)
                {
                    print("move right");
                }
            }
            else
            {
                if (this.transform.position.y > origin.position.y)
                {
                    print("move up");
                }

                if (this.transform.position.y < origin.position.y)
                {
                    print("move down");
                }
            }            
        }
    }

    private void OnMouseDown()
    {        
    }


    private void OnMouseUp()
    {
        
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.XR.MagicLeap;

public class HandManager : MonoBehaviour
{
    public GameObject cube;
    private Vector3 previous;
    public Animator uiCanvas;
    public Text debugText;
    public Text handDetectedText;

    float velocity;
    private bool _getPosOnce;
    private float _originalPosX;
    

    // Update is called once per frame
    void Update()
    {
        //if (GetGesture(MLHands.Left, MLHandKeyPose.OpenHandBack))
        //{
        //    handDetectedText.text = "hand detected";

        //    if (!_getPosOnce)
        //    {
        //        _originalPosX = MLHands.Left.Thumb.KeyPoints[0].Position.x;
        //        _getPosOnce = true;
        //    }

        //    cube.transform.position = MLHands.Left.Thumb.KeyPoints[0].Position;

        //    velocity = ((cube.transform.position - previous).magnitude) / Time.deltaTime;
        //    previous = cube.transform.position;

        //    debugText.text = velocity.ToString();

        //    if (velocity > 12)
        //    {
        //        if (cube.transform.position.x < _originalPosX)
        //        {
        //            print("swiped left");
        //            uiCanvas.Play("swiped_left");
        //        }
        //        else
        //        {
        //            print("swiped right");
        //            uiCanvas.Play("swiped_right");
        //        }
        //    }
        //}
        //else
        //{
        //    handDetectedText.text = "none";
        //    _getPosOnce = false;
        //}
        
    }


    //private bool GetGesture(MLHand hand, MLHandKeyPose type)
    //{
    //    if (hand != null)
    //    {
    //        if (hand.KeyPose == type)
    //        {
    //            if (hand.KeyPoseConfidence > 0.6f)
    //            {
    //                return true;
    //            }
    //        }
    //    }
    //    return false;
    //}

}

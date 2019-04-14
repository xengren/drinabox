using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.MagicLeap;
using UnityEngine.UI;

public class RemoteCameraManager : MonoBehaviour
{
    public Text directionText;
    public GameObject referenceCube;

    public GameObject container;        

    private MLHandKeyPose[] _gestures;    
    private Vector3[] pos;

    private Vector2 _originalPos;
    private bool _getFist;    

    private Vector2 fingerUpCamera;
    private Vector2 fingerDownCamera;    

    private bool detectSwipeOnlyAfterCameraRelease = false;    
    private float SWIPE_THRESHOLD = 0.082f;

    public string URI = "";
    private GestureSender gs;

    // Start is called before the first frame update
    void Start()
    {
        _gestures = new MLHandKeyPose[5];
        _gestures[0] = MLHandKeyPose.Ok;
        _gestures[1] = MLHandKeyPose.Finger;
        _gestures[2] = MLHandKeyPose.OpenHandBack;
        _gestures[3] = MLHandKeyPose.Fist;
        _gestures[4] = MLHandKeyPose.Thumb;
        MLHands.KeyPoseManager.EnableKeyPoses(_gestures, true, false);
        pos = new Vector3[3];

        gs = new GestureSender(URI);
    }

    // Update is called once per frame
    void Update()
    {        
        if (Config.VIDEO_SCREEN)
        {
            if (GetGesture(MLHands.Left, MLHandKeyPose.Finger))
            {                                
                Config.USING_FINGER = true;                

                if (!_getFist)
                {
                    _originalPos = referenceCube.transform.position;
                    fingerUpCamera = referenceCube.transform.position;
                    fingerDownCamera = referenceCube.transform.position;

                    _getFist = true;
                }

                referenceCube.transform.position = MLHands.Left.Wrist.KeyPoints[0].Position;
                //directionText.text = MLHands.Left.Wrist.KeyPoints[0].Position.ToString();
            }
            else
            {
                fingerDownCamera = referenceCube.transform.position;
                checkSwipe();
                _getFist = false;
                directionText.text = "none";
            }

            if (Vector3.Distance(_originalPos, referenceCube.transform.position) > 0.085f)
            {
                if (!detectSwipeOnlyAfterCameraRelease)
                {
                    fingerDownCamera = referenceCube.transform.position;
                    checkSwipe();
                }
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
        directionText.text = "UP";
        StartCoroutine(gs.GetRequest(GestureDirection.Up, true));
        Debug.Log($"Vital Signs Reported: {gs.VitalSigns}");
    }

    void OnSwipeDown()
    {
        directionText.text = "DOWN";
        StartCoroutine(gs.GetRequest(GestureDirection.Down, true));
        Debug.Log($"Vital Signs Reported: {gs.VitalSigns}");
    }

    void OnSwipeLeft()
    {
        directionText.text = "LEFT";
        StartCoroutine(gs.GetRequest(GestureDirection.Left, true));
        Debug.Log($"Vital Signs Reported: {gs.VitalSigns}");
    }

    void OnSwipeRight()
    {
        directionText.text = "RIGHT";
        StartCoroutine(gs.GetRequest(GestureDirection.Right, true));
        Debug.Log($"Vital Signs Reported: {gs.VitalSigns}");
    }    


    private bool GetGesture(MLHand hand, MLHandKeyPose type)
    {
        if (hand != null)
        {
            if (hand.KeyPose == type)
            {
                if (hand.KeyPoseConfidence > 0.6f)
                {
                    return true;
                }
            }
        }
        return false;
    }

}

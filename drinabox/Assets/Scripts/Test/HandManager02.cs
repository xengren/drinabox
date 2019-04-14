using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.MagicLeap;
using UnityEngine.UI;

public class HandManager02 : MonoBehaviour
{
    private MLHandKeyPose[] _gestures;
    public Text handDetectionText;
    public Vector3[] pos;

    // Start is called before the first frame update    
    private void Awake()
    {
        MLHands.Start();
        _gestures = new MLHandKeyPose[5];
        _gestures[0] = MLHandKeyPose.Ok;
        _gestures[1] = MLHandKeyPose.Finger;
        _gestures[2] = MLHandKeyPose.OpenHandBack;
        _gestures[3] = MLHandKeyPose.Fist;
        _gestures[4] = MLHandKeyPose.Thumb;
        MLHands.KeyPoseManager.EnableKeyPoses(_gestures, true, false);
        pos = new Vector3[3];
    }

    // Update is called once per frame
    void Update()
    {        
        if (GetGesture(MLHands.Left, MLHandKeyPose.OpenHandBack))
        {
            handDetectionText.text = "hand detected";
        }
        else
        {
            handDetectionText.text = "none";
        }
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

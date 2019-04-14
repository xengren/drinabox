using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.MagicLeap;

public class BeamCollision : MonoBehaviour
{
    //public GameObject currentInfo;
    //public GameObject prescriptions;
    public GameObject overview;
    public GameObject profile;

    public GameObject profileBlur;
    public GameObject overviewBlur;

    public GameObject container;

    public GameObject leftArrow;
    public GameObject rightArrow;    

    private MLInputController _controller;

    private bool _isTriggering;
    
    private bool _overviewTriggered;
    private bool _profileTriggered;

    private bool _leftArrowTriggered;
    private bool _rightArrowTriggered;

    private bool _delay;

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Overview")
        {
            overviewBlur.SetActive(true);
        }
        else if (other.name == "Profile")
        {
            profileBlur.SetActive(true);
        }        

        ResetTriggers();
        if (other.name == "Overview")
        {
            _overviewTriggered = true;
        }
        else if (other.name == "Profile")
        {
            _profileTriggered = true;
        }
        else if (other.name == "leftArrowCollider")
        {
            _leftArrowTriggered = true;
        }
        else if (other.name == "rightArrowCollider")
        {
            _rightArrowTriggered = true;
        }

        _isTriggering = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Overview")
        {
            overviewBlur.SetActive(false);
        }
        else if (other.name == "Profile")
        {
            profileBlur.SetActive(false);
        }
        

        ResetTriggers();
        _isTriggering = false;
    }

    private void Start()
    {
        MLInput.Start();
        _controller = MLInput.GetController(MLInput.Hand.Left);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _leftArrowTriggered = true;
            EnableScreen();
        }

        if (_controller.TriggerValue > 0.6f && _isTriggering)
        {            
            ResetScreens();            
            EnableScreen();            
        }
    }


    void ResetTriggers()
    {        
        _overviewTriggered = false;
        _profileTriggered = false;
        _leftArrowTriggered = false;
        _rightArrowTriggered = false;
    }


    private void EnableScreen()
    {       
        if (_overviewTriggered)
        {
            overview.SetActive(true);
        }
        else if (_profileTriggered)
        {
            profile.SetActive(true);
        }
        else if (_leftArrowTriggered)
        {
            OnTapLeftArrow();
        }
        else if (_rightArrowTriggered)
        {
            OnTapRightArrow();
        }
    }


    private void ResetScreens()
    {        
        overview.SetActive(false);
        profile.SetActive(false);

    }


    private void OnTapLeftArrow()
    {
        if (!_delay)
        {
            container.GetComponent<Animator>().Play("swiped_right");
            rightArrow.SetActive(true);            
            Config.MAIN_SCREEN = false;
            Config.VIDEO_SCREEN = true;
            Config.USING_FINGER = true;
            leftArrow.SetActive(false);
            _delay = true;
            Invoke("DisableDelay", 1f);
        }        
    }


    private void OnTapRightArrow()
    {
        if (!_delay)
        {
            container.GetComponent<Animator>().Play("swiped_left");
            leftArrow.SetActive(true);            
            Config.VIDEO_SCREEN = false;
            Config.MAIN_SCREEN = true;
            Config.USING_FINGER = false;
            rightArrow.SetActive(false);            
            _delay = true;
            Invoke("EnableProfile", 0.5f);
            Invoke("DisableDelay", 1f);            
        }        
    }


    private void EnableProfile()
    {
        profile.SetActive(true);
    }

    private void DisableDelay()
    {
        _delay = false;
    }

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.XR.MagicLeap;

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
         

    private bool _delay;

    
    private void Start()
    {
        //MLInput.Start();
        //_controller = MLInput.GetController(MLInput.Hand.Left);
    }

    private void Update()
    {
        /*
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out hit);
        if (hit.collider.name == "leftArrowCollider")
        {
            OnTapLeftArrow();
        }
        */

        /*
        else if(hit.collider.name == "rightArrowCollider")
        {
            OnTapRightArrow();
        }
        else if(hit.collider.name == "overviewBlur")
        {
            overview.SetActive(true);
            profile.SetActive(false);
        }
        else if (hit.collider.name == "profileBlur")
        {
            profile.SetActive(true);
            overview.SetActive(false);
        }
        */
               
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
       
    private void DisableDelay()
    {
        _delay = false;
    }

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class onTouchDown : MonoBehaviour
{
    public GameObject overview;
    public GameObject profile;

    public GameObject profileBlur;
    public GameObject overviewBlur;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out hit);
        if (hit.collider.name == "profileBlur")
        {
            profileBlur.SetActive(true);
            overviewBlur.SetActive(false);

            profile.SetActive(true);
            overview.SetActive(false);
        }
    }
}

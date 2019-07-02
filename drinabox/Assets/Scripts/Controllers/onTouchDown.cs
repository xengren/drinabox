using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class onTouchDown : MonoBehaviour
{
    public GameObject overviewScreen;
    public GameObject profileScreen;

    public GameObject profileBlur;
    public GameObject overviewBlur;
    public GameObject Prescriptions;
    public GameObject prescriptionScreen;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Input.GetMouseButtonDown(0))
        {                       
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out hit);

            if (prescriptionScreen.activeSelf && (hit.collider == null || hit.collider.name != "prescriptionScreen"))
            {
                prescriptionScreen.SetActive(false);
            }            
            else if (hit.collider != null && hit.collider.name == "Prescriptions")
            {
                prescriptionScreen.SetActive(true);                
            }
            else if (hit.collider != null && hit.collider.name == "Profile")
            {
                profileBlur.SetActive(true);
                overviewBlur.SetActive(false);

                profileScreen.SetActive(true);
                overviewScreen.SetActive(false);
            }
            else if (hit.collider != null && hit.collider.name == "Overview")
            {
                profileBlur.SetActive(false);
                overviewBlur.SetActive(true);

                profileScreen.SetActive(false);
                overviewScreen.SetActive(true);
            }
        }
        



    }
}

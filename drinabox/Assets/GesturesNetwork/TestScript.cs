using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    public string URI = "";
    private GestureSender gs;

    private void Start()
    {
        gs = new GestureSender(URI);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            StartCoroutine(gs.GetRequest(GestureDirection.Up, true));
            Debug.Log($"Vital Signs Reported: {gs.VitalSigns}");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            StartCoroutine(gs.GetRequest(GestureDirection.Right, true));
            Debug.Log($"Vital Signs Reported: {gs.VitalSigns}");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            StartCoroutine(gs.GetRequest(GestureDirection.Down, true));
            Debug.Log($"Vital Signs Reported: {gs.VitalSigns}");
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            StartCoroutine(gs.GetRequest(GestureDirection.Left, true));
            Debug.Log($"Vital Signs Reported: {gs.VitalSigns}");
        }

        
    }
}

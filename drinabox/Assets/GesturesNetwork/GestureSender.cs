using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public enum GestureDirection { NONE = 0, Up = 1, Right = 2, Down = 3, Left = 4 }

public class GestureSender
{
    //the url should be clean!!!
    // ex: http://google.com should be passed in as => "google.com" without the quotes.
    public string URL;

    public GestureSender(string URL)
    {
        this.URL = URL;
    }

    //After creating a get request, the get request will also populate the
    //vital signs so check its contents.
    public string VitalSigns
    {
        get => vitalSigns;
    }

    //1  => http://192.168.128.100/vitals.cs?gestures=[1|2|3|4]
    //2  => http://192.168.128.100/vitals.cs
    //#1 will return vitals AND WILL move the camera.
    //#2 will return vitals BUT WILL NOT move the camera.

    public IEnumerator GetRequest(GestureDirection gesture = 0, bool debugging = false)
    {
        if (blocked)
        {
            yield return null;
        }
        else
        {
            blocked = true;
            StringBuilder uri = new StringBuilder($"http://{URL}/vitals.cs");
            if ((int)gesture > 0)
            {
                uri.Append($"?gestures={(int)gesture}");
            }

            if (debugging) Debug.Log(uri.ToString());

            UnityWebRequest uwr = UnityWebRequest.Get(uri.ToString());
            yield return uwr.SendWebRequest();

            if (uwr.isNetworkError)
            {
                Debug.LogError("Error While Sending: " + uwr.error);
            }
            else
            {
                vitalSigns = uwr.downloadHandler.text;
            }

            yield return new WaitForSeconds(1.0f);
            blocked = false;
        }

    }

    private string vitalSigns = "";
    private bool blocked = false;
}

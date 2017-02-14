using UnityEngine;

public class ControllingCameraAspectScript : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        // set the desired aspect ratio (the values in this example are
        // hard-coded for 16:9, but you could make them into public
        // variables instead so you can set them at design time)
        float targetaspect = 16.0f / 9.0f;
        GetComponent<Camera>().aspect = targetaspect;
    }

}
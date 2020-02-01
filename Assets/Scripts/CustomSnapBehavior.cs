using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomSnapBehavior : MonoBehaviour
{
    public CustomSnapBehavior colliding;
    public Vector3 pos;
    public Vector3 rot;

    public void OnUnGrab()
    {
        Debug.Log("Custom UnGrab Log");
    }

    public void OnGrab(){
        Debug.Log("Custom Grab Log");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolsteredObject : MonoBehaviour
{
    public void OnGrabOther(GameObject go)
    {
        HolsteredObject ho = go.GetComponent<HolsteredObject>();
        if (ho != null)
        {
            
        }
    }
}
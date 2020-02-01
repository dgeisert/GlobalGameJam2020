using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomSnapZone : MonoBehaviour
{
    public CustomSnapBehavior parentPart;


    public void OnTriggerEnter(Collider col)
    {
        CustomSnapBehavior customSnapBehavior = col.GetComponent<CustomSnapBehavior>();
        if(customSnapBehavior == null && customSnapBehavior != parentPart){
            return;
        }
        parentPart.colliding = customSnapBehavior;
    }
    public void OnTriggerLeave(Collider col)
    {

    }
}

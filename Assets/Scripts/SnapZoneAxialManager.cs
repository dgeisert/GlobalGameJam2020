using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapZoneAxialManager : MonoBehaviour
{
    public Transform touchingObject;
    public PartAxelOut[] rots;

    void Update()
    {
        if(touchingObject != null)
        {
            // Setting initial value to -1, the lowest a dot product of two unit vectors can be
            float biggestDotProduct = -1.0f;

            // Setting to -1 as this means a value wasn't chosen (shouldn't happen, represents error)
            int chooseRot = -1;

            for(int i = 0; i < rots.Length; i++)
            {
                // Check for children
                if(rots[i].children.Count > 0)
                {
                    return;
                }

                // Disable all on first pass
                rots[i].gameObject.SetActive(false);

                // This value will get closer to 1 if the angles of the two vectors are close
                float dot = Vector3.Dot(rots[i].transform.forward, touchingObject.forward);

                if(dot > biggestDotProduct)
                {
                    chooseRot = i;
                    biggestDotProduct = dot;
                }
            }
           
            // Enable chosen item
            Debug.Log("Rotation: " + chooseRot);
            rots[chooseRot].gameObject.SetActive(true);
        }
    }
    public void OnTouch(GameObject go)
    {
        if(go.GetComponent<PartBase>() == null)
        {
            return;
        }
        touchingObject = go.transform;
    }
    public void OnUnTouch(GameObject go)
    {
        if(go.GetComponent<PartBase>() == null)
        {
            return;
        }
        if(touchingObject == go.transform)
        {
            touchingObject = null;
        }
    }
}

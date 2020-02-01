using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapZoneAxialManager : MonoBehaviour
{
    public Transform touchingObject;
    public Transform referenceObject;
    public PartAxelOut rot1, rot2, rot3, rot4;

    void Update()
    {
        if(touchingObject != null)
        {
            if(rot1.children.Count > 0 || rot2.children.Count > 0 || rot3.children.Count > 0 || rot4.children.Count > 0)
            {
                return;
            }
            referenceObject.transform.rotation = touchingObject.transform.rotation;
            float angle = referenceObject.localEulerAngles.y + 45;
            while(angle < 0){
                angle += 360;
            }
            while(angle > 360){
                angle -= 360;
            }
            int chooseRot = Mathf.FloorToInt(angle / 90);
            rot1.gameObject.SetActive(chooseRot == 0);
            rot2.gameObject.SetActive(chooseRot == 1);
            rot3.gameObject.SetActive(chooseRot == 2);
            rot4.gameObject.SetActive(chooseRot == 3);
        }
        if(!rot1.gameObject.activeInHierarchy &&
            !rot2.gameObject.activeInHierarchy &&
            !rot3.gameObject.activeInHierarchy &&
            !rot4.gameObject.activeInHierarchy)
        {
            rot1.gameObject.SetActive(true);
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

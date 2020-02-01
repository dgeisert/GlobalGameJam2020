﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SnapZoneAxialManager : MonoBehaviour
{
    public Transform touchingObject;
    public PartAxelOut rot1, rot2, rot3, rot4;
    private GameObject debugText = null;

    public void Start()
    {
        debugText = GameObject.Find("DebugText");
        if (debugText==null) {
            Debug.Log("Debug text canvas not found, continuing.");
        }
    }
    void Update()
    {
        if(touchingObject != null)
        {
            if(rot1.children.Count > 0 || rot2.children.Count > 0 || rot3.children.Count > 0 || rot4.children.Count > 0)
            {
                return;
            }
            float angle = Quaternion.FromToRotation(transform.forward, touchingObject.forward).eulerAngles.magnitude;
            while(angle < 0){
                angle += 360;
            }
            while(angle > 360){
                angle -= 360;
            }
            int chooseRot = Mathf.FloorToInt(angle / 90);
            Debug.Log("Rotation: " + chooseRot);
            if (null!=debugText) {
                Text foo = debugText.GetComponent("Text") as Text;
                foo.text = "Rot: " + chooseRot;
            }
            rot1.gameObject.SetActive(chooseRot == 0);
            rot2.gameObject.SetActive(chooseRot == 1);
            rot3.gameObject.SetActive(chooseRot == 2);
            rot4.gameObject.SetActive(chooseRot == 3);
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

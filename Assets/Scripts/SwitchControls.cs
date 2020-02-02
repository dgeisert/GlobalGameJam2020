using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchControls : MonoBehaviour
{
    public GameObject movement;
    public GameObject controller;
    public GameObject leftRemoteControl;

    public bool movementMode {
        get { return movement.activeInHierarchy; }
        set {
            if (null!=movement) movement.SetActive(value);
            if (null != controller) controller.SetActive(!value);
            if (null != leftRemoteControl) leftRemoteControl.SetActive(!value);
        }
    }

    void Start()
    {
        if (movement==null) {
            Debug.Log("variable movement of SwitchControls has not been assigned. You probably need to assign the movement variable of the SwitchControls script in the inspector.");
            movementMode = true;
            return;
        }
        if (controller==null) {
            Debug.Log("variable controller of SwitchControls has not been assigned. You probably need to assign the controller variable of the SwitchControls script in the inspector.");
            movementMode = true;
            return;
        }
        if (leftRemoteControl==null) {
            Debug.Log("variable leftRemoteControl of SwitchControls has not been assigned. You probably need to assign the leftRemoteControl variable of the SwitchControls script in the inspector.");
            movementMode = true;
            return;
        }
        // Enforce consistent visibility for all objects.
        movementMode = movementMode;
    }

    public void DoSwitchControls()
    {
        movementMode = !movementMode;
    }
}

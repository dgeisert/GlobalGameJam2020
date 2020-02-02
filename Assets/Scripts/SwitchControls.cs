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
            movement.SetActive(value);
            controller.SetActive(!value);
            leftRemoteControl.SetActive(!value);
        }
    }

    void Start()
    {
        // Enforce consistent visibility for all objects.
        movementMode = movementMode;
    }

    public void DoSwitchControls()
    {
        movementMode = !movementMode;
    }
}

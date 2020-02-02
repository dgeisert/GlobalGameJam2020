using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchControls : MonoBehaviour
{
    public GameObject movement;
    public GameObject controller;
    public GameObject leftRemoteControl;

    public void DoSwitchControls()
    {
        bool moving = movement.activeInHierarchy;
        moving = !moving;
        movement.SetActive(moving);
        controller.SetActive(!moving);
        leftRemoteControl.SetActive(!moving);
    }
}

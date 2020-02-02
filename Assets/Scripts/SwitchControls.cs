using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchControls : MonoBehaviour
{
    public GameObject movement;
    public GameObject controller;

    public void DoSwitchControls()
    {
        movement.SetActive(!movement.activeInHierarchy);
        controller.SetActive(!controller.activeInHierarchy);
    }
}

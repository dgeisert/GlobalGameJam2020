using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ControlsListener : MonoBehaviour
{
    [Tooltip("for either hand choose right and select whileHeldOnly")]
    public ControlName controlButton;
    public bool whileHeldOnly = true;
    public UnityEvent actions;
    public string keyboard;

    void Update()
    {
        if (keyboard != "" && Input.GetKeyDown(keyboard))
        {
            Activate();
        }
    }

    public void OnEnable()
    {
        actions.RemoveAllListeners();
        StartCoroutine(AddListenerAfterFrame());
    }
    IEnumerator AddListenerAfterFrame()
    {
        yield return null;
        if (!whileHeldOnly)
        {
            ControlsManager.AddControl(controlButton, this);
        }
    }
    public void OnDisable()
    {
        ControlsManager.RemoveControl(controlButton, this);
    }
    public void OnDestroy()
    {
        ControlsManager.RemoveControl(controlButton, this);
    }
    public void OnGrab(GameObject go)
    {
        Debug.Log("Grab " + transform.name + ", " + actions.GetPersistentTarget(0));
        if (whileHeldOnly)
        {
            ControlsManager.AddControl(go.name.Contains("Left") ? (ControlName) ((int) controlButton - 1) : controlButton, this);
        }
    }
    public void OnUngrab(GameObject go)
    {
        if (whileHeldOnly)
        {
            ControlsManager.RemoveControl(go.name.Contains("Left") ? (ControlName) ((int) controlButton - 1) : controlButton, this);
        }
    }
    public void Activate()
    {
        Debug.Log("Activate " + transform.name + ", " + actions.GetPersistentTarget(0));
        actions.Invoke();
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum ControlName
{
    LeftTriggerPull,
    RightTriggerPull,
    LeftTriggerRelease,
    RightTriggerRelease,
    LeftGripPull,
    RightGripPull,
    LeftGripRelease,
    RightGripRelease,
    ButtonAPress,
    ButtonARelease,
    ButtonBPress,
    ButtonBRelease,
    ButtonXPress,
    ButtonXRelease,
    ButtonYPress,
    ButtonYRelease,
    MenuButtonPress,
    MenuButtonRelease,
    RightJoystickY,
    LeftJoystickY
}

public class ControlsManager : MonoBehaviour
{
    public static ControlsManager Instance;
    static Dictionary<ControlName, List<ControlsListener>> listeners;

    public static void AddControl(ControlName controlName, ControlsListener controlsListener)
    {
        if (!listeners.ContainsKey(controlName))
        {
            listeners.Add(controlName, new List<ControlsListener>());
        }
        if (!listeners[controlName].Contains(controlsListener))
        {
            listeners[controlName].Add(controlsListener);
        }
    }
    public static void RemoveControl(ControlName controlName, ControlsListener controlsListener)
    {
        if (listeners.ContainsKey(controlName) && listeners[controlName].Contains(controlsListener))
        {
            listeners[controlName].Remove(controlsListener);
        }
    }

    public void Awake()
    {
        Instance = this;
        SceneManager.sceneLoaded += OnLevelLoaded;
        listeners = new Dictionary<ControlName, List<ControlsListener>>();
    }

    void OnLevelLoaded(Scene scene, LoadSceneMode mode)
    {
        listeners = new Dictionary<ControlName, List<ControlsListener>>();
    }
    public void ActivateControls(int i)
    {
        if (!listeners.ContainsKey((ControlName) i))
        {
            return;
        }
        foreach (ControlsListener cl in listeners[(ControlName) i])
        {
            cl.Activate();
        }
    }

    public void ActivateControls(int i, float f)
    {
        if (!listeners.ContainsKey((ControlName) i))
        {
            return;
        }
        foreach (ControlsListener cl in listeners[(ControlName) i])
        {
            cl.Activate(f);
        }
    }

    bool rightTriggerPressed;
    bool leftTriggerPressed;
    bool rightGripPressed;
    bool leftGripPressed;
    bool aPressed;
    bool bPressed;
    bool xPressed;
    bool yPressed;
    bool menuPressed;
    public void RightTrigger(bool pressed)
    {
        if (pressed && !rightTriggerPressed)
        {
            rightTriggerPressed = true;
            ActivateControls(1);
        }
        else if (!pressed && rightTriggerPressed)
        {
            rightTriggerPressed = false;
            ActivateControls(3);
        }
    }
    public void LeftTrigger(bool pressed)
    {
        if (pressed && !leftTriggerPressed)
        {
            leftTriggerPressed = true;
            ActivateControls(0);
        }
        else if (!pressed && leftTriggerPressed)
        {
            leftTriggerPressed = false;
            ActivateControls(2);
        }
    }
    public void RightGrip(bool pressed)
    {
        if (pressed && !rightGripPressed)
        {
            rightGripPressed = true;
            ActivateControls(5);
        }
        else if (!pressed && rightGripPressed)
        {
            rightGripPressed = false;
            ActivateControls(7);
        }
    }
    public void LeftGrip(bool pressed)
    {
        if (pressed && !leftGripPressed)
        {
            leftGripPressed = true;
            ActivateControls(4);
        }
        else if (!pressed && leftGripPressed)
        {
            leftGripPressed = false;
            ActivateControls(6);
        }
    }
    public void AButton(bool pressed)
    {
        if (pressed && !aPressed)
        {
            aPressed = true;
            ActivateControls(8);
        }
        else if (!pressed && aPressed)
        {
            aPressed = false;
            ActivateControls(9);
        }
    }
    public void BButton(bool pressed)
    {
        if (pressed && !bPressed)
        {
            bPressed = true;
            ActivateControls(10);
        }
        else if (!pressed && bPressed)
        {
            bPressed = false;
            ActivateControls(11);
        }
    }
    public void XButton(bool pressed)
    {
        if (pressed && !xPressed)
        {
            xPressed = true;
            ActivateControls(12);
        }
        else if (!pressed && xPressed)
        {
            xPressed = false;
            ActivateControls(13);
        }
    }
    public void YButton(bool pressed)
    {
        if (pressed && !yPressed)
        {
            yPressed = true;
            ActivateControls(14);
        }
        else if (!pressed && yPressed)
        {
            yPressed = false;
            ActivateControls(15);
        }
    }
    public void MenuButton(bool pressed)
    {
        if (pressed && !menuPressed)
        {
            menuPressed = true;
            ActivateControls(16);
        }
        else if (!pressed && menuPressed)
        {
            menuPressed = false;
            ActivateControls(17);
        }
    }
    public void RightJoystickY(float f)
    {
        ActivateControls(18, f);
    }
    public void LeftJoystickY(float f)
    {
        ActivateControls(19, f);
    }
}
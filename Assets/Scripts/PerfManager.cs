using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerfManager : MonoBehaviour
{
    private float pushCPUTime = 0f;
    private int lowPriLock = 0;
    private Queue<Action> lowPriActions = new Queue<Action>();
    private static PerfManager instance = null;
    public static PerfManager Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.LogError("No PerfManager component found.");
                return null;
            }

            return instance;
        }
    }
    
    public void Start()
    {
        instance = this;
        EventManager.StartListening("startLevel", resetPerfValues);
        EventManager.StartListening("gameOver", resetPerfValues);
    }

    public void OnDestroy()
    {
        EventManager.StopListening("startLevel", resetPerfValues);
        EventManager.StopListening("gameOver", resetPerfValues);
    }

    IEnumerator resetPerfValues()
    {
        lowPriActions.Clear();
        yield return null;
        yield return null;
        yield return null;
        resetValues();
    }
    public void resetValues()
    {
#if UNITY_ANDROID
        OVRManager.cpuLevel = 2;
        //OVRManager.gpuLevel = 2;
#endif
    }
    public void SetForLoading()
    {
#if UNITY_ANDROID
        OVRManager.cpuLevel = 4;
        //OVRManager.gpuLevel = 0;
#endif
    }
    public void PushCPU(float length=.5f)
    {
        pushCPUTime = Mathf.Max(pushCPUTime, length);
        
        //crank up the CPU
#if UNITY_ANDROID
        if (OVRManager.cpuLevel != 4)
        {
            OVRManager.cpuLevel = 4;
        }
        if(OVRManager.gpuLevel != 4)
        {
            OVRManager.gpuLevel = 4;
        }
#if !UNITY_EDITOR
        Util.SetCurrentFOVLevel(3);
#endif
#endif
    }

    public void AddLowPriAction(Action action)
    {
        lowPriActions.Enqueue(action);
    }
    public void LockLowPriActions(int frames=2) {
        lowPriLock = Math.Max(lowPriLock,frames);
    }
    public bool AvailableForLowPri()
    {
        return lowPriLock == 0 && lowPriActions.Count == 0;
    }

    private void Update()
    {
        if (pushCPUTime > 0)
        {
            pushCPUTime -= Time.unscaledDeltaTime;
            if (pushCPUTime <= 0)
            {
                //turn off cranked CPU
#if UNITY_ANDROID
                OVRManager.cpuLevel = 2;
                OVRManager.gpuLevel = 2;
#if !UNITY_EDITOR
                Util.SetCurrentFOVLevel();
#endif
#endif
            }
        }

        if (lowPriLock > 0)
        {
            lowPriLock--;
        }
        else
        {
            if (lowPriActions.Count > 0)
            {
                lowPriActions.Dequeue()();
            }
        }
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EventManager : MonoBehaviour {
    public static EventManager instance
    {
        get
        {
            if (!eventManager)
            {
                eventManager = FindObjectOfType<EventManager>() as EventManager;
                if (eventManager != null)
                {
                    eventManager.Init(); 
                }
            }

            return eventManager;
        }
    }

    private static EventManager eventManager;
    private Dictionary<string, List<ActionContext>> eventDictionary;
    private Dictionary<string, List<ActionWithPayloadContext>> payloadedEventDictionary;

    void Init ()
    {
        if (eventDictionary == null)
        {
            eventDictionary = new Dictionary<string, List<ActionContext>>();
            payloadedEventDictionary = new Dictionary<string, List<ActionWithPayloadContext>>();
        }
    }

    public static void StartListening(string eventName, Func<IEnumerator> listener, bool runInContext = false)
    {
        List<ActionContext> actions = null;
        if (!instance.eventDictionary.TryGetValue(eventName, out actions))
        {
            actions = new List<ActionContext>();
            instance.eventDictionary[eventName] = actions;
        }
        
        actions.Add(new ActionContext(listener, runInContext));
    }
    
    public static void StartListening(string eventName, Func<GameObject, IEnumerator> listener, bool runInContext = false)
    {
        List<ActionWithPayloadContext> actions = null;
        if (!instance.payloadedEventDictionary.TryGetValue(eventName, out actions))
        {
            actions = new List<ActionWithPayloadContext>();
            instance.payloadedEventDictionary[eventName] = actions;
        }
        
        actions.Add(new ActionWithPayloadContext(listener, runInContext));
    }

    public static void StopListening(string eventName, Func<IEnumerator> listener)
    {
        List<ActionContext> actions = null;
        if (instance != null && instance.eventDictionary != null && instance.eventDictionary.TryGetValue(eventName, out actions))
        {
            actions.RemoveAll(ac=>ac.Action == listener);
        }
    }

    public static void StopListening(string eventName, Func<GameObject, IEnumerator> listener)
    {
        List<ActionWithPayloadContext> actions = null;
        if (instance != null && instance.payloadedEventDictionary != null && instance.payloadedEventDictionary.TryGetValue(eventName, out actions))
        {
            actions.RemoveAll(ac => ac.Action == listener);
        }
    }

    public static void TriggerEvent(string eventName)
    {
        List<ActionContext> actions = null;
        if (instance.eventDictionary.TryGetValue(eventName, out actions))
        {
            var actionsToRun = new ActionContext[actions.Count];
            actions.CopyTo(actionsToRun);
            foreach (var action in actionsToRun)
            {
                if (action.Context)
                {
                    var obj = action.Action.Target as MonoBehaviour;
                    if (obj != null)
                    {
                        obj.StartCoroutine(action.Action());
                    }
                }
                else
                {
                    instance.StartCoroutine(action.Action());
                }
            }
        }
    }

    public static void StopEvent(Func<IEnumerator> action)
    {
        instance.StopCoroutine(action());
    }

    public static void HealthCheck()
    {
        foreach (var e in instance.eventDictionary)
        {
            foreach(var a in e.Value)
            {
                if (a.Action.Target == null)
                {
                    Debug.LogError("DANGLING EVENT TRIGGER: " + e.Key + " " + a.ToString());
                }
            }
        }
    }

    public static void TriggerEvent(string eventName, GameObject payload)
    {
        List<ActionWithPayloadContext> actions = null;
        if (instance.payloadedEventDictionary.TryGetValue(eventName, out actions))
        {
            var actionsToRun = new ActionWithPayloadContext[actions.Count];
            actions.CopyTo(actionsToRun);
            foreach (var action in actionsToRun)
            {
                if (payload != null)
                {
                    if (action.Context)
                    {
                        var obj = action.Action.Target as MonoBehaviour;
                        if (obj != null)
                        {
                            obj.StartCoroutine(action.Action(payload));
                        }
                    }
                    else
                    {
                        instance.StartCoroutine(action.Action(payload));
                    }
                }
                else
                {
                    Debug.LogWarning("Triggering event on destroyed game object.");
                }
            }
        }
    }
}

public class ActionContext
{
    public Func<IEnumerator> Action;
    public bool Context;
    public ActionContext(Func<IEnumerator> action, bool context)
    {
        Action = action;
        Context = context;
    }
}

public class ActionWithPayloadContext
{
    public Func<GameObject, IEnumerator> Action;
    public bool Context;
    public ActionWithPayloadContext(Func<GameObject, IEnumerator> action, bool context)
    {
        Action = action;
        Context = context;
    }
}
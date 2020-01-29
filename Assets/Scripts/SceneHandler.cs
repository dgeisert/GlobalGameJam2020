using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    public static void LoadSceneByName(
        string name, 
        Func<IEnumerator> post = null, 
        bool shouldStartLevel = false, 
        bool instant = false,
        bool activateImmediately = true
    )
    {
        instance.loadSceneByName(name, post, shouldStartLevel, instant, activateImmediately);
    }

    public static void LoadSceneByIndex(
        int index, 
        Func<IEnumerator> post = null, 
        bool shouldStartLevel = false, 
        bool instant = false,
        bool activateImmediately = true
    )
    {
        instance.loadSceneByIndex(index, post, shouldStartLevel, instant, activateImmediately);
    }

    public static void ActivateScene()
    {
        instance.activateScene();
    }

    private static SceneHandler instance;

    private Dictionary<string, List<Func<IEnumerator>>> postSceneNameCallbacks;
    private Dictionary<int, List<Func<IEnumerator>>> postSceneIndexCallbacks;
    private AsyncOperation async;
    private bool isLoading;
    private bool executePostSceneCallbacks;

    public void Start()
    {
        instance = this;
        SceneManager.activeSceneChanged += changedActiveScene;
        
        isLoading = false;
        executePostSceneCallbacks = false;
        postSceneNameCallbacks = new Dictionary<string, List<Func<IEnumerator>>>();
        postSceneIndexCallbacks = new Dictionary<int, List<Func<IEnumerator>>>();
    }

    public void Update()
    {
        if (executePostSceneCallbacks)
        {
            var next = SceneManager.GetActiveScene();
            if (postSceneNameCallbacks.ContainsKey(next.name))
            {
                foreach (var callback in postSceneNameCallbacks[next.name])
                {
                    if (callback != null)
                    {
                        StartCoroutine(callback());
                    }
                }

                postSceneNameCallbacks.Remove(next.name);
            }

            if (postSceneIndexCallbacks.ContainsKey(next.buildIndex))
            {
                foreach (var callback in postSceneIndexCallbacks[next.buildIndex])
                {
                    if (callback != null)
                    {
                        StartCoroutine(callback());
                    }
                }

                postSceneIndexCallbacks.Remove(next.buildIndex);
            }

            executePostSceneCallbacks = false;
        }
    }

    private void loadSceneByName(
        string name, 
        Func<IEnumerator> post = null, 
        bool shouldStartLevel = false, 
        bool instant = false,
        bool activateImmediately = true
    )
    {
        if (!isLoading)
        {
            isLoading = true;
            Action loadAction = () =>
            {
                if (postSceneNameCallbacks.ContainsKey(name))
                {
                    postSceneNameCallbacks[name].Add(post);
                }
                else
                {
                    postSceneNameCallbacks[name] = new List<Func<IEnumerator>>() { post };
                }

                postSceneNameCallbacks[name].Add(doneLoading);

                if (shouldStartLevel)
                {
                    postSceneNameCallbacks[name].Add(startLevel);
                }

                async = SceneManager.LoadSceneAsync(name);

                if (!activateImmediately)
                {
                    async.allowSceneActivation = activateImmediately;
                }
            };

            if (instant)
            {
                loadAction();
            }
            else
            {
                /*
                if (Player.Instance != null)
                {
                    Player.Instance.HidePrimaryWeapon(true, true);
                }

                if (activateImmediately)
                {
                    FadeWall.FadeOut(loadAction);
                }
                else
                {
                    loadAction();
                }
                */
            }
        }
        else
        {
            Debug.LogError("Already loading new scene.");
        }
    }

    private void loadSceneByIndex(
        int index, 
        Func<IEnumerator> post = null, 
        bool shouldStartLevel = false, 
        bool instant = false,
        bool activateImmediately = true
    )
    {
        if (!isLoading)
        {
            isLoading = true;
            Action loadAction = () =>
            {
                if (postSceneIndexCallbacks.ContainsKey(index))
                {
                    postSceneIndexCallbacks[index].Add(post);
                }
                else
                {
                    postSceneIndexCallbacks[index] = new List<Func<IEnumerator>>() { post };
                }

                postSceneIndexCallbacks[index].Add(doneLoading);

                if (shouldStartLevel)
                {
                    postSceneIndexCallbacks[index].Add(startLevel);
                }

                async = SceneManager.LoadSceneAsync(index);

                if (!activateImmediately)
                {
                    async.allowSceneActivation = activateImmediately;
                }
            };

            if (instant)
            {
                loadAction();
            }
            else
            {
                /*
                if (Player.Instance != null)
                {
                    Player.Instance.HidePrimaryWeapon(true, true);
                }

                if (activateImmediately)
                {
                    FadeWall.FadeOut(loadAction);
                }
                else
                {
                    loadAction();
                }
                */
            }
        }
        else
        {
            Debug.LogError("Already loading new scene.");
        }
    }

    private void activateScene()
    {
        async.allowSceneActivation = true;
    }

    private IEnumerator doneLoading()
    {
        isLoading = false;
        yield break;
    }

    private IEnumerator startLevel()
    {
        EventManager.TriggerEvent("startLevel");
        yield break;
    }

    private void changedActiveScene(Scene current, Scene next)
    {
        executePostSceneCallbacks = true;
    }
}
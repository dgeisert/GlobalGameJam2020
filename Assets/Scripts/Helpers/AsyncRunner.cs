using System.Collections;
using UnityEngine;


public class AsyncRunner: MonoBehaviour
{
    public static AsyncRunner GetRunner(string name)
    {
        var gameObject = new GameObject(name);
        return gameObject.AddComponent<AsyncRunner>();
    }

    public Coroutine Run(IEnumerator action)
    {
        return StartCoroutine(action);
    }
}
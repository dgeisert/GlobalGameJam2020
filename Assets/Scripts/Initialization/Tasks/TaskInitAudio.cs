using UnityEngine;
using System.Collections;

public class TaskInitAudio : IInitTask
{
    private GameObject AudioObject;
    public TaskInitAudio(GameObject audioObject)
    {
        AudioObject = audioObject;
    }
    public string GetName()
    {
        return "InitAudio";
    }

    public IEnumerator Execute()
    {
        yield return null;
        AudioObject.SetActive(true);
        yield break;
    }
}
using UnityEngine;
using System.Collections;

public class TaskWait : IInitTask
{
    private float Length;
    public TaskWait(float length)
    {
        Length = length;
    }
    public string GetName()
    {
        return "Wait";
    }

    public IEnumerator Execute()
    {
        yield return new WaitForSecondsRealtime(Length);
        yield break;
    }
}
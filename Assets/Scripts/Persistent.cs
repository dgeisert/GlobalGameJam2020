using UnityEngine;

public class Persistent : MonoBehaviour
{
    public void Awake()
    {
        DontDestroyOnLoad(this);
    }
}
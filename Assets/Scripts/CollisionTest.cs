using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollisionTest : MonoBehaviour
{
    private Text debugText = null;
    void Start()
    {
        GameObject debugObj = GameObject.Find("DebugText");
        if (null!=debugObj) {
            debugText = debugObj.GetComponent<Text>();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        Log("OnCollisionEnter on " + gameObject.name + " with " + collision.gameObject.name);
    }

    void OnTriggerEnter(Collider other)
    {
        Log("OnTriggerEnter on " + gameObject.name + " with " + other.gameObject.name);
    }

    void Log(string msg) {
        Debug.Log(msg);
        if (null!=debugText) {
            debugText.text = msg;
        }
    }
}

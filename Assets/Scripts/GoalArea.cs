using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GoalArea : MonoBehaviour
{
    // We can display debug text there, if it's enabled.
    private Text debugText = null;

    // Start is called before the first frame update
    void Start()
    {
        GameObject debugObj = GameObject.Find("DebugText");
        if (null!=debugObj) {
            debugText = debugObj.GetComponent<Text>();
        }
    }

    // Called by the engine on collision. Or is it?
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name=="Core") {
            Log("Victory!");
        }
    }

    void Log(string msg) {
        if (null==debugText) {
            Debug.Log(msg);
            return;
        }
        debugText.text = msg;
    }    

}

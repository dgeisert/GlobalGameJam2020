using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

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
        if (RootGameObject(other.gameObject).name=="Core") {
            Log("Victory!");
            SceneManager.LoadScene("Level_Select");
        }
    }

    void Log(string msg) {
        if (null==debugText) {
            Debug.Log(msg);
            return;
        }
        debugText.text = msg;
    }

    GameObject RootGameObject(GameObject obj) {
        while (true) {
            if (obj.transform == null) { return obj; }
            if (obj.transform.parent == null) { return obj; }            
            if (obj.transform.parent.gameObject == null) { return obj; }            
            if (obj.transform.parent.gameObject == obj) { return null; }
            obj = obj.transform.parent.gameObject;
        }
    }

}

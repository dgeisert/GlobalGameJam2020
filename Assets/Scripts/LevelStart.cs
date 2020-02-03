using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelStart : MonoBehaviour
{

    public string levelName;
    private Text debugText = null;
    

    // Start is called before the first frame update
    void Start()
    {
        GameObject debugObj = GameObject.Find("DebugText");
        if (null!=debugObj) {
            debugText = debugObj.GetComponent<Text>();
        }
    }

    // Only possible collision is the user's hand, let's go!
    void OnTriggerEnter(Collider other)
    {
        DoIt();
    }

    public void DoIt() {
        Log("Loading " + levelName);
        SceneManager.LoadScene(levelName);
    }

    private void Log(string logMessage)
    {
        Debug.Log(logMessage);
        if (null!=debugText) {
            Text foo = debugText.GetComponent("Text") as Text;
            foo.text = logMessage;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * Play a horn sound. Ideally when we're holding the remote controller, both
 * the antenna and the horn are connected to the core, and the player presses
 * the "A" button. Or, you know, when we press the "H" key.
 **/
public class HornSounder : MonoBehaviour
{
    // We can display debug text there, if it's enabled.
    private GameObject debugText = null;
    private AudioSource audio = null;

    void Start()
    {
        debugText = GameObject.Find("DebugText");
        audio = GetComponent<AudioSource>();
    }


    void Update()
    {
        if (Input.GetKey("h")) {
            Honk();
        }
    }

    public void Honk() {
        // TODO: check we're connected in all the right ways
        audio.Play();
        Log("Honk!");
    }

    private void Log(string logMessage)
    {
        if (null!=debugText) {
            Text foo = debugText.GetComponent("Text") as Text;
            foo.text = logMessage;
        }
    }
}

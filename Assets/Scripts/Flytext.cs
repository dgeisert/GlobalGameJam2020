using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Flytext : MonoBehaviour {
    public TextMeshPro Text;
    public float Lifetime = 1f;
    private float elapsed = 0f;
	// Update is called once per frame
	void Update () {

        elapsed += Time.deltaTime;

        if (elapsed >= Lifetime)
        {
            Destroy(gameObject);
        }
        else
        {
            transform.Translate(Vector3.up * Time.deltaTime * 0.5f);
            var color = Text.color;
            color.a = 1 - elapsed / Lifetime;
            Text.color = color;
        }
        
    }

	public void SetText(string str, Color clr){
        Text.text = str;
        Text.color = clr;
	}
}

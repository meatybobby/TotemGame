using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TextEffect : MonoBehaviour {
	

	private Text startViewText;
	
	// Use this for initialization
	void Start () {
		startViewText = GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		Color textColor = new Color (startViewText.color.r,startViewText.color.g,startViewText.color.b,Mathf.Sin(Time.time * 3.0f));
		startViewText.color = textColor;
	}
}

using UnityEngine;
using System.Collections;

public class Show_FPS : MonoBehaviour {

	float timeA;
	public float fps;
	public float lastFPS;

	// Use this for initialization
	void Start () {
		timeA = Time.timeSinceLevelLoad;

	}
	// Update is called once per frame
	void Update () {
		//Debug.Log(Time.timeSinceLevelLoad+" "+timeA);
		if(Time.timeSinceLevelLoad  - timeA <= 1)
		{
			fps++;
		}
		else
		{
			lastFPS = fps + 1;
			timeA = Time.timeSinceLevelLoad;
			fps = 0;
		}
	}
	void OnGUI()
	{
		GUI.Label(new Rect( 450,5, 30,30),""+lastFPS);
	}
}

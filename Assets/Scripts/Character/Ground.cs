using UnityEngine;
using System.Collections;

public class Ground : Character {

	public GameObject steam;

	// Use this for initialization
	void Start () {
		Invoke("SummonSteam", 1f);
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	protected void SummonSteam() {
		GameObject steamObj = Instantiate(steam, transform.position, transform.rotation) as GameObject;
		Destroy (steamObj, 1f);
	}
}

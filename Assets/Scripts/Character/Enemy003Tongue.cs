using UnityEngine;
using System.Collections;

public class Enemy003Tongue : MonoBehaviour {

	private Enemy003 enemy003;
	// Use this for initialization
	void Start () {
		enemy003 = transform.parent.gameObject.GetComponent<Enemy003>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Player" || other.tag=="Totem") {
			enemy003.setCollideSomethingTrue();
		}
		
	}
}

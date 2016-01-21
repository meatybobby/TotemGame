using UnityEngine;
using System.Collections;

public class Enemy004Attacker : MonoBehaviour {
	public Enemy004 enemy004;
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}
	

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Player" || other.tag == "Totem" || other.tag == "Rock") {
			Character c = other.GetComponent<Character> ();
			c.CauseDamage (enemy004.damage);
		}
	}
}

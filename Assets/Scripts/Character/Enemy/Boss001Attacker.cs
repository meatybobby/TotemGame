using UnityEngine;
using System.Collections;

public class Boss001Attacker : MonoBehaviour {
	public Boss001 boss001;
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}
	

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Player" || other.tag == "Totem" || other.tag == "Rock") {
			Character c = other.GetComponent<Character> ();
			c.CauseDamage (boss001.damage);
		}
	}
}

using UnityEngine;
using System.Collections;

public class Enemy001Attacker : MonoBehaviour {

	public float speed;
	public int damage = 1;

	// Use this for initialization
	void Start () {
		GetComponent<Rigidbody2D> ().velocity = -transform.up * speed;

	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	void OnTriggerEnter2D(Collider2D other) {
		// Destroy everything that leaves the trigger
		if (other.tag == "Player" || other.tag=="Totem") {
			Destroy (gameObject);
			Character target = other.gameObject.GetComponent<Character> ();
			target.CauseDamage(damage);
		}
		
	}
}

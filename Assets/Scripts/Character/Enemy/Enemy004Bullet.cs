using UnityEngine;
using System.Collections;

public class Enemy004Bullet : MonoBehaviour {
	public float speed;
	public int damage;
	
	void Start () {
		GetComponent<Rigidbody2D> ().velocity = transform.up * speed;
	}
	
	
	
	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Totem" || other.tag == "Player" || other.tag == "Rock") {
			Character c = other.GetComponentInParent<Character> ();
			if (c != null)
				c.CauseDamage (damage);
			Destroy (gameObject);
		}
	}
}

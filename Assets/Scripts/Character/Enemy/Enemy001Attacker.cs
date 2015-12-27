using UnityEngine;
using System.Collections;

public class Enemy001Attacker : MonoBehaviour {

	public float speed;
	public Enemy001 enemy001;
	
	// Use this for initialization
	void Start () {
		GetComponent<Rigidbody2D> ().velocity = -transform.up * speed;
		Destroy (this.gameObject, 0.6f);
	}
	
	// Update is called once per frame
	void Update () {
	}


	void OnTriggerEnter2D(Collider2D other) {
		// Destroy everything that leaves the trigger
		if (other.tag == "Player" || other.tag=="Totem" || other.tag=="Rock") {
			Destroy (this.gameObject);
			Character target = other.GetComponent<Character> ();
			target.CauseDamage(enemy001.damage);
			/*if(other.tag=="Player") {
				Player p = other.GetComponent<Player>();
				p.CauseDamage(enemy001.damage);
			}
			else {
				Totem t = other.GetComponent<Totem>();
				//target.CauseDamage(enemy001.damage);
				t.CauseDamage(enemy001.damage);
			}*/
		}
		
	}
}

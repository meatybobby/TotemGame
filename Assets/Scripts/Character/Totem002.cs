using UnityEngine;
using System.Collections;

public class Totem002 : Totem {

	public Animator anim;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
	
	}

	void OnTriggerEnter2D(Collider2D other) {
		// Destroy everything that leaves the trigger
		if (other.tag=="MonsterHand" 
		    /*|| other.tag == "bullet"*/ ) {
			//Debug.Log ("totem attacked by monster!");
			HP--;
			if(HP==0) {
				anim.SetTrigger("isDie");
				Destroy (GetComponent<BoxCollider2D>());
				Destroy(gameObject, 2.5f);
				Map.Destroy(this);
			}
		}
		
	}
}

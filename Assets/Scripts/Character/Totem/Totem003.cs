using UnityEngine;
using System.Collections;

public class Totem003 : Totem {

	public Animator anim;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
		Initialize ();
	}

	// Update is called once per frame
	void Update(){
		//Hp GUI
		HpUpdate ();
		ResetRangePosition();
		if(HP<=0 && !isDead) {
			Die ();
		}
	}

	public void Die() {
		anim.Play ("Totem003_die");
		//Destroy (GetComponent<BoxCollider2D>());
		base.Die ();
	}

	void OnTriggerEnter2D (Collider2D other) {
		// Destroy everything that leaves the trigger
	
	}
}

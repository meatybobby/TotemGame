using UnityEngine;
using System.Collections;

public class Totem002 : Totem {

	public Animator anim;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
		// the steam animation when created
		Initialize(2);

	}
	void Update() {
		if(HP<=0 && !isDead) {
			Die ();
		}
	}
	public void Die() {
		anim.SetTrigger("isDie");
		Destroy (GetComponent<BoxCollider2D>());
		base.Die ();
	}

	void OnTriggerEnter2D(Collider2D other) {

		
	}
}

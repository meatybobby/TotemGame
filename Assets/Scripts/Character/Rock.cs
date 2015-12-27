using UnityEngine;
using System.Collections;

public class Rock : Ground {

	public Animator anim;

	void Start () {
		anim = GetComponent<Animator>();
	
	}
	

	void Update () {
		if(HP<=0 && !isDead) {
			Die ();
		}
	}

	private void Die() {
		isDead = true;
		anim.Play("rock_die");
		Map.Destroy (this);
		Destroy (GetComponent<Collider2D> ());
		Destroy (this);
		Destroy (gameObject, 1.5f);
	}
}

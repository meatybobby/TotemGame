using UnityEngine;
using System.Collections;

public class Rock : Ground {

	public Animator anim;
	public AudioClip stoneBreakSound;

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
		GetComponent<AudioSource>().PlayOneShot(stoneBreakSound, 0.7f);
		anim.Play("rock_die");
		Map.Destroy (this);
		Destroy (GetComponent<Collider2D> ());
		Destroy (this);
		Destroy (gameObject, 2f);
	}
}

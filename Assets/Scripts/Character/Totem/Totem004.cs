using UnityEngine;
using System.Collections;

public class Totem004 : Totem {

	public Animator anim;

	public int healPoint;
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
		Initialize ();
	}
	
	// Update is called once per frame
	void Update () {
		//Hp GUI
		HpUpdate ();
		ResetRangePosition();
		if(HP<=0 && !isDead) {
			Die ();
		}
	}

	public void Die() {
		//Destroy (GetComponent<BoxCollider2D>());
		anim.Play ("totem004_die");
		base.Die ();
	}
}

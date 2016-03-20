using UnityEngine;
using System.Collections;

public class Totem004 : Totem {

	public Animator anim;
	public int healPoint;

	void Start () {
		anim = GetComponent<Animator>();
		Initialize ();
	}
	
	void Update () {
		//Hp GUI
		HpUpdate ();
		ResetRangePosition();
		if(HP<=0 && !isDead) {
			Die ();
		}
	}

	public void Die() {
		anim.Play ("totem004_die");
		base.Die ();
	}
}

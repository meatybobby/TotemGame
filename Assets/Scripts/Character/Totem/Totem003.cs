using UnityEngine;
using System.Collections;

public class Totem003 : Totem {

	public Animator anim;
 	
	void Start () {
		anim = GetComponent<Animator>();
		Initialize ();
	}

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
		base.Die ();
	}

}

using UnityEngine;
using System.Collections;

public class Totem002 : Totem {

	public Animator anim;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
		// the steam animation when created
		GameObject steamObj = Instantiate(steam, transform.position, transform.rotation) as GameObject;
		Destroy (steamObj, 1f);
		characterId = 2;

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
				transform.FindChild("TotemCatchLight").gameObject.SetActive(false);
				Destroy(gameObject, 2.5f);
				if(isCaught) {
					playerRef.SetIdle();
				}
				Map.Destroy(this);
			}
		}
		
	}
}

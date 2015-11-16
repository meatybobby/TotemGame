using UnityEngine;
using System.Collections;

public class Player_animate : MonoBehaviour {

	protected Animator anim;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();

	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.W)) {
			anim.SetTrigger ("back_run");
		}
		if (Input.GetKeyDown (KeyCode.S)) {
			anim.SetTrigger ("front_run");
		}
		if (Input.GetKeyDown (KeyCode.A)) {
			anim.SetTrigger ("left_run");
		}
		if (Input.GetKeyDown (KeyCode.D)) {
			anim.SetTrigger ("right_run");
		}





		if (Input.GetKeyDown (KeyCode.DownArrow)) {

			anim.SetTrigger ("front_idle");
		}
		if (Input.GetKeyDown (KeyCode.UpArrow)) {

			anim.SetTrigger ("back_idle");

		}
		if (Input.GetKeyDown (KeyCode.RightArrow)) {

			anim.SetTrigger ("right_idle");
		}
		if (Input.GetKeyDown (KeyCode.LeftArrow)) {

			anim.SetTrigger ("left_idle");
		}

	
	}



}

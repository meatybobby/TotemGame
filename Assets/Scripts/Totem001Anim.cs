using UnityEngine;
using System.Collections;

public class Totem001Anim : MonoBehaviour {

	private Animator anim;
	GameObject front , back , left , right;

	IntVector2 LEFT = new IntVector2 (-1,0);
	IntVector2 RIGHT = new IntVector2 (1,0);
	IntVector2 UP = new IntVector2 (0,1);
	IntVector2 DOWN = new IntVector2 (0,-1);
	
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
		front = this.transform.FindChild("Totem001_front").gameObject;
		back = this.transform.FindChild("Totem001_back").gameObject;
		left = this.transform.FindChild("Totem001_left").gameObject;
		right = this.transform.FindChild("Totem001_right").gameObject;
	}


	public void playAnim(IntVector2 dir , int mode){
		front.SetActive(false);
		back.SetActive(false);
		right.SetActive(false);
		left.SetActive (false);

		if (dir == LEFT) {
			left.SetActive(true);
			switch(mode){
			case 0:
				break;
			case 1:
				anim.SetTrigger ("left_fire");
				break;
			case 2:
				anim.SetTrigger ("left_die");
				break;
			default :
				break;
			}
		} else if (dir == RIGHT) {
			right.SetActive(true);
			switch(mode){
			case 0:
				break;
			case 1:
				anim.SetTrigger ("right_fire");
				break;
			case 2:
				anim.SetTrigger ("right_die");
				break;
			default :
				break;
			}
		} else if (dir == UP) {
			back.SetActive(true);
			switch(mode){
			case 0:
				break;
			case 1:
				anim.SetTrigger ("back_fire");
				break;
			case 2:
				anim.SetTrigger ("back_die");
				break;
			default :
				break;
			}

		} else if (dir == DOWN) {
			front.SetActive(true);
			switch(mode){
			case 0:
				anim.SetTrigger ("front_idle");
				break;
			case 1:
				anim.SetTrigger ("front_fire");
				break;
			case 2:
				anim.SetTrigger ("front_die");
				break;
			default :
				break;
			}
			
		}
	
	}




}

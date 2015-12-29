using UnityEngine;
using System.Collections;

public class Enemy002Anim : MonoBehaviour {

	private Animator anim;
	public GameObject front , back , left;


	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
	//	front = this.transform.FindChild("Enemy002_front").gameObject;
	//	back = this.transform.FindChild("Enemy002_back").gameObject;
	//	left = this.transform.FindChild("Enemy002_left").gameObject;
		//right = this.transform.FindChild("Enemy001_right").gameObject;
	
	}

	public void playAnim(IntVector2 dir, int mode) {
		front.SetActive(false);
		back.SetActive(false);
		//right.SetActive(false);
		left.SetActive (false);

		if (dir == Direction.DOWN) {
			front.SetActive (true);
			switch (mode) {
			case 0:
				//anim.Play("Enemy002_front_walk");
				anim.SetTrigger("front_walk");
				break;
			case 1:
				anim.SetTrigger ("front_attack");
				break;
			case 2:
				anim.SetTrigger ("front_die");
				break;
			case 3:
				anim.SetTrigger("front_idle");
				break;
			default :
				break;
			}
		} else if (dir == Direction.UP) {
			back.SetActive (true);
			switch (mode) {
			case 0:
				anim.SetTrigger ("back_walk");
				break;
			case 1:
				anim.SetTrigger ("back_attack");
				break;
			case 2:
				anim.SetTrigger ("back_die");
				break;
			case 3:
				anim.SetTrigger("back_idle");
				break;
			default :
				break;
			}
		} else if (dir == Direction.LEFT) {
			left.SetActive (true);
			switch (mode) {
			case 0:
				//anim.SetTrigger ("left_walk");
				anim.Play("Enemy002_left_walk");
				break;
			case 1:
				anim.SetTrigger ("left_attack");
				break;
			case 2:
				anim.SetTrigger ("left_die");
				break;
			case 3:
				anim.SetTrigger("left_idle");
				break;
			default :
				break;
			}

		} else if (dir == Direction.RIGHT) {
			left.SetActive (true);
			switch (mode) {
			case 0:
				//anim.SetTrigger ("right_walk");
				anim.Play("Enemy002_right_walk");
				break;
			case 1:
				anim.SetTrigger ("right_attack");
				break;
			case 2:
				anim.SetTrigger ("right_die");
				break;
			case 3:
				anim.SetTrigger("right_idle");
				break;
			default :
				break;
			}
		}
		
	}

}

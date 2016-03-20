using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Totem004HealArea : MonoBehaviour {

	public float healIntv = 2.0f;
	public Totem004 totem;
	public AudioClip healSound;

	private bool healFlag;
	private List<GameObject> friendList;

	void Start () {
		totem = GetComponentInParent<Totem004> ();
		friendList = new List<GameObject> ();
		healFlag = false;
	}
	
	void Update () {
		friendList.RemoveAll (item => item == null);

		if(friendList.Count > 0 && healFlag == false && !totem.isDead){
			healFlag = true;
			StartCoroutine (Heal());
		}
	}
	
	protected IEnumerator Heal() {
		totem.anim.Play ("totem004_heal");
		GetComponent<AudioSource>().PlayOneShot(healSound, 0.5f);
		foreach (GameObject friend in friendList) {
			if (friend.GetComponent<Character> () != null)
				friend.GetComponent<Character> ().HealHP (totem.healPoint);
		}
		yield return new WaitForSeconds (healIntv);
		healFlag = false;
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject!=totem.gameObject && (other.tag == "Totem" || other.tag =="Player") ) {
			friendList.Add (other.gameObject);
		}
	}
	
	void OnTriggerExit2D(Collider2D other){
		if (other.tag == "Totem" || other.tag =="Player") {
			friendList.Remove (other.gameObject);
		}
	}
}

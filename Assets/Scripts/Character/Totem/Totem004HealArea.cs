using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Totem004HealArea : MonoBehaviour {

	public float healIntv = 2.0f;
	private List<GameObject> friendList;

	public Totem004 totem;
	private bool healFlag;

	public AudioClip healSound;

	// Use this for initialization
	void Start () {
		totem = GetComponentInParent<Totem004> ();
		friendList = new List<GameObject> ();
		healFlag = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (totem == null) {
			Destroy(this);
		}
		friendList.RemoveAll (item => item == null);
		//Debug.Log (friendList.Count);
		if(friendList.Count > 0 && healFlag == false && !totem.isDead){
			healFlag = true;
			//attack animate and substract enemy's hp
		//	Debug.Log("Heal somebody");
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
		//Debug.Log ("something detected");
		if (other.gameObject!=totem.gameObject && (other.tag == "Totem" || other.tag =="Player") ) {
			//Debug.Log ("new enemy detected");
			friendList.Add (other.gameObject);
		}
	}
	
	void OnTriggerExit2D(Collider2D other){
		if (other.tag == "Totem" || other.tag =="Player") {
			friendList.Remove (other.gameObject);
		}
	}
}

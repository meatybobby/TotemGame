using UnityEngine;
using System.Collections;

public class Enemy003AttackArea : MonoBehaviour {

	public Enemy003 host;
	
	void Start () {
		host = GetComponentInParent<Enemy003> ();
	}

	void Update () {
		// Remove the targets that have been destroyed
		host.attackTargets.RemoveAll (item => item == null);
	}

	void OnTriggerEnter2D(Collider2D other) {
		//Debug.Log ("something detected");
		if (other.tag == "Player" || other.tag == "Totem" || other.tag=="Rock") {
			//Debug.Log ("new target detected");

			host.attackTargets.Add(other.GetComponent<Character>());
		}
	}

	void OnTriggerExit2D(Collider2D other){
		if (other.tag == "Player" || other.tag == "Totem" || other.tag=="Rock") {
			host.attackTargets.Remove(other.GetComponent<Character>());
		}
	}
}

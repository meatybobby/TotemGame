using UnityEngine;
using System.Collections;

public class Enemy003AttackArea : MonoBehaviour {

	public Enemy003 host;

	// Use this for initialization
	void Start () {
		host = GetComponentInParent<Enemy003> ();
	}
	
	// Update is called once per frame
	void Update () {
		host.attackTargets.RemoveAll (item => item == null);  
	}


	void OnTriggerEnter2D(Collider2D other){
		//Debug.Log ("something detected");
		if (other.tag == "Player" || other.tag == "Totem") {
			//Debug.Log ("new enemy detected");
			host.attackTargets.Add(other.gameObject);
		}
	}

	void OnTriggerExit2D(Collider2D other){
		if (other.tag == "Player" || other.tag == "Totem") {
			host.attackTargets.Remove(other.gameObject);
		}
	}
}

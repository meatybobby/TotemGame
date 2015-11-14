using UnityEngine;
using System.Collections;

public class Totem : Character {

	public bool isCaught;
	public Player playerRef;
	public Transform shotSpawn;
	public GameObject shot;

	public float shotIntv;

	// Use this for initialization
	void Start () {
		StartCoroutine (Shooting ());
	}


	IEnumerator Shooting () {
		while(true) {
			GameObject obj = Instantiate(shot, shotSpawn.position, shotSpawn.rotation) as GameObject;
			obj.GetComponent<Rigidbody2D>().velocity = -transform.up * 6;
			yield return new WaitForSeconds(shotIntv);
		}
		
	}

	void OnTriggerEnter2D(Collider2D other) {
		// Destroy everything that leaves the trigger
		if (other.tag == "MonsterHand") {
			Debug.Log ("attacked by monster!");
			HP--;
			if(HP<=0) {
				Destroy(gameObject);
				Map.Destroy(this);
			}
		}
		
	}
}

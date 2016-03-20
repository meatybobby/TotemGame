using UnityEngine;
using System.Collections;

public class Enemy004Attacker : MonoBehaviour {
	public Enemy004 enemy004;
	public Vector3 originPos;
	
	// Use this for initialization
	void Start () {
		originPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
	}
	

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Player" || other.tag == "Totem" || other.tag == "Rock") {
			Character c = other.GetComponent<Character> ();
			c.CauseDamage (enemy004.damage);
		}
	}

	public void hideTail() {
		Debug.Log("Hide");
		transform.position = new Vector3 (originPos.x, -5, originPos.z);
		enemy004.isAttack = false;
	}
	
}

using UnityEngine;
using System.Collections;

public class Enemy002Tongue : MonoBehaviour {

	private Enemy002 enemy002;

	// Use this for initialization
	void Start () {
		enemy002 = transform.parent.gameObject.GetComponent<Enemy002>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetZPos(float z) {
		transform.position = new Vector3 (transform.position.x, transform.position.y, z);
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Player" || other.tag=="Totem" || other.tag=="Rock") {
			enemy002.setCollideSomethingTrue();
			other.GetComponent<Character> ().CauseDamage (enemy002.damage);
		}
	}
	void OnTriggerExit2D(Collider2D other) {
		if(other.tag=="Boundary"){
			Debug.Log ("hit boundary");
			enemy002.setCollideSomethingTrue();
		}
	}
}

using UnityEngine;
using System.Collections;

public class TotemBullet : MonoBehaviour {

	private Animator anim;
	public float speed;
	public Totem shooter;
	public float destroyTime = 0.001f;

	void Start () {
		anim = GetComponent<Animator>();
		GetComponent<Rigidbody2D> ().velocity = transform.right * speed;
	}



	void OnTriggerEnter2D(Collider2D other) {

		if (other.tag == "Enemy" || other.tag=="Player" ||
		    (other.tag == "Totem" && other.GetComponent<Totem> () != shooter) ) {
			// Can't be destroyed by the collider of th totem who shot the bullet
			Debug.Log ("bullet in collider");
			Destroy (GetComponent<CircleCollider2D>()); // trigger only once;

			GetComponent<Rigidbody2D> ().velocity = new Vector3(0f,0f,0f);
			anim.SetTrigger("isBoom");
			Destroy(gameObject, 1f);
		}
		
	}

}

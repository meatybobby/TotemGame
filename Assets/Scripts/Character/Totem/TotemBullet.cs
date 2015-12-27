using UnityEngine;
using System.Collections;

public class TotemBullet : MonoBehaviour {

	private Animator anim;
	public float speed;
	public Totem001 shooter;
	public float destroyTime = 0.001f;


	void Start () {
		anim = GetComponent<Animator>();
		GetComponent<Rigidbody2D> ().velocity = transform.right * speed;
	}



	void OnTriggerEnter2D(Collider2D other) {

		if (other.tag == "Enemy" || other.tag=="Player" || other.tag=="Rock" ||
		    (other.tag == "Totem" && other.GetComponent<Totem001> () != shooter) ) {
			// Can't be destroyed by the collider of the totem who shot the bullet
			if (other.tag=="Enemy" || other.tag=="Rock") {
				Character c = other.GetComponent<Character> ();
				if(c!=null)
					c.CauseDamage (shooter.damage);
			}

			Destroy (GetComponent<CircleCollider2D>()); // trigger only once;

			GetComponent<Rigidbody2D> ().velocity = new Vector3(0f,0f,0f);
			anim.SetTrigger("isBoom");
			// rotate to zero degree for the explosion animation
			transform.rotation = Quaternion.Euler (0f, 0f, 0f);
			Destroy(gameObject, 1f);
		}
		
	}

}

using UnityEngine;
using System.Collections;

public class PreventBullet : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other) {
		// Destroy everything that leaves the trigger
		if (other.tag == "bullet") {
			Destroy(other.gameObject);
		}

	}
}

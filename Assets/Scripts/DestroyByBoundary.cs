using UnityEngine;
using System.Collections;

public class DestroyByBoundary : MonoBehaviour {

	void OnTriggerExit2D(Collider2D other) {
		// Destroy everything that leaves the trigger
		Debug.Log (other);
		Destroy(other.gameObject);
	}
}

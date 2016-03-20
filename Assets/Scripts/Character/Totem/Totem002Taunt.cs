using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Totem002Taunt : MonoBehaviour {

	public List<Enemy> enemyList;

	private Totem002 totem002;
	private bool isIdle = true;
	
	void Start () {
		enemyList = new List<Enemy> ();
		totem002 = GetComponentInParent<Totem002> ();
	}

	void Update() {
		// remomve the enemies that have been destroyed
		enemyList.RemoveAll (item => item == null);
	
		if (!isIdle && enemyList.Count==0) {
			isIdle = true;
			totem002.anim.SetTrigger("isIdle");
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Enemy") {
			totem002.anim.SetTrigger("isTaunt");
			isIdle = false;
			Enemy enemy = other.GetComponent<Enemy> ();
			enemy.GetAngry ();

			if (!enemyList.Contains(enemy)) {
				enemyList.Add (enemy);
			}
			enemy.SetPriority (totem002.characterId,0);
		}
	}
	
	void OnTriggerExit2D(Collider2D other) {
		if (other.tag == "Enemy") {
			totem002.anim.SetTrigger("isIdle");
			Enemy enemy = other.GetComponent<Enemy> ();
			enemy.CancelAngry ();

			enemy.SetPriority (totem002.characterId,enemy.defaultPriority[totem002.characterId]);
			enemyList.Remove (enemy);
		}
	}
}


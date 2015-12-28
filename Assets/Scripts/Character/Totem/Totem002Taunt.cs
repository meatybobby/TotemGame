using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//嘲諷區域
public class Totem002Taunt : MonoBehaviour {

	private Totem002 totem002;
	public List<Enemy> enemyList;
	private bool isIdle = true;
	
	void Start () {
		enemyList = new List<Enemy> ();
		totem002 = GetComponentInParent<Totem002> ();
	}

	void Update() {
		enemyList.RemoveAll (item => item == null);
	
		if (!isIdle && enemyList.Count==0) {
			isIdle = true;
			totem002.anim.SetTrigger("isIdle");
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		//Debug.Log (other.tag);
		if (other.tag == "Enemy") {
			//Debug.Log("Taunt");
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
			//Debug.Log ("Enenmy exit collider!");
			totem002.anim.SetTrigger("isIdle");
			Enemy enemy = other.GetComponent<Enemy> ();
			enemy.CancelAngry ();

			enemy.SetPriority (totem002.characterId,enemy.defaultPriority[totem002.characterId]);
			enemyList.Remove (enemy);
		}
	}
}

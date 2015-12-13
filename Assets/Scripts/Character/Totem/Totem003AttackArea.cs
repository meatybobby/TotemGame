using UnityEngine;
using System.Collections;
using System.Collections.Generic;
// 震盪波區域
public class Totem003AttackArea : MonoBehaviour {

	public float attackIntv = 2.0f;
	public float attackWait = 0.5f;
	public GameObject waveEffect;
	public int damage = 1;

	private List<GameObject> enemyList;
	private bool attackFlag;
	public Totem003 totem;

	void Start () {
		totem = GetComponentInParent<Totem003> ();
		enemyList = new List<GameObject> ();
		attackFlag = false;
	}
		
	void Update () {
		// remomve the enemies that have been destroyed
		enemyList.RemoveAll (item => item == null);  

		//Debug.Log ("List count : " + enemyList.Count + " attackFlag : " + attackFlag);
		if(enemyList.Count != 0 && attackFlag == false && totem.HP > 0){
			attackFlag = true;
			//attack animate and substract enemy's hp
			totem.anim.Play("Totem003_attack");
			StartCoroutine (CreateWave());
		}
	}
	protected IEnumerator CreateWave() {
		yield return new WaitForSeconds (attackWait);
		GameObject wave = Instantiate(waveEffect,new Vector3(transform.position.x,transform.position.y+0.12f,transform.position.z),transform.rotation) as GameObject;
		foreach (GameObject enemy in enemyList) {
			if (enemy != null)
				enemy.GetComponent<Enemy> ().CauseDamage (damage);
		}
		Destroy (wave, 0.8f);
		yield return new WaitForSeconds (attackIntv);
		attackFlag = false;
	}


	void OnTriggerEnter2D(Collider2D other){
		//Debug.Log ("something detected");
		if (other.tag == "Enemy") {
			//Debug.Log ("new enemy detected");
			enemyList.Add (other.gameObject);
		}
	}

	void OnTriggerExit2D(Collider2D other){
		if (other.tag == "Enemy") {
			enemyList.Remove (other.gameObject);
		}
	}

}

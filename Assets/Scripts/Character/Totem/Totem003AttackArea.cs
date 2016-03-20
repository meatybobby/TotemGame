using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Totem003AttackArea : MonoBehaviour {

	public float attackIntv = 2.0f;
	public GameObject waveEffect;
	public Totem003 totem;

	private List<GameObject> enemyList;
	private bool attackFlag;
	private GameObject wave;
	
	void Start () {
		totem = GetComponentInParent<Totem003> ();
		enemyList = new List<GameObject> ();
		attackFlag = false;
	}
		
	void Update () {
		// remomve the enemies that have been destroyed
		enemyList.RemoveAll (item => item == null);

		if(enemyList.Count != 0 && attackFlag == false && !totem.isDead){
			attackFlag = true;
			//attack animate and substract enemy's hp
			totem.anim.Play("Totem003_attack");
			StartCoroutine (CreateWave());
		}
	}
	protected IEnumerator CreateWave() {

		wave = Instantiate(waveEffect,new Vector3(transform.position.x,transform.position.y+0.12f,transform.position.z-2f),transform.rotation) as GameObject;
		foreach (GameObject enemy in enemyList) {
			Character c = enemy.GetComponentInParent<Character>();
			if (c != null)
				c.CauseDamage (totem.damage);
		}

		yield return new WaitForSeconds (attackIntv);
		attackFlag = false;
	}

	public void RemoveWave(){
		Destroy (wave);
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "Enemy" || other.tag=="Rock") {
			enemyList.Add (other.gameObject);
		}
	}

	void OnTriggerExit2D(Collider2D other){
		if (other.tag == "Enemy" || other.tag=="Rock") {
			enemyList.Remove (other.gameObject);
		}
	}

}

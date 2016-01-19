using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Enemy004 : Enemy {
	public GameObject attackTail;
	public GameObject bullet;
	public float timer;
	private int attackRow;
	private int attackMode;
	public int waitTime;
	public float warningTime;
	private Animator anim;
	private Animator anitor;
	public GameObject shotSpawn;
	private int[] leftAttack = new int[]{6,4,3,3,3,2,2,1,0,0};
	private int[] rightAttack = new int[]{5,7,8,8,9,9,10,10,11,11};

	void Start () {
		timer = 0;
		shapeVector = new List<IntVector2> ();
		Vector3 orgin = transform.position;
		transform.position = new Vector3(orgin.x + offset.x, orgin.y + offset.y, orgin.z);
		anim = attackTail.GetComponent<Animator> ();
		anitor = GetComponent<Animator> ();
		Initialize ();
		isAttack = false;
	}
	
	
	void Update () {
		timer += Time.deltaTime;
		if (HP <= 0 && !isDead) {
			anitor.SetTrigger("die");
			Die ();
		}
		if (timer > waitTime) {
			waitTime = -1;
		}
		if (timer >= attackIntv && timer >= waitTime && !isAttack) {
			isAttack = true;
			attackMode = Random.Range(0,4);
			Debug.Log(attackMode);
			if(attackMode == 3) {
				anitor.SetTrigger ("fire");
				Invoke ("shotbullet", 1.7f);
			}
			else warning();
			timer = 0;
		}
	}

	void warning() {
		if (attackMode == 0) {
			attackRow = Random.Range (-2, 2) + player.pos.x;
			if (attackRow < 1)
				attackRow = 1;
			else if (attackRow > Map.MAP_WIDTH)
				attackRow = Map.MAP_WIDTH;
			for (int i = 1; i<=Map.MAP_HEIGHT; i++) {
				Map.warningArea [attackRow, i].SetActive (true);
			}
		} else if (attackMode == 1) {
			for (int i = 1; i <= Map.MAP_WIDTH; i++)
				for (int j = 1; j <= leftAttack[i-1]; j++)
					Map.warningArea [j, i].SetActive (true);
		} else if (attackMode == 2) {
			for (int i = 1; i <= Map.MAP_WIDTH; i++)
				for (int j = rightAttack[i-1]; j <= Map.MAP_HEIGHT; j++)
					Map.warningArea [j, i].SetActive (true);
		}
		Invoke ("attack", warningTime);
	}

	void attack() {
		if (attackMode == 0) {
			for (int i = 1; i<= Map.MAP_HEIGHT; i++) {
				Map.warningArea [attackRow, i].SetActive (false);
			}
			Vector3 realpos = Map.GetRealPosition (new IntVector2 (attackRow, 0), typeof(Enemy));
			Vector3 originPos = attackTail.transform.position;
			attackTail.transform.position = new Vector3 (realpos.x, -4.623681f, originPos.z);
			anim.SetTrigger ("attack1");
		} else if (attackMode == 1) {
			for(int i = 1; i <= Map.MAP_WIDTH; i++)
				for(int j = 1; j <= leftAttack[i-1] ; j++)
					Map.warningArea [j, i].SetActive (false);
			Vector3 originPos = attackTail.transform.position;
			attackTail.transform.position = new Vector3 (-6f, -21f, originPos.z);
			anim.SetTrigger ("attack2");
		} else if(attackMode == 2) {
			for(int i = 1; i <= Map.MAP_WIDTH; i++)
				for(int j = rightAttack[i-1]; j <= Map.MAP_HEIGHT ; j++)
							Map.warningArea [j, i].SetActive (false);
			Vector3 originPos = attackTail.transform.position;
			attackTail.transform.position = new Vector3 (6.75f, -23.3f, originPos.z);
			anim.SetTrigger ("attack2_right");
		}
		isAttack = false;
	}

	void shotbullet() {
		Instantiate(bullet, shotSpawn.transform.position, shotSpawn.transform.rotation);
		Quaternion q = Quaternion.Euler (0, 0, 120);
		Instantiate(bullet, shotSpawn.transform.position, q);
		q = Quaternion.Euler (0, 0, 240);
		Instantiate(bullet, shotSpawn.transform.position, q);
		isAttack = false;
	}

	public void Die() {
		isDead = true;
		Destroy (healthPanel);
		Destroy (this);
		Map.Destroy(this);
		for(int i = 0; i < manaDrop; i++) {
			float rx = Random.Range (-0.5f,0.5f),ry = Random.Range (-0.5f,0.5f);
			Instantiate (soul, transform.position+new Vector3(rx,ry,0), Quaternion.Euler (0f, 0f, 0f));
		}
		Destroy (GetComponent<Collider2D>());
		Destroy(gameObject, 2.8f);
	}
}

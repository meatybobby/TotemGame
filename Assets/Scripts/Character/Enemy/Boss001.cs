using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Boss001: Enemy {
	public GameObject attackTail;
	public float timer;
	private int attackRow;
	public int waitTime;
	public float warningTime;
	public Animator anim;

	void Start () {
		timer = 0;
		shapeVector = new List<IntVector2> ();
		Vector3 orgin = transform.position;
		transform.position = new Vector3(orgin.x + offset.x, orgin.y + offset.y, orgin.z);
		Initialize ();
	}
	
	
	void Update () {
		timer += Time.deltaTime;
		if (timer > waitTime) {
			waitTime = -1;
		}
		if (timer >= attackIntv && timer >= waitTime) {
			timer = 0;
			attackRow = Random.Range(-2, 2) + player.pos.x;
			if(attackRow < 1) attackRow = 1;
			else if(attackRow > Map.MAP_WIDTH) attackRow = Map.MAP_WIDTH;
			warning();
		}
	}

	void warning() {
		for (int i = 1; i<=Map.MAP_HEIGHT; i++) {
			Map.warningArea[attackRow,i].SetActive(true);
		}
		Invoke ("attack", warningTime);
	}

	void attack() {
		for (int i = 1; i<= Map.MAP_HEIGHT; i++) {
			Map.warningArea[attackRow,i].SetActive(false);
		}
		Vector3 realpos = Map.GetRealPosition (new IntVector2(attackRow,0), typeof(Enemy));
		Vector3 originPos = attackTail.transform.position;
		attackTail.transform.position = new Vector3 (realpos.x, originPos.y, originPos.z);
		anim.SetTrigger("attack");
	}
	
}

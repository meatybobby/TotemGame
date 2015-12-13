using UnityEngine;
using System.Collections;
using System.Collections.Generic;
// The mushroom which can only attack next to the target.
public class Enemy001 : Enemy {

	public GameObject hand;


	private Transform attackSpawn;
	private Enemy001Anim anim;


	
	void Start () {
		Initialize ();
		shapeVector = new List<IntVector2> ();
		shapeVector.Add (new IntVector2(0,0));
		attackSpawn = transform.FindChild ("Attack Spawn").gameObject.transform;
		anim = GetComponent<Enemy001Anim> ();
		isAttack = false;
	}


	void Update () {
		if (HP <= 0 && !isDead) {
			SetAnimation(DIE);
			Die ();
		}
		if (HP>0 && !inMoveThread) {
			if(mapUpdated == true)
			{
				FindDirection(player.pos);
			}			
			//Debug.Log(pace + "," + disMap[targetPos.x, targetPos.y] + "/" + guide[pace].x + "," + guide[pace].y);
			
			if (pace < disMap [targetPos.x, targetPos.y] - 1) {
				//Debug.Log("move like jagger");
				// Debug.Log("->"+guide[pace].x+" "+ guide[pace].y);
				if (guide [pace] != dir) {
					Rotate (guide [pace]);
				}
				MoveByVector (guide [pace]);
				pace++;
				SetAnimation(WALK);
			} else if (pace == disMap [targetPos.x, targetPos.y] - 1) {
				/*if(Map.IsEmpty(targetPos) || Map.Seek(targetPos)[0] is Enemy) {
					FindDirection(player.pos);
				}
				else */if(!isAttack) {
					Rotate (guide [pace]);
					isAttack = true;
					SetAnimation(ATTACK);
					StartCoroutine (BasicAttack ());
				}
			}
		}
	}
	protected IEnumerator BasicAttack(){
		GameObject obj = Instantiate(hand, attackSpawn.position, attackSpawn.rotation) as GameObject;
		yield return new WaitForSeconds(attackIntv);
		Destroy(obj);
		isAttack = false;
	}

	public void Rotate(IntVector2 a){
		dir = a;
		int angle;
		angle = a.x==0? (90*a.y):(90 - 90*a.x + 45*a.x*a.y);
		
		attackSpawn.rotation = Quaternion.Euler (0.0f, 0.0f, (float)angle+90.0f);
		
		SetAnimation (WALK);
	}
	
	public void SetAnimation(int mode) {
		// this will cause error because before start()
		if (anim != null) {
			anim.playAnim (dir, mode);
		}
	}
		
}

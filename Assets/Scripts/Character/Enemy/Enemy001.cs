using UnityEngine;
using System.Collections;
using System.Collections.Generic;
// The mushroom which can only attack next to the target.
public class Enemy001 : Enemy {

	public GameObject hand;


	private Transform attackSpawn;
	private Enemy001Anim anim;


	
	void Start () {
		attackPriority = new float[] {1,1,1,1,1};
		defaultPriority = new float[] {1,1,1,1,1};
		shapeVector = new List<IntVector2> ();
		shapeVector.Add (new IntVector2(0,0));
		attackSpawn = transform.FindChild ("Attack Spawn").gameObject.transform;
		anim = GetComponent<Enemy001Anim> ();
		isAttack = false;
		Initialize ();
	}


	void Update () {
		//Hp GUI
		HpUpdate ();
		if (HP <= 0 && !isDead) {
			SetAnimation(DIE);
			Die ();
		}
		if (HP>0 && !inMoveThread && player!=null) {
			if(mapUpdated == true)
			{
				FindDirection(player.pos);
			}
			//Debug.Log(pace + "," + disMap[targetPos.x,targetPos.y]);
			//Debug.Log(pace + "," + disMap[targetPos.x, targetPos.y] + "/" + guide[pace].x + "," + guide[pace].y);
			
			if (pace < disMap [targetPos.x, targetPos.y] - 2
			    || (pace == disMap [targetPos.x, targetPos.y]-2 && guide[pace]!=guide[pace+1])) {
				//Debug.Log("move like jagger");
				// Debug.Log("->"+guide[pace].x+" "+ guide[pace].y);
				if (guide [pace] != dir) {
					Rotate (guide [pace]);
				}
				MoveByVector (guide [pace]);
				pace++;
				SetAnimation(WALK);
			} else if ( pace == disMap [targetPos.x, targetPos.y]-1 
			           || (pace == disMap [targetPos.x, targetPos.y]-2 && guide[pace]==guide[pace+1]) || disMap[targetPos.x, targetPos.y] == -1) {
				/*if(Map.IsEmpty(targetPos) || Map.Seek(targetPos)[0] is Enemy) {
					FindDirection(player.pos);
				}
				else */
				//Debug.Log ("2 paces away!");
				if(!isAttack) {
					Rotate (guide [pace]);
					isAttack = true;
					SetAnimation(ATTACK);
					StartCoroutine (BasicAttack ());
					// two steps away: move while attacking
					if(pace == disMap [targetPos.x, targetPos.y]-2) {
						StartCoroutine(MoveToTarget(guide[pace]));
						pace++;
					}
				}
			}
		}
	}

	protected IEnumerator MoveToTarget(IntVector2 moveDir) {
		IntVector2 newPos; // the new pos after being moved
		newPos = pos + moveDir;      
		IntVector2 pre = new IntVector2(pos.x, pos.y);
		pos = newPos; 
		Map.UpdatePos (this, pre);
		Vector3 next = Map.GetRealPosition(newPos, this.GetType());

		// 往下走，z值先更新 （解決斜走重疊的問題）
		if (transform.position.y > next.y) {
			transform.position = new Vector3(transform.position.x, transform.position.y, next.z);
		}
		Vector3 nextXY = new Vector3 (next.x, next.y, transform.position.z); // 不要移動z
		inMoveThread = true;
		while (transform.position != nextXY) {
			//Debug.Log ("attack moving!");
			transform.position = Vector3.MoveTowards(transform.position, nextXY, 2*speed*Time.deltaTime);
			yield return null;
		}
		inMoveThread = false;
		transform.position = next;//new Vector3( next.x, next.y, next.z);
	}


	protected IEnumerator BasicAttack() {
		GameObject obj = Instantiate(hand, attackSpawn.position, attackSpawn.rotation) as GameObject;
		obj.GetComponent<Enemy001Attacker> ().enemy001 = this;
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

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
// the 1x1 enemy which can jump.
public class Enemy004 : Enemy {
	
	private bool isAttack;
	public float moveIntv;
	public float moveCnt;

	void Start () {
		Debug.Log ("enemy004 start!");
		player = GameObject.FindWithTag ("Player").GetComponent<Player> ();

		disMap = new int[Map.MAP_WIDTH + 2, Map.MAP_HEIGHT + 2];
		mapUpdated = true;
		isAttack = false;
		pace = 0;
		moveCnt = 0f;
		shapeVector = new List<IntVector2> ();
		shapeVector.Add (new IntVector2(0,0));
		attackPriority = new float[] {1,1,1};
		Map.Create(this);
		Rotate(dir);
	}

	void Update () {
		if (HP>0 && !inMoveThread) {
			if(mapUpdated == true)
			{
				FindDirection(player.pos);
			}			
			//Debug.Log(pace + "," + disMap[targetPos.x, targetPos.y] + "/" + guide[pace].x + "," + guide[pace].y);
			
			if (pace < disMap [targetPos.x, targetPos.y] - 1) {
				if(moveCnt < moveIntv) {
					moveCnt += Time.deltaTime;
					if(moveCnt == 0f) SetAnimation(IDLE);
				}
				else {
					moveCnt = 0f;
					if (guide [pace] != dir) {
						Rotate (guide [pace]);
					}
					MoveByVector (guide [pace]);
					pace++;
				}

			} else if (pace == disMap [targetPos.x, targetPos.y] - 1) {
				/*if(Map.IsEmpty(targetPos) || Map.Seek(targetPos)[0] is Enemy) {
					FindDirection(player.pos);
				}
				else */if(!isAttack) {
					Rotate (guide [pace]);
					isAttack = true;
					SetAnimation(ATTACK);
					//StartCoroutine (BasicAttack ());
				}
			}
		}
	}
	/*protected IEnumerator BasicAttack(){
		GameObject obj = Instantiate(hand, attackSpawn.position, attackSpawn.rotation) as GameObject;
		yield return new WaitForSeconds(attackIntv);
		Destroy(obj);
		isAttack = false;
	}*/
	
	public void Rotate(IntVector2 a){
		dir = a;
		int angle;
		angle = a.x==0? (90*a.y):(90 - 90*a.x + 45*a.x*a.y);
		
		//attackSpawn.rotation = Quaternion.Euler (0.0f, 0.0f, (float)angle+90.0f);
		
		SetAnimation (WALK);
	}
	
	public void SetAnimation(int mode) {
		// this will cause error because before start()
		/*if (anim != null) {
			anim.playAnim (dir, mode);
		}*/
	}
	
	void OnTriggerEnter2D(Collider2D other) {
		// Destroy everything that leaves the trigger
		if (other.tag == "bullet") {
			HP--;
			if(HP==0) {
				SetAnimation(DIE);
				Destroy (GetComponent<CircleCollider2D>());
				Destroy(gameObject, 1f);
				Map.Destroy(this);
			}
		}
	}

}

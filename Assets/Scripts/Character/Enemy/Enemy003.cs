using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy003 : Enemy {
	public IntVector2 lower, upper;
	public float moveWait;
	//private List<IntVector2> dir;
	//private int jumpingStep;
	
	private Enemy003Anim anim;
	public GameObject energyWave;
	public List<Character> attackTargets = new List<Character>();
	
	void Start () {
		//isAttack = false;
		anim = GetComponent<Enemy003Anim> ();
		
		attackPriority = new float[] {1,1,1,1,1};
		defaultPriority = new float[] {1,1,1,1,1};
		
		shapeVector = new List<IntVector2> ();
		shapeVector.Add (new IntVector2(0,0));
		shapeVector.Add (new IntVector2(0,1));
		shapeVector.Add (new IntVector2(1,0));
		shapeVector.Add (new IntVector2(1,1));
		
		lower = pos;
		upper = new IntVector2 (lower.x + 1, lower.y + 1);
		//jumpingStep = 100;
		//SetAnimation (IDLE);
		Initialize ();
	}
	
	// Update is called once per frame
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
				lower = pos;
				upper.x = lower.x + 1;
				upper.y = lower.y + 1;
				FindDirection(player.pos);
			}
			//Debug.Log(pace + "," + disMap[targetPos.x, targetPos.y] + "/" + guide[pace].x + "," + guide[pace].y);
			
			if (pace < disMap [targetPos.x, targetPos.y] && !isAttack) {
				//Debug.Log("move like jagger");
				// Debug.Log("->"+guide[pace].x+" "+ guide[pace].y);
				// Check if it can jump more than one step
				if (guide [pace] != dir || !isMoving) {
					isMoving = true;
					Rotate (guide [pace]);
				}
				/*List<IntVector2> movingList = new List<IntVector2>();
				for(int i = 0; i < jumpingStep; i++) {
					if(guide [pace] != dir || pace == disMap [targetPos.x, targetPos.y] - 1)
						break;
					movingList.Add(guide[pace]);
					pace++;
				}
				Debug.Log(movingList.Count);
				StartCoroutine(MoveByVectorArray(movingList,speed*movingList.Count*2));*/
				MoveByVector(guide[pace]);
				pace++;
				//SetAnimation(WALK);
				
			} else if (pace == disMap [targetPos.x, targetPos.y]) {
				/*if(Map.IsEmpty(targetPos) || Map.Seek(targetPos)[0] is Enemy) {
					FindDirection(player.pos);
				}
				else if(!isAttack) {
					Rotate (guide [pace]);
					StartCoroutine (BasicAttack ());
				}*/
				isMoving = false;
				if(!isAttack) {
					isAttack = true;
					//Debug.Log (guide[pace-1]);
					SetAnimation(ATTACK);
					StartCoroutine (JumpAttack());
				}
				
				
				/*if(!isAttack) {
					Rotate (guide [pace]);
					isAttack = true;
					SetAnimation(ATTACK);
				}*/
			}
		}
	}
	
	// Jump monster AI
	/*private void FindDirection(IntVector2 playerPos) {
		targetPos = playerPos;
		float minWeight = getDistance (player) * attackPriority [0];
		List<Character> totems = Map.FindAllType<Totem> () ;
		foreach (Character t in totems) {
			float weight = getDistance(t) * attackPriority[t.characterId];
			if(weight < minWeight) {
				minWeight = weight;
				targetPos = t.pos;
			}
		}
		int r;
		if (targetPos.x < lower.x || targetPos.x > upper.x) {
			if (targetPos.y < lower.y || targetPos.y > upper.y)
				r = Random.Range (0, 2);
			else
				r = 1;
		} else if (targetPos.y < lower.y || targetPos.y > upper.y)
			r = 0;
		else
			r = 2;
		Debug.Log ("R"+r);
		Debug.Log (targetPos.x + "," + targetPos.y + "/" + lower.x + "," + lower.y + "/" + upper.x + "," + upper.y);
		if (r == 1) {
			if (targetPos.x < lower.x) {
				if (lower.x - targetPos.x > 1)
					MoveByVector (new IntVector2 (-2, 0));
				else
					MoveByVector (new IntVector2 (-1, 0));
			} else {
				if (targetPos.x - upper.x > 1)
					MoveByVector (new IntVector2 (2, 0));
				else
					MoveByVector (new IntVector2 (1, 0));
			}
		} else if (r == 0) {
			if (targetPos.y < lower.y) {
				if (lower.y - targetPos.y > 1)
					MoveByVector (new IntVector2 (0, -2));
				else
					MoveByVector (new IntVector2 (0, -1));
			} else {
				if (targetPos.y - upper.y > 1)
					MoveByVector (new IntVector2 (0, 2));
				else
					MoveByVector (new IntVector2 (0, 1));
			}
		} else {
			while(true) {
				r = Random.Range(0,4);
				Debug.Log("Ran" + r);
				IntVector2 newPos = new IntVector2(lower.x+2*dir[r].x,lower.y+2*dir[r].y);
				if(Map.isInBounded(newPos)) {
					newPos = new IntVector2(upper.x+2*dir[r].x,upper.y+2*dir[r].y);
					if(Map.isInBounded(newPos)) break;
				}
			}
			MoveByVector(new IntVector2(2*dir[r].x,2*dir[r].y));
		}
	}*/
	
	public IEnumerator JumpAttack() {
		yield return new WaitForSeconds(attackIntv);
		if (isDead)
			yield break;
		foreach (Character c in attackTargets) {
			if (c != null )
				c.CauseDamage (damage);
		}
		ShakeTheGround ();
		GameObject energy = Instantiate (energyWave, transform.position, transform.rotation) as GameObject;
		yield return new WaitForSeconds (2f);
		Destroy (energy);
		isAttack = false;
	}
	public void Rotate(IntVector2 a) {
		dir = a;
		SetAnimation (WALK);
	}
	public void SetAnimation(int mode) {
		// this will cause error because before start()
		if (anim != null) {
			anim.playAnim (dir, mode);
		}
	}
	
	public void FindDirection(IntVector2 playerPos) {
		disMap = PathFinder.ShortestPath(lower,upper);
		IntVector2 tempPos = playerPos;
		float minWeight = getDistance (player) * attackPriority [0];
		List<Character> totems = Map.FindAllType<Totem> () ;
		foreach (Character t in totems) {
			float weight = getDistance(t) * attackPriority[t.characterId];
			if(weight < minWeight) {
				minWeight = weight;
				tempPos = t.pos;
			}
		}
		targetPos = PathFinder.RetrievePlayer (tempPos, disMap,2,2);
		guide = PathFinder.TracePathNoRandom(targetPos, disMap);
		pace = 0;
		mapUpdated = false;
	}
	
	/*public IEnumerator MoveByVectorArray(List<IntVector2> vecList, float newSpeed) {
		float tempSpeed = speed;
		speed = newSpeed;
		foreach (IntVector2 vec in vecList) {
			// rotate the character when moving toward different direction
			if(vec != dir) {
				Rotate (vec);
			}
			MoveByVector(vec);
			while (inMoveThread)
				yield return new WaitForSeconds (0.01f);
		}
		speed = tempSpeed;
		inMoveThread = true;
		SetAnimation (WALK);
		yield return new WaitForSeconds (moveWait);
		inMoveThread = false;
	}*/
	
	
	public void MoveByVector(IntVector2 offset) {
		
		IntVector2 newPos; // the new pos after being moved
		newPos = Map.BoundPos(pos+offset);                
		
		//if (Map.mainMap [newPos.x, newPos.y].Count != 0)
		//return;
		
		// Update the main-map position first
		IntVector2 pre = new IntVector2(pos.x, pos.y);
		pos = newPos; 
		Map.UpdatePos (this, pre);
		Vector3 next = Map.GetRealPosition(newPos, this.GetType(), this.offset);
		StartCoroutine(MoveThread (next));
	}
	
	private void ShakeTheGround() {
		Camera.main.GetComponent<EZCameraShake.CameraShaker> ().ShakeOnce (1.5f, 7.0f, 0.5f, 2.0f);
	}
	
}

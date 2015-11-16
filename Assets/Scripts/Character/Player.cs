using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : Character {

	public const int IDLE = 0;
	public const int CATCH = 1;
	public int mode;

	//public Transform totemSpawn;
	public GameObject totem;
	public Totem caughtTotem;
	private IntVector2 oldDir;

	public int totemNum = 0;
	public int maxTotemNum;

	private Animator anim;
	IntVector2 LEFT = new IntVector2 (-1,0);
	IntVector2 RIGHT = new IntVector2 (1,0);
	IntVector2 UP = new IntVector2 (0,1);
	IntVector2 DOWN = new IntVector2 (0,-1);

	void Start () {
		mode = IDLE;
		//Map.mainMap [1, 1].Add (this);
		Map.Create (this);
		oldDir = dir;
		anim = GetComponent<Animator>();
		Rotate (dir);
	}

	void Update () {
		float moveH = Input.GetAxis ("Horizontal");
		float moveV = Input.GetAxis ("Vertical");


		if (!(Mathf.Abs (moveH) == 0.0f && Mathf.Abs (moveV) == 0.0f) && !inMoveThread) {



			int unitH = moveH == 0.0f ? 0 : (moveH > 0.0f ? 1 : -1);
			int unitV = moveV == 0.0f ? 0 : (moveV > 0.0f ? 1 : -1);

			IntVector2 movement = new IntVector2 (unitH, unitV);

			if ((movement != dir && mode != CATCH) ) {
				Rotate (movement);
			} else if ( movement==oldDir || (Mathf.Abs (moveH) >= 0.5f || Mathf.Abs (moveV) >= 0.5f) ) {
				       // If move toward the same direction or key hold for long enough
					   // move the player

				if (mode == CATCH) {
					// Slant move is not allowed when CATCH
					if (Mathf.Abs (movement.x + movement.y) == 1) {
						MoveByVector (movement);
					}
				} else {
					if(!isMoving) {
						isMoving  = true;
						Rotate (movement);
					}
					MoveByVector (movement);
				}
			}
		} 
		else if( Mathf.Abs (moveH) == 0.0f && Mathf.Abs (moveV) == 0.0f ) {
			if(isMoving){
				isMoving = false;
				Rotate(dir);
			}
			oldDir = dir;
		}

		// Plant a totem when pressing left ctrl and not moving and not slant
		if (Input.GetKeyDown (KeyCode.LeftControl) && !isMoving &&
		     ( Mathf.Abs(dir.x+dir.y)==1) && totemNum < maxTotemNum) {

			IntVector2 plantPos = Map.BoundPos(pos+dir);
			// If the grid is empty
			if(Map.IsEmpty(plantPos)) {
				Vector3 totemRealPos = Map.GetRealPosition(pos+dir);
				GameObject totemObj = Instantiate (totem, totemRealPos, Quaternion.Euler(0f,0f,0f)) as GameObject;
				Totem newTotem = totemObj.GetComponent<Totem>();
				newTotem.Rotate(dir);
				newTotem.pos = pos+dir;
				newTotem.isCaught = false;
				newTotem.playerRef = this;
				Map.Create(newTotem);
				totemNum++;
			}
		}

		// Catch or release the totem when pressing Space key
		if (Input.GetKeyDown (KeyCode.Space) && !isMoving) {

			IntVector2 actionPos = pos+dir;
			switch(mode) {
			case IDLE:
				List<Character> charList = Map.Seek(actionPos);
				foreach(Character c in charList) {
					// If there's a totem in front of Player
					if(c.GetType() == typeof(Totem)) {
						caughtTotem = (Totem)c;
						caughtTotem.transform.parent = transform;
						caughtTotem.isCaught = true;
						mode = CATCH;
						break;
					}
				}
				break;

			case CATCH:
				caughtTotem.transform.parent = null;
				caughtTotem.isCaught = false;
				caughtTotem = null;
				mode = IDLE;
				break;
			}
		}

		// Testing the MoveByVectorArray function by pressing 'M'
		if (Input.GetKeyDown (KeyCode.M) && !isMoving && mode != CATCH) {
			List<IntVector2> vecList = new List<IntVector2>();
			// right, right, up, up, left
			vecList.Add(new IntVector2(1,0));
			vecList.Add(new IntVector2(1,0));
			vecList.Add(new IntVector2(0,1));
			vecList.Add(new IntVector2(0,1));
			vecList.Add(new IntVector2(-1,0));
			vecList.Add(new IntVector2(1,0));
			vecList.Add(new IntVector2(1,0));
			vecList.Add(new IntVector2(0,1));
			vecList.Add(new IntVector2(0,1));
			float newSpeed = 10.0f;
			StartCoroutine(MoveByVectorArray(vecList, newSpeed));
		}
	}

	public void MoveByVector(IntVector2 offset) {
		IntVector2 newPos = Map.BoundPos(pos+offset);
		if (mode == CATCH) {

			IntVector2 totemNewPos = Map.BoundPos (pos + dir + offset);
			if (offset == dir) { // forward
				if ( !Map.IsEmpty(totemNewPos) ) {
					return;
				}
			} else if (offset == -dir) { // backward
				if ( !Map.IsEmpty(newPos) )
					return;
			} else { // 平移
				if ( !Map.IsEmpty(totemNewPos) || !Map.IsEmpty(newPos) )
					return;
			}
			// Update the pos of the caught totem
			IntVector2 totemPre = new IntVector2(caughtTotem.pos.x, caughtTotem.pos.y);
			caughtTotem.pos = totemNewPos; 
			Map.UpdatePos (caughtTotem, totemPre);
		
		} else if (!Map.IsEmpty(newPos)) {
				return;
		}

		base.MoveByVector (offset);
	}

	public void Rotate(IntVector2 a) {

		if (isMoving) {
			if(a == LEFT) {
				anim.SetTrigger ("left_run");
			} else if(a == RIGHT) {
				anim.SetTrigger ("right_run");
			} else if(a == DOWN) {
				anim.SetTrigger ("front_run");
			} else if(a == UP){
				anim.SetTrigger ("back_run");
			}

		} else {
			if (a == LEFT) {
				anim.SetTrigger ("left_idle");
			} else if (a == RIGHT) {
				anim.SetTrigger ("right_idle");
			} else if (a == UP) {
				anim.SetTrigger ("back_idle");
			} else if (a == DOWN) {
				anim.SetTrigger ("front_idle");
			}
		}
		dir = a;

	}

	void OnTriggerEnter2D(Collider2D other) {
		// Destroy everything that leaves the trigger
		if (other.tag == "MonsterHand") {
			Destroy(other.gameObject);
			Debug.Log ("Player attacked by monster!");
			HP--;
			if(HP<=0) {
				Debug.Log("Destroy!!");
				Destroy(gameObject);
				Map.Destroy(this);
			}
		}
		
	}

}
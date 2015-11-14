using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : Character {

	public const int IDLE = 0;
	public const int CATCH = 1;
	public int mode;

	public Transform totemSpawn;
	public GameObject totem;
	public Totem caughtTotem;

	public IntVector2 oldDir;

	void Start () {
		mode = IDLE;
		//Map.mainMap [1, 1].Add (this);
		Map.Create (this);
		Debug.Log (Time.deltaTime);
		oldDir = dir;
	}

	void Update () {
		float moveH = Input.GetAxis ("Horizontal");
		float moveV = Input.GetAxis ("Vertical");

		if (!(Mathf.Abs (moveH) == 0.0f && Mathf.Abs (moveV) == 0.0f) && !isMoving) {

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
					if (Mathf.Abs (movement.x + movement.y) == 1)
						MoveByVector (movement);
				} else {
					MoveByVector (movement);
				}
			}
		} 
		else if( Mathf.Abs (moveH) == 0.0f && Mathf.Abs (moveV) == 0.0f ) {
			oldDir = dir;
		}

		// Plant a totem when pressing left ctrl and not moving and not slant
		if (Input.GetKeyDown (KeyCode.LeftControl) && !isMoving && ( Mathf.Abs(dir.x+dir.y)==1)) {

			IntVector2 plantPos = Map.BoundPos(pos+dir);
			// If the grid is empty
			if(Map.IsEmpty(plantPos)) {
				GameObject totemObj = Instantiate (totem, totemSpawn.position, Quaternion.Euler(0f,0f,0f)) as GameObject;
				Totem newTotem = totemObj.GetComponent<Totem>();
				newTotem.Rotate(dir);
				newTotem.pos = pos+dir;
				//newTotem.dir = new IntVector2(0,-1);
				newTotem.isCaught = false;
				newTotem.speed = speed;
				newTotem.playerRef = this;
				Map.Create(newTotem);
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
		if (Input.GetKeyDown (KeyCode.M) && !isMoving) {
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
	                                            

}
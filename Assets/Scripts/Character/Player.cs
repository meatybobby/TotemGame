using UnityEngine;
using System.Collections;

public class Player : Character {

	public const int IDLE = 0;
	public const int CATCH = 1;
	public int mode;

	public Transform totemSpawn;
	public GameObject totem;
	public Totem caughtTotem;
	

	void Start () {
		mode = IDLE;
		Map.mainMap [1, 1].Add (this);
	}

	void Update () {
		float moveH = Input.GetAxis ("Horizontal");
		float moveV = Input.GetAxis ("Vertical");

		if ( !(Mathf.Abs(moveH) == 0.0f && Mathf.Abs(moveV) == 0.0f) && !isMoving) {

			int unitH = moveH==0.0f? 0 : (moveH>0.0f? 1 : -1);
			int unitV = moveV==0.0f? 0 : (moveV>0.0f? 1 : -1);

			IntVector2 movement = new IntVector2 (unitH, unitV);

			if(movement!=dir && mode!=CATCH) {
				Rotate (movement);
			}
			else {

				if(mode == CATCH) {
					// Slant move is not allowed when CATCH
					if( Mathf.Abs(movement.x+movement.y)==1 )
						MoveByVector(movement);
				} else {
					MoveByVector(movement);
				}
			}
		}

		// Plant a totem when pressing left ctrl and not moving and not slant
		if (Input.GetKeyDown (KeyCode.LeftControl) && !isMoving && ( Mathf.Abs(dir.x+dir.y)==1)) {

			IntVector2 plantPos = Map.BoundPos(pos+dir);
			// If the grid is empty
			if(Map.mainMap [plantPos.x, plantPos.y].Count==0) {
				GameObject totemObj = Instantiate (totem, totemSpawn.position, Quaternion.Euler(0f,0f,0f)) as GameObject;
				Totem newTotem = totemObj.GetComponent<Totem>();
				newTotem.pos = pos+dir;
				newTotem.dir = new IntVector2(0,-1);
				newTotem.isCaught = false;
				newTotem.playerRef = this;
				Map.mainMap [pos.x+dir.x, pos.y+dir.y].Add (newTotem);
			}
		}

		// Catch or release the totem when pressing Space key
		if (Input.GetKeyDown (KeyCode.Space) && !isMoving) {

			IntVector2 actionPos = pos+dir;
			switch(mode) {
			case IDLE:
				foreach(Character c in Map.mainMap[actionPos.x, actionPos.y]) {
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
	}

	public void MoveByVector(IntVector2 offset) {
		IntVector2 newPos = Map.BoundPos(pos+offset);
		if (mode == CATCH) {

			IntVector2 totemNewPos = Map.BoundPos (pos + dir + offset);
			if (offset == dir) { // forward
				if (Map.mainMap [totemNewPos.x, totemNewPos.y].Count != 0) {
					return;
				}
			} else if (offset == -dir) { // backward
				if (Map.mainMap [newPos.x, newPos.y].Count != 0)
					return;
			} else { // 平移
				if (Map.mainMap [totemNewPos.x, totemNewPos.y].Count != 0 || 
					Map.mainMap [newPos.x, newPos.y].Count != 0)
					return;
			}
			// Update the pos of the caught totem
			IntVector2 totemPre = new IntVector2(caughtTotem.pos.x, caughtTotem.pos.y);
			caughtTotem.pos = totemNewPos; 
			Map.UpdatePos (caughtTotem, totemPre);
		
		} else if (Map.mainMap [newPos.x, newPos.y].Count != 0) {
				return;
		}

		base.MoveByVector (offset);
	}
	                                            

}
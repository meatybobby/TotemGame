using UnityEngine;
using System.Collections;

public class Player : Character {

	public const int IDLE = 0;
	public const int CATCH = 1;
	public int mode;

	public Transform totemSpawn;
	public GameObject totem;
	
	void Start () {
		//Vector3 startPos = new Vector3 (Map.MAP_POS [1, 1].x, Map.MAP_POS [1, 1].y, 0);
		//transform.position = startPos;
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

			if(movement!=dir) {
				Rotate (movement);
			}
			else {
				//Debug.Log ( (pos+movement).x + " " + (pos+movement).y );
				StartCoroutine(MoveByVector(movement));
			}

		}

		// Plant a totem when press left ctrl and not moving and not slant
		if (Input.GetKeyDown (KeyCode.LeftControl) && !isMoving && ( Mathf.Abs(dir.x+dir.y)==1)) {

			IntVector2 plantPos = Map.BoundPos(pos+dir);

			if(Map.mainMap [plantPos.x, plantPos.y].Count==0){
				GameObject totemObj = Instantiate (totem, totemSpawn.position, Quaternion.Euler(0f,0f,0f)) as GameObject;
				Totem newTotem = totemObj.GetComponent<Totem>();
				newTotem.pos = pos+dir;
				newTotem.dir = new IntVector2(0,-1);
				Map.mainMap [pos.x+dir.x, pos.y+dir.y].Add (newTotem);
			}
		}
	}

	void FixedUpdate() {

	}



}

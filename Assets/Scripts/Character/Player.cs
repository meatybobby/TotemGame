using UnityEngine;
using System.Collections;

public class Player : Character {

	public int mode;

	// Use this for initialization
	void Start () {
		Vector3 startPos = new Vector3 (Map.MAP_POS [1, 1].x, Map.MAP_POS [1, 1].y, 0);
		transform.position = startPos;
		pos = new Vector2 (1, 1);
		dir = new Vector2 (1, 0);

		Debug.Log ("x: " + transform.position.x);
		Debug.Log ("y: " + transform.position.y);
	}
	
	// Update is called once per frame
	void Update () {
		float moveH = Input.GetAxis ("Horizontal");
		float moveV = Input.GetAxis ("Vertical");

		if ( !(moveH==0.0f && moveV==0.0f) && !isMoving) {
			float unitH = moveH==0.0f? 0.0f : (moveH>0.0f? 1.0f : -1.0f);
			float unitV = moveV==0.0f? 0.0f : (moveV>0.0f? 1.0f : -1.0f);

			Vector2 movement = new Vector2 (unitH, unitV);
			Debug.Log ( "movement: " + movement +" h:"+moveH+" v: "+moveV);

			StartCoroutine(MoveByVector(movement));
		}
	}

	void FixedUpdate() {

	}
	


}

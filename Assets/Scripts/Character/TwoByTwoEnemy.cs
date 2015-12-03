using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TwoByTwoEnemy : Enemy {
	public IntVector2 lower, upper;

    // Use this for initialization
    void Start () {
        Debug.Log("enemy start!");
        mapUpdated = true;
        isAttack = false;
        pace = 0;
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        disMap = new int[Map.MAP_WIDTH + 2, Map.MAP_HEIGHT + 2];
		shapeVector = new List<IntVector2> ();
		shapeVector.Add (new IntVector2(0,0));
		shapeVector.Add (new IntVector2(0,1));
		shapeVector.Add (new IntVector2(1,0));
		shapeVector.Add (new IntVector2(1,1));
        Map.Create(this);
		lower = pos;
		upper = new IntVector2 (lower.x + 1, lower.y + 1);
    }
	// Update is called once per frame
	void Update() {
		lower = pos;
		upper.x = lower.x + 1;
		upper.y = lower.y + 1;
		if(mapUpdated == true)
		{
			FindDirection(player.pos);
		}
		if (!inMoveThread) {
			if (pace < disMap [targetPos.x, targetPos.y] - 1) {
				//Debug.Log("move like jagger");
				// Debug.Log("->"+guide[pace].x+" "+ guide[pace].y);
				if (guide [pace] != dir) {
					Rotate (guide [pace]);
				}
				MoveByVector (guide [pace]);
				pace++;
				isAttack = false;
			} else if (pace == disMap [targetPos.x, targetPos.y] - 1) {
<<<<<<< HEAD
				if(Map.IsEmpty(targetPos)) {
=======
				/*if(Map.IsEmpty(targetPos)) {
>>>>>>> origin/master
					FindDirection(player.pos);
				}
				else if(!isAttack) {
					Rotate (guide [pace]);
					isAttack = true;
					StartCoroutine (BasicAttack ());
<<<<<<< HEAD
				}
=======
				}*/
>>>>>>> origin/master
			}
		}
	}

	private void FindDirection(IntVector2 playerPos)
	{
		disMap = PathFinder.ShortestPathRect(lower,upper);
<<<<<<< HEAD
		if (getDistance (player) <= 25)
			targetPos = PathFinder.RetrievePlayer (playerPos, disMap);
		else {
=======
		//if (getDistance (player) <= 25)
			targetPos = PathFinder.RetrievePlayer (playerPos, disMap);
		/*else {
>>>>>>> origin/master
			Totem[] totems = FindObjectsOfType<Totem> ();
			int min = 26;
			foreach (Totem t in totems) {
				if (getDistance (t) < min) {
					min = getDistance (t);
					targetPos = PathFinder.RetrievePlayer (t.pos, disMap);
				}
			}
			if (min == 26) {
				int rx = Random.Range (-3, 3), ry = Random.Range (-3, 3);
				IntVector2 tempPos = Map.BoundPos(new IntVector2 (pos.x + rx, pos.y + ry));
				targetPos = PathFinder.RetrievePlayer (tempPos, disMap);
			}
		}
<<<<<<< HEAD
		Debug.Log (targetPos.x + "." + targetPos.y);
		guide = PathFinder.TracePath(targetPos, disMap);
		mapUpdated = false; 
		pace = 0;
	}

=======
		Debug.Log (targetPos.x + "." + targetPos.y);*/
		guide = PathFinder.TracePath(targetPos, disMap);
		mapUpdated = false;
		pace = 0;
		Debug.Log (targetPos.x + "," + targetPos.y + "(" + guide [pace].x + "," + guide [pace].y + ")");
	}
>>>>>>> origin/master
}

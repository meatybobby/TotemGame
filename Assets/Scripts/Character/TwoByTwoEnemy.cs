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
		lower = new IntVector2 (0, 0);
		upper = new IntVector2 (1, 1);
        Map.Create(this);
        //disMap = PathFinder.ShortestPath(pos, frontVector);
    }

	private void FindDirection(IntVector2 playerPos)
	{
		disMap = PathFinder.ShortestPathRect (lower, upper);
		targetPos = PathFinder.RetrievePlayer (playerPos, disMap);
		guide = PathFinder.TracePath(targetPos, disMap);
		mapUpdated = false; 
	}
}

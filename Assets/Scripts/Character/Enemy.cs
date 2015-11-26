using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : Character {
	
	public int[,] disMap;// = new int[Map.MAP_WIDTH + 2, Map.MAP_HEIGHT + 2];
	public bool mapUpdated;
	//public GameObject attackTrigger;
	public float attackSpeed;
	public bool isAttack;
	public List<IntVector2> shapeVector;
	public Player player;
	protected IntVector2[] guide = new IntVector2[(Map.MAP_WIDTH+2) * (Map.MAP_HEIGHT+2)];
	// The step number of enemy
    protected int pace;
	public IntVector2 targetPos;
	public GameObject hand;
	public float attackIntv;

	// Use this for initialization
	void Start () {
		Debug.Log ("enemy start!");
		mapUpdated = true;
		isAttack = false;
		pace = 0;
        player = GameObject.FindWithTag ("Player").GetComponent<Player> ();
		disMap = new int[Map.MAP_WIDTH + 2, Map.MAP_HEIGHT + 2];
		shapeVector = new List<IntVector2> ();
		shapeVector.Add (new IntVector2(0,0));
		Map.Create(this);
		Rotate(dir);
	}
	
	// Update is called once per frame
	void Update() {
		if (!inMoveThread) {
			if(mapUpdated == true)
			{
				FindDirection(player.pos);
			}
			Debug.Log(pace + "," + disMap[targetPos.x, targetPos.y] + "/" + guide[pace].x + "," + guide[pace].y);
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
				/*if(Map.IsEmpty(targetPos) || Map.Seek(targetPos)[0] is Enemy) {
					FindDirection(player.pos);
				}
				else */if(!isAttack) {
					Rotate (guide [pace]);
					isAttack = true;
					StartCoroutine (BasicAttack ());
				}
			}
		}
	}

	public void FindDirection(IntVector2 playerPos)
	{
		disMap = PathFinder.ShortestPath(pos);
		targetPos = null;
		//if (getDistance (player) <= 25)
			targetPos = PathFinder.RetrievePlayer (playerPos, disMap);
		/*else {
			Totem[] totems = FindObjectsOfType<Totem> ();
			int min = 26;
			foreach (Totem t in totems) {
				if (getDistance (t) < min) {
					min = getDistance (t);
					targetPos = PathFinder.RetrievePlayer (t.pos, disMap);
				}
			}
			if (targetPos == null) {
				int rx = Random.Range (-3, 3), ry = Random.Range (-3, 3);
				IntVector2 tempPos = Map.BoundPos (new IntVector2 (pos.x + rx, pos.y + ry));
				targetPos = PathFinder.RetrievePlayer (tempPos, disMap);
			}
		}*/
		guide = PathFinder.TracePath(targetPos, disMap);
		pace = 0;
		mapUpdated = false;
	}

	protected IEnumerator BasicAttack()
	{
		while(true) {
			GameObject obj = Instantiate(hand, transform.position, transform.rotation) as GameObject;
			yield return new WaitForSeconds(attackIntv);
			if(!isAttack) {
				Destroy(obj);
				yield break;
			}
		}


		/*Vector3 attackVector =   new Vector3(Map.MAP_POS[pos.x + dir.x, pos.y + dir.y].x, Map.MAP_POS[pos.x + dir.x, pos.y + dir.y].y,0);
		while (attackTrigger.transform.position != attackVector)
		{
			attackTrigger.transform.position = Vector3.MoveTowards(attackTrigger.transform.position, attackVector, attackSpeed * Time.deltaTime);
			yield return null;
		}
		attackTrigger.transform.position = transform.position;
		/*while (attackTrigger.transform.position != transform.position)
		{
			attackTrigger.transform.position = Vector3.MoveTowards(attackTrigger.transform.position, transform.position, attackSpeed * Time.deltaTime);
			yield return null;
		}
		isAttack = false;*/
	}
	
}


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : Character {
	
	public int[,] disMap;// = new int[Map.MAP_WIDTH + 2, Map.MAP_HEIGHT + 2];
	public bool mapUpdated;

	public GameObject hand;

	public Transform attackSpawn;
	public float attackIntv;
	public bool isAttack;

	public List<IntVector2> shapeVector;
	public Player player;
	protected IntVector2[] guide = new IntVector2[(Map.MAP_WIDTH+2) * (Map.MAP_HEIGHT+2)];
	// The step number of enemy
    protected int pace;
	public IntVector2 targetPos;


	
	public Enemy001Anim anim;
	public const int WALK = 0;
	public const int ATTACK = 1;
	public const int DIE = 2;

	public Enemy(){
	}
	

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

		anim = GetComponent<Enemy001Anim> ();
	}
	
	// Update is called once per frame
	void Update() {

		if (HP>0 && !inMoveThread) {

			if(mapUpdated == true)
			{
				FindDirection(player.pos);
			}			
			//Debug.Log(pace + "," + disMap[targetPos.x, targetPos.y] + "/" + guide[pace].x + "," + guide[pace].y);

			if (pace < disMap [targetPos.x, targetPos.y] - 1) {
				//Debug.Log("move like jagger");
				// Debug.Log("->"+guide[pace].x+" "+ guide[pace].y);
				if (guide [pace] != dir) {
					Rotate (guide [pace]);
				}
				MoveByVector (guide [pace]);
				pace++;
				isAttack = false;
				SetAnimation(WALK);
			} else if (pace == disMap [targetPos.x, targetPos.y] - 1) {
				/*if(Map.IsEmpty(targetPos) || Map.Seek(targetPos)[0] is Enemy) {
					FindDirection(player.pos);
				}
				else */if(!isAttack) {
					Rotate (guide [pace]);
					isAttack = true;
					SetAnimation(ATTACK);
					StartCoroutine (BasicAttack ());
				}
			}
		}

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
			GameObject obj = Instantiate(hand, attackSpawn.position, attackSpawn.rotation) as GameObject;

			yield return new WaitForSeconds(attackIntv);
			if(!isAttack) {
				Destroy(obj);
				yield break;
			}
		}
	}

	public void Rotate(IntVector2 a){
		dir = a;
		int angle;
		angle = a.x==0? (90*a.y):(90 - 90*a.x + 45*a.x*a.y);
		
		attackSpawn.rotation = Quaternion.Euler (0.0f, 0.0f, (float)angle+90.0f);

		SetAnimation (WALK);
	}

	private void SetAnimation(int mode) {
		// this will cause error because before start()
		if (anim != null) {
			anim.playAnim (dir, mode);
		}
	}
	
}


using UnityEngine;
using System.Collections;

public class Enemy : Character {
	
	public int[,] disMap;// = new int[Map.MAP_WIDTH + 2, Map.MAP_HEIGHT + 2];
	public bool mapUpdated;
	public GameObject attackTrigger;
	public float attackSpeed;
	public bool isAttack;
	
	public Player player;
	private IntVector2[] guide = new IntVector2[(Map.MAP_WIDTH+2) * (Map.MAP_HEIGHT+2)];
	private int pace;
	public IntVector2 targetPos;
	
	// Use this for initialization
	void Start () {
		Debug.Log ("enemy start!");
		mapUpdated = true;
		isAttack = false;
		pace = 0;
		player = GameObject.FindWithTag ("Player").GetComponent<Player> ();
		disMap = new int[Map.MAP_WIDTH + 2, Map.MAP_HEIGHT + 2];
		Map.Create(this);
		disMap = FindPath.ShortestPath(pos);
	}
	
	// Update is called once per frame
	void Update() {
		if(mapUpdated == true)
		{
			FindDirection();
			pace = 0;
		}
		if (!isMoving && pace < disMap[targetPos.x , targetPos.y] -1)
		{
			//Debug.Log("move like jagger");
			// Debug.Log("->"+guide[pace].x+" "+ guide[pace].y);
			if(guide[pace]!=dir) {
				Rotate(guide[pace]);
			}
			MoveByVector(guide[pace]);
			pace++;
		}
		else if(!isMoving && pace == disMap[targetPos.x, targetPos.y] - 1 && !isAttack)
		{
			Rotate (guide[pace]);
			isAttack = true;
			StartCoroutine(BasicAttack());
		}
	}
	void OnTriggerEnter2D(Collider2D other) {
		// Destroy everything that leaves the trigger
		if (other.tag == "bullet") {
			Destroy(other.gameObject);
			HP--;
			if(HP<=0) {
				Destroy(gameObject);
				Map.Destroy(this);
			}
		}
		
	}
	private void FindDirection()
	{

		targetPos = new IntVector2(player.pos.x, player.pos.y);
		guide = FindPath.TracePath(targetPos, disMap);
		mapUpdated = false; 
	}
	private IEnumerator BasicAttack()
	{
		Vector3 attackVector = new Vector3(Map.MAP_POS[pos.x + dir.x, pos.y + dir.y].x, Map.MAP_POS[pos.x + dir.x, pos.y + dir.y].y,0);
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
		}*/
		isAttack = false;
	}
}


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : Character {
	public int[,] disMap;
	public bool mapUpdated;
	public List<IntVector2> shapeVector;

	public Vector2 offset = new Vector2 (0, 0);

	protected IntVector2[] guide = new IntVector2[(Map.MAP_WIDTH+2) * (Map.MAP_HEIGHT+2)];
	protected int pace;
	protected float[] attackPriority;
	public float[] defaultPriority;
	protected Player player;
	protected IntVector2 targetPos;

	public const int WALK = 0;
	public const int ATTACK = 1;
	public const int DIE = 2;
	public const int IDLE = 3;
	public float attackIntv;
	public bool isAttack;
	public GameObject angry;
	//public Transform angrySpawn;
	//public bool isAngry;
	public int angryNum;
	public GameObject soul;
	public int manaDrop = 3;

	public void Initialize() {
		player = GameObject.FindWithTag ("Player").GetComponent<Player> ();
		isMoving = false;
		mapUpdated = true;
		pace = 0;
		disMap = new int[Map.MAP_WIDTH + 2, Map.MAP_HEIGHT + 2];
		//isAngry = false;
		angryNum = 0;
		Map.Create(this);
	}

	public void Die() {
		isDead = true;
		Destroy (this);
		Map.Destroy(this);
		for(int i = 0; i < manaDrop; i++) {
			float rx = Random.Range (-0.5f,0.5f),ry = Random.Range (-0.5f,0.5f);
			Instantiate (soul, transform.position+new Vector3(rx,ry,0), Quaternion.Euler (0f, 0f, 0f));
		}
		Destroy (GetComponent<Collider2D>());
		Destroy(gameObject, 1.5f);
	}
	
	public void FindDirection(IntVector2 playerPos) {
		disMap = PathFinder.ShortestPath(pos);
		IntVector2 tempPos = playerPos;
		float minWeight = getDistance (player) * attackPriority [0];
		List<Character> totems = Map.FindAllType<Totem> () ;
		foreach (Character t in totems) {
			float weight = getDistance(t) * attackPriority[t.characterId];
			if(weight < minWeight) {
				minWeight = weight;
				tempPos = t.pos;
			}
		}
		targetPos = PathFinder.RetrievePlayer (tempPos, disMap);
		if (targetPos == null) {
			targetPos = playerPos;
			guide = new IntVector2[5];
			guide[0] = new IntVector2(playerPos.x - pos.x, playerPos.y - pos.y);
		}
		else guide = PathFinder.TracePath(targetPos, disMap);
		pace = 0;
		mapUpdated = false;
	}

	public float GetPriority(int id) {
		return attackPriority [id];
	}
	public void SetPriority(int id, float priority) {
		attackPriority [id] = priority;
	}

	public void GetAngry() {
		angryNum++;
		if (angryNum==1) {
			StartCoroutine (AngryAnimation());
		}
	}
	public void CancelAngry() {
		angryNum--;
	}

	protected IEnumerator AngryAnimation() {
		yield return new WaitForSeconds (1f);
		while (angryNum > 0) {
			angry.SetActive (true);
			yield return new WaitForSeconds (1.5f);
			angry.SetActive (false);
		}
	}

}


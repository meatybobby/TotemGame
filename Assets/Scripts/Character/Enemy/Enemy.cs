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
	protected Player player;
	protected IntVector2 targetPos;

	public const int WALK = 0;
	public const int ATTACK = 1;
	public const int DIE = 2;
	public const int IDLE = 3;
	public float attackIntv;
	public bool isAttack;

	public void Initialize() {
		player = GameObject.FindWithTag ("Player").GetComponent<Player> ();
		isMoving = false;
		mapUpdated = true;
		pace = 0;
		attackPriority = new float[] {1,1,1};
		disMap = new int[Map.MAP_WIDTH + 2, Map.MAP_HEIGHT + 2];
		Map.Create(this);
	}

	public void Die() {
		isDead = true;
		Destroy (GetComponent<Collider2D>());
		Destroy(gameObject, 2.5f);
		Map.Destroy(this);
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
		guide = PathFinder.TracePath(targetPos, disMap);
		pace = 0;
		mapUpdated = false;
	}

	public float GetPriority(int id) {
		return attackPriority [id];
	}
	public void SetPriority(int id, float priority) {
		attackPriority [id] = priority;
	}
}


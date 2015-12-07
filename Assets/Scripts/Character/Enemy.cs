using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : Character {
	public int[,] disMap;
	public bool mapUpdated;
	public List<IntVector2> shapeVector;
	
	protected IntVector2[] guide = new IntVector2[(Map.MAP_WIDTH+2) * (Map.MAP_HEIGHT+2)];
	protected int pace;
	protected float[] attackPriority;
	protected Player player;
	protected IntVector2 targetPos;
	
	public Enemy(){
	}
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update() {
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
}


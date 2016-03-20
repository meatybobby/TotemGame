using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/*public static class Boundary {
	public static float xMin = -3.3f;// = -1.875f;
	public static float xMax = 3.3f;// = 1.875f;
	public static float yMin = -3.3f;// = -1.875f;
	public static float yMax = 3.3f;// = 1.875f;
}*/

public static class Direction {
	public static IntVector2 LEFT = new IntVector2 (-1,0);
	public static IntVector2 RIGHT = new IntVector2 (1,0);
	public static IntVector2 UP = new IntVector2 (0,1);
	public static IntVector2 DOWN = new IntVector2 (0,-1);
	public static IntVector2 UP_LEFT = new IntVector2 (-1,1);
	public static IntVector2 UP_RIGHT = new IntVector2 (1,1);
	public static IntVector2 DOWN_LEFT = new IntVector2 (-1,-1);
	public static IntVector2 DOWN_RIGHT = new IntVector2 (1,-1);
}

public class Map {

	public static int MAP_WIDTH = 10;
	public static int MAP_HEIGHT = 10;
	//public static float MAP_SIZE_X = 1.5f;
	//public static float MAP_SIZE_Y = 1.5f;
	public static float unitCell = 0.75f;
	public static Vector3[,] MAP_POS = new Vector3[MAP_WIDTH+2, MAP_HEIGHT+2];
	private static List<Character>[,] mainMap = new List<Character>[MAP_WIDTH+2, MAP_HEIGHT+2];
	public static GameObject[,] warningArea = new GameObject[MAP_WIDTH+2, MAP_HEIGHT+2];

	static Map() {
		Initialize ();
	}

	public static void Initialize() {
		for (int i = 0; i < MAP_WIDTH + 2; i++) {
			for (int j = 0; j < MAP_HEIGHT + 2; j++) {
				//MAP_POS[i,j].x = (i - (float)(MAP_WIDTH+2) / 2 + 0.5f) * MAP_SIZE_X;
				//MAP_POS[i,j].y = -(j - (float)(MAP_HEIGHT+2) / 2 + 0.5f) * MAP_SIZE_Y;
				MAP_POS[i,j].x = (i - (MAP_WIDTH+2)/2) * unitCell + (MAP_WIDTH%2==0? unitCell/2 : 0);
				MAP_POS[i,j].y = (j - (MAP_HEIGHT+2)/2) * unitCell + (MAP_HEIGHT%2==0? unitCell/2 : 0);
				MAP_POS[i,j].z = (j-1)*3;
				mainMap[i,j] = new List<Character>();
			}
		}
	}

	public static IntVector2 BoundPos(IntVector2 unbound) {
		return new IntVector2(Mathf.Clamp(unbound.x, 1, MAP_WIDTH),Mathf.Clamp(unbound.y, 1, MAP_HEIGHT));
	}

	public static void UpdatePos(Character c, IntVector2 pre) {
		if (c.pos == pre)
			return;


		if (c is Enemy) { // Enemy 佔地面積比較多要另外判斷
			Enemy enemy = c as Enemy;
			foreach (IntVector2 offset in enemy.shapeVector) {
				IntVector2 newPos = new IntVector2 (pre.x + offset.x, pre.y + offset.y);
				if (isInOuterBound (newPos)) {
					mainMap [newPos.x, newPos.y].Remove (enemy);
				}
				newPos = new IntVector2 (enemy.pos.x + offset.x, enemy.pos.y + offset.y);
				if (isInBounded (newPos)) {
					mainMap [newPos.x, newPos.y].Add (enemy);
				}
			}
		} else {
			mainMap [pre.x, pre.y].Remove (c);
			mainMap [c.pos.x, c.pos.y].Add(c);
			ShortestMapUpdate(c);
		}

    }

	public IntVector2 Seek(Character c) {
        for (int i = 0; i < MAP_WIDTH + 2; i++) {
            for (int j = 0; j < MAP_HEIGHT + 2; j++) {
                if(mainMap[i,j].Contains(c))
                    return new IntVector2(0, 0);
            }
        }
        return new IntVector2(-1, -1);
    }
	public static List<Character> Seek(IntVector2 pos) {
        return mainMap [pos.x, pos.y];
	}
	public static void Create(Character c) {

		if (c is Enemy) { // Enemy 佔地面積比較多要另外判斷
			Enemy enemy = c as Enemy;
			foreach (IntVector2 offset in enemy.shapeVector) {
				IntVector2 newPos = new IntVector2 (enemy.pos.x + offset.x, enemy.pos.y + offset.y);
				if (isInOuterBound (newPos)) {
					mainMap [newPos.x, newPos.y].Add (enemy);
					Debug.Log ("enemy created!");
					Debug.Log ("No enmey in Map? "+ NoEnemy());
				}
			}
		} else {
			mainMap [c.pos.x, c.pos.y].Add (c);
			ShortestMapUpdate(c);
		}

	}
	public static void Destroy(Character c) {
		if (c is Enemy) { // Enemy 佔地面積比較多要另外判斷
			Enemy enemy = c as Enemy;
			foreach (IntVector2 offset in enemy.shapeVector) {
				IntVector2 newPos = new IntVector2 (enemy.pos.x + offset.x, enemy.pos.y + offset.y);
				if (isInBounded (newPos)) {
					mainMap [newPos.x, newPos.y].Remove (enemy);
				}
			}
		} else {
			mainMap [c.pos.x, c.pos.y].Remove (c);
		}

		ShortestMapUpdate(c);
	}
	
	public static bool IsEmpty(IntVector2 pos) {
		return mainMap [pos.x, pos.y].Count == 0;
	}

	public static Vector3 GetRealPosition(IntVector2 pos, Type type) {

		float z = MAP_POS [pos.x, pos.y].z;
		// Enemy is at higher layer
		//if (c is Enemy) {
		if(type==typeof(Enemy) || type.IsSubclassOf(typeof(Enemy))) {
			z -= 1.0f;
		}

		return new Vector3 (MAP_POS [pos.x, pos.y].x, MAP_POS [pos.x, pos.y].y, z);
	}
	public static Vector3 GetRealPosition(IntVector2 pos, Type type, Vector2 offset) {
		
		float z = MAP_POS [pos.x, pos.y].z;
		// Enemy is at higher layer
		//if (c is Enemy) {
		if(type==typeof(Enemy) || type.IsSubclassOf(typeof(Enemy))) {
			z -= 1.0f;
		}
		return new Vector3 (MAP_POS [pos.x, pos.y].x+offset.x, MAP_POS [pos.x, pos.y].y+offset.y, z);
	}
	public static bool isInBounded(IntVector2 unbound) {
		if (unbound.x > MAP_WIDTH || unbound.x < 1)
			return false;
		if (unbound.y > MAP_HEIGHT || unbound.y < 1)
			return false;
		return true;
	}

	public static bool isInOuterBound(IntVector2 unbound) {
		if (unbound.x > MAP_WIDTH+1 || unbound.x < 0)
			return false;
		if (unbound.y > MAP_HEIGHT+1 || unbound.y < 0)
			return false;
		return true;
	}

	private static void ShortestMapUpdate(Character c)
	{
		for (int i = 0; i < MAP_WIDTH+2; i++)
			for (int j = 0; j < MAP_HEIGHT+2; j++)
			{
				if (mainMap[i, j].Count > 0)
				{
					for (int k = 0; k < mainMap[i, j].Count; k++)
						if (mainMap[i, j][k] is Enemy )
						{
							Enemy enemy = mainMap[i, j][k] as Enemy;
							enemy.mapUpdated = true;
						}
				}
			}
	}

	public static bool NoEnemy() {
		List<Character> list = new List<Character>();
		list = FindAllType<Enemy>();
		if(list.Count==0)
			return true;
		return false;
	}

	public static List<Character> FindAllType<T>() {	
		List<Character> list = new List<Character> ();
		for (int i = 0; i < MAP_WIDTH+2; i++)
			for (int j = 0; j < MAP_HEIGHT+2; j++)
				if (mainMap [i, j].Count > 0)
					for (int k = 0; k < mainMap[i, j].Count; k++)
						if (mainMap [i, j] [k] is T)
							list.Add (mainMap [i, j] [k]);
		return list;
	}

	public static IntVector2 FindTotem002ForEnemy002(IntVector2 origin) {
		List<IntVector2> dirVec = new List<IntVector2>();
		dirVec.Add(Direction.LEFT);
		dirVec.Add(Direction.RIGHT);
		dirVec.Add(Direction.UP);
		dirVec.Add(Direction.DOWN);
		for (int i = 0; i < dirVec.Count; i++) {
			IntVector2 temp = dirVec[i];
			int randomIndex = UnityEngine.Random.Range(i, dirVec.Count);
			dirVec[i] = PathFinder.four_dir[randomIndex];
			dirVec[randomIndex] = temp;
			//Debug.Log (i+": "+dirVec[i]);
		}
		foreach (IntVector2 dir in dirVec) {
			Character c = MapRayCast(origin, dir);
			if(c != null && (c is Totem002) ) {

				Debug.Log (c.GetType() + " found!");
				return dir;
			}
		}
		
		return new IntVector2 (0, 0);
	}

	public static Character MapRayCast(IntVector2 origin, IntVector2 dir) {
		
		int i = origin.x;
		int j = origin.y;
		if (dir == Direction.LEFT) {
			for(i = i-1; i > 0 ; i--) {
				if(mainMap[i, j].Count > 0 && (mainMap[i,j][0] is Player || mainMap[i,j][0] is Totem) ) {
					return mainMap[i,j][0];
				}
			}
		} else if (dir == Direction.RIGHT) {
			for(i = i+1; i <= MAP_WIDTH ; i++) {
				if(mainMap[i, j].Count > 0 && (mainMap[i,j][0] is Player || mainMap[i,j][0] is Totem)) {
					return mainMap[i,j][0];
				}
			}
		} else if (dir == Direction.UP) {
			for(j = j+1; j <= MAP_HEIGHT ; j++) {
				if(mainMap[i, j].Count > 0 && (mainMap[i,j][0] is Player || mainMap[i,j][0] is Totem)) {
					return mainMap[i,j][0];
				}
			}
		} else if (dir == Direction.DOWN) {
			for(j = j-1; j > 0 ; j--) {
				if(mainMap[i, j].Count > 0 && (mainMap[i,j][0] is Player || mainMap[i,j][0] is Totem)) {
					return mainMap[i,j][0];
				}
			}
		}
		
		return null;
	}
}

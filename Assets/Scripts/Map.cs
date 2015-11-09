using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class Boundary {
	public static float xMin = -3.3f;// = -1.875f;
	public static float xMax = 3.3f;// = 1.875f;
	public static float yMin = -3.3f;// = -1.875f;
	public static float yMax = 3.3f;// = 1.875f;
}

public class Map {

	public static int MAP_WIDTH = 10;
	public static int MAP_HEIGHT = 10;
	//public static float MAP_SIZE_X = 1.5f;
	//public static float MAP_SIZE_Y = 1.5f;
	public static float unitCell = 0.75f;
	public static Vector2[,] MAP_POS = new Vector2[MAP_WIDTH+2, MAP_HEIGHT+2];
	public static List<Character>[,] mainMap = new List<Character>[MAP_WIDTH+2, MAP_HEIGHT+2];

	static Map() {

		for (int i = 0; i < MAP_WIDTH + 2; i++) {
			for (int j = 0; j < MAP_HEIGHT + 2; j++) {
				//MAP_POS[i,j].x = (i - (float)(MAP_WIDTH+2) / 2 + 0.5f) * MAP_SIZE_X;
				//MAP_POS[i,j].y = -(j - (float)(MAP_HEIGHT+2) / 2 + 0.5f) * MAP_SIZE_Y;
				MAP_POS[i,j].x = (i - (MAP_WIDTH+2)/2) * unitCell + (MAP_WIDTH%2==0? unitCell/2 : 0);
				MAP_POS[i,j].y = (j - (MAP_HEIGHT+2)/2) * unitCell + (MAP_HEIGHT%2==0? unitCell/2 : 0);
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

		mainMap [pre.x, pre.y].Remove (c);
		mainMap [c.pos.x, c.pos.y].Add(c);
	}
	public IntVector2 Seek(Character c) {
		return new IntVector2 (0, 0);
	}
	public List<Character> Seek(IntVector2 pos) {
		return mainMap [0, 0];
	}
	public void Create(Character c) {
	}
	public void Destroy(Character c) {
	}

}

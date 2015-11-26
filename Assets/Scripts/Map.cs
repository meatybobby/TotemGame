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
	public static Vector3[,] MAP_POS = new Vector3[MAP_WIDTH+2, MAP_HEIGHT+2];
	private static List<Character>[,] mainMap = new List<Character>[MAP_WIDTH+2, MAP_HEIGHT+2];

	static Map() {

		for (int i = 0; i < MAP_WIDTH + 2; i++) {
			for (int j = 0; j < MAP_HEIGHT + 2; j++) {
				//MAP_POS[i,j].x = (i - (float)(MAP_WIDTH+2) / 2 + 0.5f) * MAP_SIZE_X;
				//MAP_POS[i,j].y = -(j - (float)(MAP_HEIGHT+2) / 2 + 0.5f) * MAP_SIZE_Y;
				MAP_POS[i,j].x = (i - (MAP_WIDTH+2)/2) * unitCell + (MAP_WIDTH%2==0? unitCell/2 : 0);
				MAP_POS[i,j].y = (j - (MAP_HEIGHT+2)/2) * unitCell + (MAP_HEIGHT%2==0? unitCell/2 : 0);
				MAP_POS[i,j].z = j;
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
				mainMap [pre.x + offset.x, pre.y + offset.y].Remove (enemy);
				mainMap [enemy.pos.x + offset.x, enemy.pos.y + offset.y].Add (enemy);
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
				mainMap[enemy.pos.x+offset.x, enemy.pos.y+offset.y].Add(enemy);
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
				mainMap[enemy.pos.x+offset.x, enemy.pos.y+offset.y].Remove(enemy);
			}
		} else {
			mainMap [c.pos.x, c.pos.y].Remove (c);
		}

		ShortestMapUpdate(c);
	}
	
	public static bool IsEmpty(IntVector2 pos) {
		return mainMap [pos.x, pos.y].Count == 0;
	}

	public static Vector3 GetRealPosition(IntVector2 pos){
		return new Vector3 (MAP_POS [pos.x, pos.y].x, MAP_POS [pos.x, pos.y].y, MAP_POS [pos.x, pos.y].z);
	}

    private static void ShortestMapUpdate(Character c)
    {
        for (int i = 0; i < MAP_WIDTH+2; i++)
            for (int j = 0; j < MAP_HEIGHT+2; j++)
            {
                if (mainMap[i, j].Count > 0)
                {
                    for (int k = 0; k < mainMap[i, j].Count; k++)
                        if (mainMap[i, j][k].GetType() == typeof(Enemy) && (Enemy)mainMap[i, j][k] != c)
                        {
                            Enemy enemy = (Enemy)mainMap[i, j][k];
							enemy.mapUpdated = true;
                        }
                }
            }
    }
}

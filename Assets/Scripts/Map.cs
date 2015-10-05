using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Map {

	public static int MAP_WIDTH = 6;
	public static int MAP_HEIGHT = 6;
	public static float MAP_SIZE_X = 1.5f;
	public static float MAP_SIZE_Y = 1.5f;

	public static float unitCell = 0.75f;

	public static Vector2[,] MAP_POS = new Vector2[MAP_WIDTH+2, MAP_HEIGHT+2];
	public static List<Character>[,] mainMap = new List<Character>[MAP_WIDTH+2, MAP_HEIGHT+2];

	static Map() {

		for (int i = 0; i < MAP_WIDTH + 2; i++) {
			for (int j = 0; j < MAP_HEIGHT + 2; j++) {
				//MAP_POS[i,j].x = (i - (float)(MAP_WIDTH+2) / 2 + 0.5f) * MAP_SIZE_X;
				//MAP_POS[i,j].y = -(j - (float)(MAP_HEIGHT+2) / 2 + 0.5f) * MAP_SIZE_Y;
				MAP_POS[i,j].x = (i-4) * 0.75f + 0.375f;
				MAP_POS[i,j].y = (j-4) * 0.75f + 0.375f;
			}
		}

	}

	public void UpdatePos(){
	}
	public Vector2 Seek(Character c) {
		return new Vector2 (0, 0);
	}
	public List<Character> Seek(Vector2 pos) {
		return mainMap [0, 0];
	}
	public void Create(Character c) {
	}
	public void Destroy(Character c) {
	}

}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Boss001: Enemy {
	public GameObject warningArea;
	public float timer;

	void Start () {
		timer = 0;
	}
	
	
	void Update () {
		timer += Time.deltaTime;
		if (timer >= attackIntv) {
			timer = 0;
			int attackRow = Random.Range(1, Map.MAP_WIDTH + 1);
			Vector2 realPos = Map.GetRealPosition(new IntVector2(attackRow, Map.MAP_HEIGHT / 2), this.GetType());
			Quaternion charRotation = Quaternion.Euler (0f, 0f, 0f);
			Instantiate(warningArea, realPos, charRotation);
		}
	}
	
}

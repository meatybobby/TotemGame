﻿using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public GameObject monster1;
	public float waveWait;
	public int enemyCount = 0;
	public int enemyNum;

	void Start () {
		StartCoroutine( MonsterWaves ());
	}

	void Update () {
	
	}

	
	IEnumerator MonsterWaves () {
		while(true){
			/*for(int i = 0; i < enemyCount; i++) {

			}*/
			if(enemyCount>=enemyNum) yield break;
			IntVector2 newPos = new IntVector2(4,1);
			Vector3 enemyPosition = Map.GetRealPosition(newPos);
			Quaternion enemyRotation = Quaternion.Euler(0f,0f,-90.0f);
			Instantiate(monster1, enemyPosition , enemyRotation);
			Enemy enemy = monster1.GetComponent<Enemy>();
			enemy.pos = newPos;
			enemy.Rotate(enemy.dir);
			enemyCount++;
//			Map.Create(enemy);
			yield return new WaitForSeconds(waveWait);

		}
		
	}
}

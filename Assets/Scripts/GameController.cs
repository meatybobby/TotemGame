using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public GameObject monster1;
	public GameObject monster2;
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
			int rx,ry;
			IntVector2 newPos = new IntVector2(0,0);
			while(true) {
				rx = Random.Range(1,Map.MAP_WIDTH);
				ry = Random.Range(1,Map.MAP_HEIGHT);
				newPos.x = rx;
				newPos.y = ry;
				if(Map.IsEmpty(newPos) || Map.Seek(newPos)[0] is Enemy) break;
			}

			Vector3 enemyPosition = Map.GetRealPosition(newPos, new Enemy());
			//enemyPosition.x += Map.unitCell/2;
			//enemyPosition.y += Map.unitCell/2;
			Quaternion enemyRotation = Quaternion.Euler(0f,0f,0f);
			Enemy enemy = ((GameObject)Instantiate(monster1, enemyPosition , enemyRotation)).GetComponent<Enemy>() ;
			enemy.pos = newPos;
			enemyCount++;
//			Map.Create(enemy);
			yield return new WaitForSeconds(waveWait);

		}
		
	}
}

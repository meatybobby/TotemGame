using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	/*public GameObject monster1;
	public GameObject monster2;
	public GameObject monster3;
	public GameObject boss1;
	public float waveWait;
	public int enemyCount = 0;
	public int enemyNum;*/
	public float timer;
	public WaveController wave;
	private WaveInformation next;
	void Start () {
		timer = 0;
		wave = GetComponent<WaveController>();
		wave.ReadFile("level1");
		next = wave.Next ();
	//	Map.Initialize ();
	}

	void Update () {
		timer += Time.deltaTime;
		if (next != null) {
			while (next != null && timer >= next.bornTime) {
				Character c = next.monster.GetComponent<Character> ();
				Vector3 charPosition;
				//charPosition = Map.GetRealPosition (next.bornPos, typeof(Enemy), c.offset);
				if(c.gameObject.tag=="Enemy") {
					Enemy e = c as Enemy;
					charPosition = Map.GetRealPosition (next.bornPos, typeof(Enemy), e.offset);
				}
				else {
					charPosition = Map.GetRealPosition (next.bornPos, typeof(Enemy));
				}
				
				//Enemy enemy = next.monster.GetComponent<Enemy> ();
				
				//Vector3 enemyPosition = Map.GetRealPosition (next.bornPos, typeof(Enemy), enemy.offset);
				//Debug.Log (enemy.offset);
				Quaternion charRotation = Quaternion.Euler (0f, 0f, 0f);
				c = ((GameObject)Instantiate (next.monster, charPosition, charRotation)).GetComponent<Character> ();
				c.pos = next.bornPos;
				Map.Create (c);
				next = wave.Next ();
			}
		}
	}

	public void Reload() {
		Time.timeScale = 1;
		Application.LoadLevel (Application.loadedLevelName);
	}

	
	/*IEnumerator MonsterWaves () {
		while(true) {
			if(enemyCount>=enemyNum) yield break;
			int rx,ry;
			IntVector2 newPos = new IntVector2(0,0);
			/*while(true) {
				rx = Random.Range(1,Map.MAP_WIDTH);
				ry = Random.Range(1,Map.MAP_HEIGHT);
				newPos.x = rx;
				newPos.y = ry;
				if(Map.IsEmpty(newPos) || Map.Seek(newPos)[0] is Enemy) break;
			}
			newPos = new IntVector2(Map.MAP_WIDTH-1,Map.MAP_HEIGHT-1);
			Vector3 enemyPosition = Map.GetRealPosition(newPos, typeof(Enemy));
			//enemyPosition.x += Map.unitCell/2;
			//enemyPosition.y += Map.unitCell/2;
			Quaternion enemyRotation = Quaternion.Euler(0f,0f,0f);

			Enemy enemy = ((GameObject)Instantiate(boss1, enemyPosition , enemyRotation)).GetComponent<Enemy>() ;
			enemy.pos = newPos;
			enemyCount++;
//			Map.Create(enemy);
			yield return new WaitForSeconds(waveWait);

		}
		
	}*/
}

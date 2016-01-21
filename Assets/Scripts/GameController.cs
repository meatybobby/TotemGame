using UnityEngine;
using System.Collections;
using Soomla.Levelup;
namespace UnityStandardAssets.CrossPlatformInput
{
public class GameController : MonoBehaviour {

	/*public GameObject monster1;
	public GameObject monster2;
	public GameObject monster3;
	public GameObject boss1;
	public float waveWait;
	public int enemyCount = 0;
	public int enemyNum;*/
	public float timer;
	private WaveController wave;
	private WaveInformation next;
	public GameObject warningPrefab;
	public GameObject warningPar;
	public Joystick joystick1;
	public Joystick joystick2;
	public GameObject playerObject;
	private Player player;	
	
	public Transform canvasTranform;
	public GameObject gameOverPrefab, gameClearPrefab, gameStartPrefab;
	private GameObject gameOver, gameClear, gameStart;
	private const int GAME_START=0, GAME_CLEAR=1, GAME_OVER=2;
	
	Level level;
	public bool end = false;
	
	public static bool bossDead = false;
		
	public string levelId;
	
	


	void Start () {
		//Time.timeScale = 1;
		Map.Initialize ();
		timer = 0;
		
		PlayTextMessage(GAME_START);
		
		
		//levelId = "level" + ApplicationModel.levelNum;
		//level = new Level(levelId);
		level = ApplicationModel.GetCurrentLevel();
		
		
		wave = GetComponent<WaveController>();
		wave.ReadFile(level.ID);
		next = wave.Next ();
		initWarning ();
		joystick1.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width/2,Screen.height);
		joystick2.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width/2,Screen.height);
		player = playerObject.GetComponent<Player>();
		
		
		level.Start();
		Debug.Log( "This level has been started "+level.GetTimesStarted()+" times.");
		Debug.Log( "This level has been played "+level.GetTimesPlayed()+" times.");

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
				Debug.Log ("No enmey in Map? "+Map.NoEnemy());
				next = wave.Next ();
			}
		}
		if(!end && player.isDead) {
			Debug.Log("Game Over!");
			end = true;
			level.End(false);
			PlayTextMessage(GAME_OVER);
			Invoke("ReturnToMap", 5f);
		}
		else if(timer > Time.deltaTime && next==null  && !end && Map.NoEnemy() && (!(level.ID=="level5") || bossDead) ) {
			Debug.Log ("You win!");
			end = true;
			level.End(true);
			PlayTextMessage(GAME_CLEAR);
			Debug.Log ("Duration: "+level.GetPlayDurationMillis());
			Debug.Log ("Fastest: "+level.GetFastestDurationMillis());
			Invoke("ReturnToMap", 5f);
		}
		
	}
	
	
	private void PlayTextMessage(int type) {
		switch(type) {
			case GAME_START:
				gameStart = Instantiate(gameStartPrefab) as GameObject;
				gameStart.transform.SetParent(canvasTranform, false);
				break;
			case GAME_OVER:
				gameOver = Instantiate(gameOverPrefab) as GameObject;
				gameOver.transform.SetParent(canvasTranform, false);
				break;
			case GAME_CLEAR:
				gameClear = Instantiate(gameClearPrefab) as GameObject;
				gameClear.transform.SetParent(canvasTranform, false);
				break;
		}
	}
	public static void BossDie() {
		bossDead = true;
	}

	// Red warning area for enemies initially set to invisible
	private void initWarning() {
		IntVector2 temp = new IntVector2(0,0);
		Quaternion charRotation = Quaternion.Euler (0f, 0f, 0f);
		for (int i = 1; i <= Map.MAP_WIDTH; i++) {
			for (int j = 1; j <= Map.MAP_HEIGHT; j++) {
				temp.x = i;
				temp.y = j;
				Vector3 pos = Map.GetRealPosition(temp,typeof(Enemy));
				pos.z = 2;
				Map.warningArea[i,j] = Instantiate(warningPrefab, pos, charRotation) as GameObject;
				Map.warningArea[i,j].transform.parent = warningPar.transform;
				Map.warningArea[i,j].SetActive(false);
			}
		}
	}
	public void Reload() {
		Time.timeScale = 1;
		Application.LoadLevel (Application.loadedLevelName);
	}
	public void ReturnToMap() {
		Time.timeScale = 1;
		Application.LoadLevel("LevelSelect");
	}
	/*public static void Pause() {
		Time.timeScale = 0;
		level.Pause();
	}*/
	
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
}

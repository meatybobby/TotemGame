using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Soomla.Levelup;
using UnityStandardAssets.CrossPlatformInput;


public class GameController : MonoBehaviour {

	public float timer;
	private float waveTime;
	private WaveController wave;
	private WaveInformation next;
	public GameObject warningPrefab;
	public GameObject warningPar;
	public GameObject playerObject;
	private Player player;	
	
	public Transform canvasTranform;
	public GameObject gameOverPrefab, gameClearPrefab, gameStartPrefab;
	private GameObject gameOver, gameClear, gameStart;
	private const int GAME_START=0, GAME_CLEAR=1, GAME_OVER=2;
	private const int GAME_MUSIC=0, BOSS_MUSIC=1, CLEAR_SOUND=2, OVER_SOUND=3;
	

	public AudioClip gameMusic, bossMusic, clearSound, overSound;
	private AudioSource myAudio;
	
	Level level;
	public bool end = false;

	public string levelId;

	private int originHP, originMana;

	private bool gameStartPlayed;
	public GameObject fader;
	private FadeEffect fadeEffect;
	
	
	void Awake() {
		Destroy (GameObject.Find("BGM_start"));

	}
	
	void Start () {
		//Time.timeScale = 1;
		Map.Initialize ();
		timer = 0;
		waveTime = 0;

		ApplicationModel.currentEnemyNum = 0;

		myAudio = GetComponent<AudioSource>();
		PlaySound(GAME_MUSIC);
	
		level = ApplicationModel.GetCurrentLevel();	
		
		wave = GetComponent<WaveController>();
		wave.ReadFile(level.ID);
		next = wave.Next ();
		initWarning ();
		player = playerObject.GetComponent<Player>();
		
		
		level.Start();
		Debug.Log( "This level has been started "+level.GetTimesStarted()+" times.");
		Debug.Log( "This level has been played "+level.GetTimesPlayed()+" times.");

		fadeEffect = fader.GetComponent<FadeEffect> ();
		fadeEffect.FadeIn ();
		gameStartPlayed = false;

	}
	
	void Update () {
		if (!gameStartPlayed && fadeEffect.fadeInFinish) {
			PlayTextMessage (GAME_START);
			gameStartPlayed = true;
		}
		
		timer += Time.deltaTime;
		waveTime += Time.deltaTime;
		if (next != null) {
			while (next != null && waveTime >= next.bornTime) {
				Character c = next.monster.GetComponent<Character> ();
				Vector3 charPosition;
				//charPosition = Map.GetRealPosition (next.bornPos, typeof(Enemy), c.offset);
				if(c.gameObject.tag=="Enemy") {
					Enemy e = c as Enemy;
					charPosition = Map.GetRealPosition (next.bornPos, typeof(Enemy), e.offset);
					ApplicationModel.currentEnemyNum++;
					if(next.waveItemType==4) { // boss
						PlaySound(BOSS_MUSIC);
					}
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
		if(!end && player.isDead) {
			Debug.Log("Game Over!");
			end = true;
			level.End(false);
			PlayTextMessage(GAME_OVER);
			PlaySound(OVER_SOUND);
			Invoke("ReturnToMap", 3f);
		}
		else if(timer > Time.deltaTime && next==null  && !end && ApplicationModel.currentEnemyNum<=0 ) {
			if (!wave.NextWave ()) {
				Debug.Log ("You win!");
				end = true;
				level.End (true);
				PlayTextMessage (GAME_CLEAR);
				PlaySound (CLEAR_SOUND);
				Debug.Log ("Duration: " + level.GetPlayDurationMillis ());
				Debug.Log ("Fastest: " + level.GetFastestDurationMillis ());
				Invoke ("ReturnToMap", 3f);
			} else {
				waveTime = 0;
				next = wave.Next ();
			}
		}
		
	}
	
	private void PlaySound(int type) {
		myAudio.Stop();
		switch(type) {
			case GAME_MUSIC:
				myAudio.clip = gameMusic;
				myAudio.Play();
				break;
			case BOSS_MUSIC:
				myAudio.clip = bossMusic;
				myAudio.Play();
				break;
			case CLEAR_SOUND:
				myAudio.PlayOneShot(clearSound);
				break;
			case OVER_SOUND:
				myAudio.PlayOneShot(overSound);
			break;

		}
	}
	public void PauseSound(bool pause) {
		if(pause)
			myAudio.Pause();
		else
			myAudio.Play();
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
	public void ReturnToMap ()
	{
		Time.timeScale = 1;
		fadeEffect.FadeOut ();
		StartCoroutine (waitFadeOut ());
	}
	private IEnumerator waitFadeOut(){
		while(!fadeEffect.fadeOutFinish)
			yield return new WaitForSeconds(0.01f);
		Application.LoadLevel ("LevelSelect");
	}
	
	public void Inifinite ()
	{
		if (player.maxHP != 99999) {
			originHP = player.maxHP;
			player.maxHP = 99999;
			player.HP = 99999;
			TotemSummoner tm = player.GetComponent<TotemSummoner> ();
			originMana = tm.manaMax;
			tm.manaMax = 99999;
			tm.mana = 99999;
		} else {
			player.maxHP = originHP;
			player.HP = originHP;
			TotemSummoner tm = player.GetComponent<TotemSummoner> ();
			tm.manaMax = originMana;
			tm.mana = originMana;
		}
		GetComponent<UIController> ().CloseMenu ();
	}
	
	public void AllDie() {
		List<Character> allEnemy = Map.FindAllType<Enemy> ();
		foreach (Enemy e in allEnemy) {
			e.CauseDamage(9999);
		}
		GetComponent<UIController> ().CloseMenu ();
	}
		

}
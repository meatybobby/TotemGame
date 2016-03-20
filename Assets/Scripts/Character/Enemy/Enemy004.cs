using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Enemy004 : Enemy {
	public GameObject attackTail;
	public GameObject bullet;
	public float timer;
	private int attackRow;
	private int attackMode;
	public int waitTime;
	public float warningTime;
	private Animator anim;
	private Animator anitor;
	public GameObject shotSpawn;
	public GameObject bossHead;
	public CameraController mainCamera;
	private int[] leftAttack = new int[]{9,6,6,6,5,5,4,3,2,1};
	private int[] rightAttack = new int[]{2,5,5,5,6,6,7,7,8,9};

	private AudioSource myAudio;
	public AudioClip enterSound, attackBulletSound, attackTailSound;
	public const int ENTER_SOUND=0, ATTACK_BULLET_SOUND=1, ATTACK_TAIL_SOUND=2, DIE_SOUND=3;

	void Start () {
		timer = 0;
		myAudio = GetComponent<AudioSource>();
		shapeVector = new List<IntVector2> ();
		Vector3 orgin = transform.position;
		transform.position = new Vector3(orgin.x + offset.x, orgin.y + offset.y, orgin.z);
		anim = attackTail.GetComponent<Animator> ();
		anitor = GetComponent<Animator> ();
		Initialize ();
		isAttack = false;
		mainCamera = Camera.main.GetComponentInParent<CameraController> ();
		moveCamera ();
		PlaySound(ENTER_SOUND);
	}
	

	void Update () {
		if (HP <= 0 && !isDead) {
			Die ();
		} else if(!isDead) {
			if(!isAttack) timer += Time.deltaTime;
			if (timer > waitTime) {
				waitTime = -1;
			}
			if (timer >= attackIntv && timer >= waitTime && !isAttack) {
				isAttack = true;
				attackMode = Random.Range (0, 4);
				Debug.Log (attackMode);
				if (attackMode == 3) {
					anitor.SetTrigger ("fire");
					StartCoroutine (shotBullet ());
				} else
					warning ();
				timer = 0;
			}
		}
	}

	void warning() {
		if (attackMode == 0) {
			attackRow = Random.Range (-2, 2) + player.pos.x;
			if (attackRow < 1)
				attackRow = 1;
			else if (attackRow > Map.MAP_WIDTH)
				attackRow = Map.MAP_WIDTH;
			for (int i = 1; i<=Map.MAP_HEIGHT; i++) {
				Map.warningArea [attackRow, i].SetActive (true);
			}
		} else if (attackMode == 1) {
			for (int i = 1; i <= Map.MAP_WIDTH; i++)
				for (int j = 1; j <= leftAttack[i-1]; j++)
					Map.warningArea [j, i].SetActive (true);
		} else if (attackMode == 2) {
			for (int i = 1; i <= Map.MAP_WIDTH; i++)
				for (int j = rightAttack[i-1]; j <= Map.MAP_HEIGHT; j++)
					Map.warningArea [j, i].SetActive (true);
		}
		Invoke ("attack", warningTime);
	}

	void attack() {
		if (attackMode == 0) {
			for (int i = 1; i<= Map.MAP_HEIGHT; i++) {
				Map.warningArea [attackRow, i].SetActive (false);
			}
			Vector3 realpos = Map.GetRealPosition (new IntVector2 (attackRow, 0), typeof(Enemy));
			Vector3 originPos = attackTail.transform.position;
			attackTail.transform.position = new Vector3 (realpos.x, -4.623681f, originPos.z);
			anim.SetTrigger ("attack1");
		} else if (attackMode == 1) {
			for(int i = 1; i <= Map.MAP_WIDTH; i++)
				for(int j = 1; j <= leftAttack[i-1] ; j++)
					Map.warningArea [j, i].SetActive (false);
			Vector3 originPos = attackTail.transform.position;
			attackTail.transform.position = new Vector3 (-4f, -21f, originPos.z);
			anim.SetTrigger ("attack2");
		} else if(attackMode == 2) {
			for(int i = 1; i <= Map.MAP_WIDTH; i++)
				for(int j = rightAttack[i-1]; j <= Map.MAP_HEIGHT ; j++)
							Map.warningArea [j, i].SetActive (false);
			Vector3 originPos = attackTail.transform.position;
			attackTail.transform.position = new Vector3 (4.75f, -23.3f, originPos.z);
			anim.SetTrigger ("attack2_right");
		}
		PlaySound(ATTACK_TAIL_SOUND);
	}

	IEnumerator shotBullet() {
		yield return new WaitForSeconds(1.7f);
		PlaySound(ATTACK_BULLET_SOUND);
		Instantiate(bullet, shotSpawn.transform.position, shotSpawn.transform.rotation);
		Quaternion q = Quaternion.Euler (0, 0, 120);
		Instantiate(bullet, shotSpawn.transform.position, q);
		q = Quaternion.Euler (0, 0, 240);
		Instantiate(bullet, shotSpawn.transform.position, q);
		yield return new WaitForSeconds(1f);
		q = Quaternion.Euler (0, 0, 150);
		Instantiate(bullet, shotSpawn.transform.position, q);
		q = Quaternion.Euler (0, 0, 210);
		Instantiate(bullet, shotSpawn.transform.position, q);
		isAttack = false;
	}

	private void PlaySound(int type) {
		switch(type) {
		case ENTER_SOUND:
			myAudio.PlayOneShot(enterSound);
			break;
		case ATTACK_BULLET_SOUND:
			myAudio.PlayOneShot(attackBulletSound);
			break;
		case ATTACK_TAIL_SOUND:
			myAudio.PlayOneShot(attackTailSound);
			break;
		case DIE_SOUND:
			myAudio.PlayOneShot(dieSound);
			break;
		}
	}

	public void Die() {
		isDead = true;
		for(int i = 1; i <= Map.MAP_WIDTH; i++)
			for(int j = 1; j <= Map.MAP_HEIGHT ; j++)
				Map.warningArea [j, i].SetActive (false);
		Map.Destroy(this);
		Collider2D[] colliders = GetComponentsInChildren<Collider2D> ();
		foreach (Collider2D c in colliders) {
			Destroy(c);
		}
		moveCamera ();
		anitor.SetTrigger("die");
		Destroy(gameObject, 4.9f);
		PlaySound(DIE_SOUND);

	}
	public void FinallyDie() {
		ApplicationModel.currentEnemyNum--;
		cameraResume();
	}

	public void cameraResume() {
		mainCamera.target = player.gameObject;
	}

	public void moveCamera() {
		mainCamera.target = bossHead;
	}
}

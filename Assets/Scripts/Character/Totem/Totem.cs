using UnityEngine;
using System.Collections;

public class Totem : Character {

	public const int IDLE = 0;
	public const int FIRE = 1;
	public const int DIE = 2;
	public const int SUMMON = 3;

	public bool isCaught;
	public Player playerRef;
	public GameObject steam;

	public GameObject range;

	public AudioClip dieSound;

	public void Initialize() {
		isDead = false;
		SummonSteam ();
		//Hp GUI initialize
		HpInitialize ();
	}
		
	public void Die() {
		isDead = true;
		Destroy (healthPanel);
		PlayDieSound();
		if(range) Destroy (range);
		Destroy (this);
		Map.Destroy(this);
		SetCatchLight (false);
		Destroy (GetComponent<Collider2D>());
		Destroy(gameObject, 2.5f);
		if(isCaught) {
			playerRef.SetIdle(true);
		}
	}

	private void PlayDieSound() {
		GetComponent<AudioSource>().PlayOneShot (dieSound ,  0.3F);
	}

	public void CaughtByPlayer() {
		isCaught = true;
		transform.parent = playerRef.transform;
		SetCatchLight (true);
	}

	public void ReleasedByPlayer() {
		isCaught = false;
		transform.parent = null;
		SetCatchLight (false);
	}

	// for the z pos of totem002, 003, 004, which should always be at bottom-most
	protected void ResetRangePosition() {
		Vector3 rangePos =  transform.FindChild("range").transform.position;
		transform.FindChild("range").transform.position = 
			new Vector3(rangePos.x, rangePos.y, 30f);
	}

	private void SummonSteam() {
		GameObject steamObj = Instantiate(steam, transform.position, transform.rotation) as GameObject;
		Destroy (steamObj, 1f);
	}
	private void SetCatchLight(bool val) {
		transform.FindChild("TotemCatchLight").gameObject.SetActive(val);
	}

}

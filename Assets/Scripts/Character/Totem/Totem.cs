﻿using UnityEngine;
using System.Collections;

public class Totem : Character {

	public const int IDLE = 0;
	public const int FIRE = 1;
	public const int DIE = 2;
	public const int SUMMON = 3;

	public bool isCaught;
	public Player playerRef;
	public GameObject steam;

	public void Initialize() {
		isDead = false;
		SummonSteam ();
	}
		
	public void Die() {
		isDead = true;
		Destroy (this);
		Map.Destroy(this);
		SetCatchLight (false);
		Destroy (GetComponent<Collider2D>());
		Destroy(gameObject, 2.5f);
		if(isCaught) {
			playerRef.SetIdle();
		}
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

	private void SummonSteam() {
		GameObject steamObj = Instantiate(steam, transform.position, transform.rotation) as GameObject;
		Destroy (steamObj, 1f);
	}
	private void SetCatchLight(bool val) {
		transform.FindChild("TotemCatchLight").gameObject.SetActive(val);
	}

}

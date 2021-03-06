﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Totem002 : Totem {
	
	public Animator anim;
	private Totem002Taunt taunt;
	
	void Start () {
		anim = GetComponent<Animator>();
		taunt = transform.FindChild ("Taunt").gameObject.GetComponent<Totem002Taunt>();
		Initialize();
		
	}
	void Update() {
		//Hp GUI
		HpUpdate ();
		ResetRangePosition();
		if(HP<=0 && !isDead) {
			Die ();
		}

	}
	public void Die() {
		anim.SetTrigger("isDie");

		foreach (Enemy e in taunt.enemyList) {
			e.SetPriority(characterId, e.defaultPriority[characterId]);
			e.CancelAngry();
		}

		base.Die ();
	}

}

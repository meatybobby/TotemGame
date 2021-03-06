﻿using UnityEngine;
using System.Collections;

public class Totem001 : Totem {

	public Transform shotSpawn;
	public GameObject shot;
	public float bulletSpeed;
	public float shotIntv;
	public Totem001Anim anim;

	public AudioSource audio;
	public AudioClip shootSound;

	void Start () {
		anim = GetComponent<Totem001Anim> ();
		audio = GetComponent<AudioSource>();
		Initialize();

		StartCoroutine (Shooting ());
	}

	void Update() {
		//Hp GUI
		HpUpdate ();

		if(HP<=0 && !isDead) {
			Die ();
		}
	}

	public void Die(){
		anim.playAnim (dir, DIE);
		base.Die ();
	}

	IEnumerator Shooting () {
		while(true) {
			if(HP<=0) {
				yield break;
			}
			// if totem faces up, left, or right, then 子彈要在圖騰下層
			if(dir==Direction.UP || dir==Direction.LEFT || dir==Direction.RIGHT) { 
				shotSpawn.position = new Vector3(shotSpawn.position.x, shotSpawn.position.y, transform.position.z+1);
			}
			anim.playAnim (dir, FIRE);
			audio.PlayOneShot(shootSound, /*0.2f*/shotIntv);
			GameObject bullet = Instantiate(shot, shotSpawn.position, shotSpawn.rotation) as GameObject;
			bullet.GetComponent<TotemBullet>().shooter = this;
			yield return new WaitForSeconds(shotIntv);
		}
		
	}

	public void Rotate(IntVector2 a){
		dir = a;
		
		int angle;
		angle = a.x==0? (90*a.y):(90 - 90*a.x + 45*a.x*a.y);
		
		// rotate shotSpawn for rotating the direction of the flight of the bullet
		shotSpawn.rotation = Quaternion.Euler (0.0f, 0.0f, (float)angle);
	}

}

using UnityEngine;
using System.Collections;

public class Totem001 : Totem {

	public Transform shotSpawn;
	public GameObject shot;
	
	public float bulletSpeed;
	public float shotIntv;
	public Totem001Anim anim;
	
	void Start () {
		anim = GetComponent<Totem001Anim> ();
		anim.playAnim (dir, SUMMON);
		StartCoroutine (Shooting ());
		characterId = 1;
	}
	

	IEnumerator Shooting () {
		yield return new WaitForSeconds(1.0f);
		GameObject steamObj = Instantiate(steam, transform.position, transform.rotation) as GameObject;
		Destroy (steamObj, 1f);
		while(true) {
			if(HP<=0) {
				yield break;
			}
			// if totem faces up, left, or right, then 子彈要在圖騰下層
			if(dir==Direction.UP || dir==Direction.LEFT || dir==Direction.RIGHT) { 
				shotSpawn.position = new Vector3(shotSpawn.position.x, shotSpawn.position.y, transform.position.z+1);
			}
			anim.playAnim (dir, FIRE);
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

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag=="MonsterHand" 
		    /*|| other.tag == "bullet" && other.GetComponent<TotemBullet>().shooter!=this*/) {
			//Debug.Log ("totem attacked by monster!");
			HP--;
			if(HP==0) {
				anim.playAnim (dir, DIE);
				Destroy (GetComponent<BoxCollider2D>());
				transform.FindChild("TotemCatchLight").gameObject.SetActive(false);
				Destroy(gameObject, 2.5f);
				if(isCaught) {
					playerRef.SetIdle();
				}
				Map.Destroy(this);
			}
		}
		
	}

}

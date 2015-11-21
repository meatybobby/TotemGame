using UnityEngine;
using System.Collections;

public class Totem : Character {

	public const int IDLE = 0;
	public const int FIRE = 1;
	public const int DIE = 2;

	public bool isCaught;
	public Player playerRef;
	public Transform shotSpawn;
	public GameObject shot;

	public float bulletSpeed;
	public float shotIntv;
	public Totem001Anim totemAnim;



	// Use this for initialization
	void Start () {
		totemAnim = GetComponent<Totem001Anim> ();
		totemAnim.playAnim (dir, FIRE);
		StartCoroutine (Shooting ());
	}


	IEnumerator Shooting () {
		while(true) {
			GameObject obj = Instantiate(shot, shotSpawn.position, shotSpawn.rotation) as GameObject;
			obj.GetComponent<Rigidbody2D>().velocity = -shotSpawn.up * bulletSpeed;
			yield return new WaitForSeconds(shotIntv);
		}
		
	}

	public void Rotate(IntVector2 a){
		dir = a;
		
		int angle;
		angle = a.x==0? (90*a.y):(90 - 90*a.x + 45*a.x*a.y);

		shotSpawn.rotation = Quaternion.Euler (0.0f, 0.0f, (float)angle+90.0f);


	}

	void OnTriggerEnter2D(Collider2D other) {
		// Destroy everything that leaves the trigger
		if (other.tag == "MonsterHand") {
			//Debug.Log ("totem attacked by monster!");
			HP--;
			if(HP<=0) {
				Destroy(gameObject);
				Map.Destroy(this);
			}
		}
		
	}
}

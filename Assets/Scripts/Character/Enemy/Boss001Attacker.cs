using UnityEngine;
using System.Collections;

public class Boss001Attacker : MonoBehaviour {
	
	public float warningTime;
	public float burnTime;
	public bool inBurn;
	public float timer;
	public Boss001 boss001;
	
	// Use this for initialization
	void Start () {
		inBurn = false;
		timer = 0;
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
		if (inBurn) {
			if(timer>=burnTime)
				Destroy(gameObject);
		} else if(timer >= warningTime) {
			timer = 0;
			inBurn = true;
			SpriteRenderer r = GetComponentInChildren<SpriteRenderer>();
			r.color = new Color(0.7f , 0.7f , 0.7f , 0.3f) ;
		}
	}
	

	void OnTriggerEnter2D(Collider2D other) {
		if (inBurn) {
			if (other.tag == "Player" || other.tag == "Totem" || other.tag == "Rock") {
				Character c = other.GetComponent<Character> ();
				c.CauseDamage (boss001.damage);
			}
		}
		
	}
}

using UnityEngine;
using System.Collections;

public class SoulController : MonoBehaviour {
	public Player player;
	public TotemSummoner summoner;
	// Use this for initialization
	void Start () {
		player = GameObject.FindWithTag ("Player").GetComponent<Player> ();
		summoner = player.GetComponent<TotemSummoner> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
		
	protected IEnumerator MoveThread() {
		Vector3 nextXY = new Vector3 (player.transform.position.x, player.transform.position.y, transform.position.z); // 不要移動z
		while (transform.position != nextXY) {
			nextXY = new Vector3 (player.transform.position.x, player.transform.position.y, transform.position.z);
			transform.position = Vector3.MoveTowards(transform.position, nextXY, 3.5f * Time.deltaTime);
			yield return null;
		}
		transform.position = player.transform.position;//new Vector3( next.x, next.y, next.z);
		Destroy(gameObject);
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Player") {
			summoner.addMana (1);
			Destroy (GetComponent<Collider2D> ());
			StartCoroutine(MoveThread ());
		}
	}
}

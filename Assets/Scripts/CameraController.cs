using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public GameObject player;
	public float minX, maxX, minY, maxY;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//transform.position = player.transform.position;
		float playerX = player.transform.position.x;
		float playerY = player.transform.position.y;
		float xPos = Mathf.Clamp(playerX, minX, maxX);
		float yPos = Mathf.Clamp(playerY, minY, maxX);
		float zPos = transform.position.z;
		transform.position = new Vector3 (xPos, yPos, zPos);

	}
}

using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public GameObject player;
	public static float minX, maxX, minY, maxY;

	//public float dampTime = 0.15f;
	//private Vector3 velocity = Vector3.zero;

	//public Transform target;
	

	// Update is called once per frame
	/*void FixedUpdate () 
	{
		if (target)
		{
			Vector3 point =  GetComponentInChildren<Camera>().WorldToViewportPoint(target.position);
			Vector3 delta = target.position - GetComponentInChildren<Camera>().ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
			Vector3 destination = transform.position + delta;
			transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
		}
		
	}
*/
	public float interpVelocity;
	public float minDistance;
	public float followDistance;
	public GameObject target;
	public Vector3 offset;
	Vector3 targetPos;
	// Use this for initialization
	void Start () {
		targetPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (target)
		{
			Vector3 posNoZ = transform.position;
			posNoZ.z = target.transform.position.z;
			
			Vector3 targetDirection = (target.transform.position - posNoZ);
			
			interpVelocity = targetDirection.magnitude * 5f;
			
			targetPos = transform.position + (targetDirection.normalized * interpVelocity * Time.deltaTime); 
			
			transform.position = Vector3.Lerp( transform.position, targetPos + offset, 0.25f);


			float xPos = Mathf.Clamp(transform.position.x, minX, maxX);
			float yPos = Mathf.Clamp(transform.position.y, minY, maxY);
			float zPos = -10f;
			transform.position = new Vector3 (xPos, yPos, zPos);
		}
	}


	
	// Update is called once per frame
	/*void Update () {
		//transform.position = player.transform.position;
		if (player != null) {
			float playerX = player.transform.position.x;
			float playerY = player.transform.position.y;
			float xPos = Mathf.Clamp(playerX, minX, maxX);
			float yPos = Mathf.Clamp(playerY, minY, maxY);
			float zPos = -10f;
			transform.position = new Vector3 (xPos, yPos, zPos);
			//Debug.Log (transform.position);
		}

	}*/

	public static void UpdateBoundary(float newCamSize)
	{
		minY = -(4.4f - newCamSize);
		maxY = 5.9f - newCamSize;
		//minX = -(4.7f - newCamSize);
		//maxX = 4.7f - newCamSize;
		minX = maxX = 0f;
	}
}

using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class OrthographicSizeController : MonoBehaviour
{

	public float resizeSpeed = 2.0f;
	public float camSize;

	void Start() {
		camSize = GetComponent<Camera>().orthographicSize;
		CameraController.UpdateBoundary(camSize);
	}

	void Update(){
		float moveH_left = CrossPlatformInputManager.GetAxis("Horizontal_Left");
		float moveV_left = CrossPlatformInputManager.GetAxis("Vertical_Left");
		float length_left = Mathf.Sqrt (moveH_left * moveH_left + moveV_left * moveV_left);
		float angle_left = Mathf.Atan2 (moveV_left, moveH_left);
		
		float moveH_right = CrossPlatformInputManager.GetAxis("Horizontal_Right");
		float moveV_right = CrossPlatformInputManager.GetAxis("Vertical_Right");
		float length_right = Mathf.Sqrt (moveH_right * moveH_right + moveV_right * moveV_right);
		float angle_right = Mathf.Atan2 (moveV_right, moveH_right);

		//Debug.Log ("Left "+length_left+" Right "+length_right);

		if((angle_left>Mathf.PI/2 || angle_left < -Mathf.PI/2) 
		   && length_left!=0f && length_right!=0f
		   && (angle_right > -Mathf.PI/2 && angle_right < Mathf.PI/2 ))
		{
			camSize = Mathf.Clamp(camSize - (length_left+length_right)/2 * resizeSpeed *Time.deltaTime, 2f, 2.5f) ;
			GetComponent<Camera>().orthographicSize  = camSize;
			CameraController.UpdateBoundary(camSize);
		}
		else if((angle_right>Mathf.PI/2 || angle_right < -Mathf.PI/2) 
		        && length_left!=0f && length_right!=0f
		        && (angle_left > -Mathf.PI/2 && angle_left < Mathf.PI/2 ))
		{
			camSize = Mathf.Clamp(camSize + (length_left+length_right)/2 * resizeSpeed *Time.deltaTime, 2f, 2.5f) ;
			GetComponent<Camera>().orthographicSize = camSize;
			CameraController.UpdateBoundary(camSize);
		}
	}
	
}

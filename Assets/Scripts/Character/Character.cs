using UnityEngine;
using System.Collections;

[System.Serializable]
public class Boundary {
	public float xMin, xMax, yMin, yMax;
}
public class Character : MonoBehaviour {

	public int HP;
	public Vector2 pos;
	public Vector2 dir;
	public float speed;
	public bool isMoving;
	public Boundary boundary;


	// Use this for initialization
	void Start () {

	}

	public IEnumerator MoveByVector(Vector2 offset) {
		isMoving = true;

		dir = offset;
		Rotate (dir);

		float desX = transform.position.x + offset.x * Map.unitCell;
		float desY = transform.position.y + offset.y * Map.unitCell;
		Vector3 next = new Vector3
		(
			Mathf.Clamp(desX, boundary.xMin, boundary.xMax),
			Mathf.Clamp(desY, boundary.yMin, boundary.yMax),
			0.0f
		);

		while (transform.position != next){
			transform.position = Vector3.MoveTowards(transform.position, next, speed * Time.deltaTime);
			yield return null;
		}

		pos = new Vector2(Mathf.Clamp(pos.x+offset.x, 1, 6),Mathf.Clamp(pos.y+offset.y, 1, 6));

		isMoving = false;
	}

	public void MoveToPoint(Vector2 des) {
	
	}

	public void Rotate(Vector2 a) {
		float angle;
		angle = a.x==0? (90*a.y):(90 - 90*a.x + 45*a.x*a.y);
		transform.rotation = Quaternion.Euler (0.0f, 0.0f, angle);
	}
	

}

using UnityEngine;
using System.Collections;

[System.Serializable]
public class IntVector2{
	public int x, y;
	public IntVector2(int _x, int _y) {
		x = _x;
		y = _y;
	}
	public static bool operator ==(IntVector2 a, IntVector2 b) {
		// If both are null, or both are same instance, return true.
		if (System.Object.ReferenceEquals(a, b)) {
			return true;
		}
		// If one is null, but not both, return false.
		if (((object)a == null) || ((object)b == null)) {
			return false;
		}
		// Return true if the fields match:
		return a.x == b.x && a.y == b.y;
	}
	public static bool operator !=(IntVector2 a, IntVector2 b) {
		return !(a == b);
	}
	public static IntVector2 operator +(IntVector2 a, IntVector2 b) {
		return new IntVector2 (a.x + b.x, a.y + b.y);
	}
}
public class Character : MonoBehaviour {

	public int HP;
	public IntVector2 pos;
	public IntVector2 dir;
	public float speed;
	public bool isMoving;
	
	// Use this for initialization
	void Start () {

	}

	public Character(){
	}

	public IEnumerator MoveByVector(IntVector2 offset) {

		IntVector2 newPos = Map.BoundPos(pos+offset);

		// If there are things in front of the character, break
		if (Map.mainMap [newPos.x, newPos.y].Count != 0)
			yield break;

		isMoving = true;
		float desX = transform.position.x + (float)offset.x * Map.unitCell;
		float desY = transform.position.y + (float)offset.y * Map.unitCell;
		Vector3 next = new Vector3
		(
			Mathf.Clamp(desX, Boundary.xMin, Boundary.xMax),
			Mathf.Clamp(desY, Boundary.yMin, Boundary.yMax),
			0.0f
		);

		while (transform.position != next) {
			transform.position = Vector3.MoveTowards(transform.position, next, speed * Time.deltaTime);
			yield return null;
		}

		IntVector2 pre = new IntVector2(pos.x, pos.y);
		pos = newPos; 
		Map.UpdatePos (this, pre);

		isMoving = false;
	}

	public void MoveToPoint(IntVector2 des) {
	
	}

	public void Rotate(IntVector2 a) {

		dir = a;

		int angle;
		angle = a.x==0? (90*a.y):(90 - 90*a.x + 45*a.x*a.y);
		transform.rotation = Quaternion.Euler (0.0f, 0.0f, (float)angle);
	}
	

}

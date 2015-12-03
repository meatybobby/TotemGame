using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
	public static IntVector2 operator -(IntVector2 a, IntVector2 b) {
		return new IntVector2 (a.x - b.x, a.y - b.y);
	}
	public static IntVector2 operator -(IntVector2 a) {
		return new IntVector2 (-a.x, -a.y);
	}
}
public class Character : MonoBehaviour {
	
	public int HP;
	public IntVector2 pos;
	public IntVector2 dir;
	public float speed;
	public bool isMoving;
	public bool inMoveThread;
	
	// Use this for initialization
	void Start () {
		
	}
	
	public Character(){
	}
	
	
	// Move the character according to the vecList, 
	// it will ignore the barriers on the map when moving
	public IEnumerator MoveByVectorArray(List<IntVector2> vecList, float newSpeed) {
		float tempSpeed = speed;
		speed = newSpeed;
		foreach (IntVector2 vec in vecList) {
			
			while (inMoveThread)
				yield return new WaitForSeconds (0.01f);
			// rotate the character when moving toward different direction
			if(vec != dir) {
				Rotate (vec);
			}
			MoveByVector(vec);
		}
		speed = tempSpeed;
	}
	
	
	public void MoveByVector(IntVector2 offset) {
		
		IntVector2 newPos; // the new pos after being moved
		newPos = Map.BoundPos(pos+offset);                
		
		//if (Map.mainMap [newPos.x, newPos.y].Count != 0)
		//return;
		
		// Update the main-map position first
		IntVector2 pre = new IntVector2(pos.x, pos.y);
		pos = newPos; 
		Map.UpdatePos (this, pre);
		
		Vector3 next = Map.GetRealPosition(newPos, this);
		StartCoroutine(MoveThread (next));
	}
	
	private IEnumerator MoveThread(Vector3 next) {
		bool playerCatch = false;
		Player p;
		if (this.GetType () == typeof(Player)) {
			p = this as Player;
			if(p.mode==Player.CATCH) {
				playerCatch = true;
				p.caughtTotem.inMoveThread = true;
			}
		}
		
		// 往下走，z值先更新 （解決斜走重疊的問題）
		if (transform.position.y > next.y) {
			transform.position = new Vector3(transform.position.x, transform.position.y, next.z);
		}
		Vector3 nextXY = new Vector3 (next.x, next.y, transform.position.z); // 不要移動z
		inMoveThread = true;
		while (transform.position != nextXY) {
			transform.position = Vector3.MoveTowards(transform.position, nextXY, speed * Time.deltaTime);
			yield return null;
		}
		inMoveThread = false;
		transform.position = next;//new Vector3( next.x, next.y, next.z);
		
		
		if (playerCatch) {
			p = this as Player;
			p.caughtTotem.inMoveThread = false;
		}
	}
	
	public void MoveToPoint(IntVector2 des) {
		
	}
	
	public void Rotate(IntVector2 a) {
		
		dir = a;
		
		int angle;
		angle = a.x==0? (90*a.y):(90 - 90*a.x + 45*a.x*a.y);
		
		transform.rotation = Quaternion.Euler (0.0f, 0.0f, (float)angle+90.0f);
	}
	
	public int getDistance(Character c) {
		return (pos.x - c.pos.x) * (pos.x - c.pos.x) + (pos.y - c.pos.y) * (pos.y - c.pos.y);
	}
	
}
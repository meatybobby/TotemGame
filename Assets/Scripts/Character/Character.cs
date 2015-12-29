using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

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
	public override string ToString ()
	{
		return string.Format ("(" + x + "," + y + ")");
	}
}
public class Character : MonoBehaviour {
	
	public int HP;
	public int maxHP;
	public int damage;
	public IntVector2 pos;
	public IntVector2 dir;
	public float speed;
	public bool isMoving;
	public bool inMoveThread;
	public int characterId;
	public bool isDead;
	public Texture frame;
	public GameObject healEffect;

	//HP GUI
	public GameObject healthPrefab;
	public float healthPanelOffset;
	protected Canvas canvas;
	protected GameObject healthPanel;
	protected Slider healthSlider;

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
		Vector3 next = Map.GetRealPosition(newPos, this.GetType());
		StartCoroutine(MoveThread (next));
	}
	
	protected IEnumerator MoveThread(Vector3 next) {
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
	
	public void HealHP(int healPoint) {
		//if( HP < maxHP )
			StartCoroutine (HealEffect());
		HP = Mathf.Clamp (HP + healPoint, 0, maxHP);
	}

	protected IEnumerator HealEffect() {
		yield return new WaitForSeconds (0.25f);
		healEffect.SetActive (true);
		yield return new WaitForSeconds (1f);
		healEffect.SetActive (false);
	}

	public void CauseDamage(int harm){
		HP = HP - harm;
		StartCoroutine (FlashRed());
	}

	protected IEnumerator FlashRed() {
		for (int i=0; i<1; i++) {
			// If the Player is in CATCH mode, the caughtTotem can't be flashed
			if(this.gameObject.tag=="Player") {
				SpriteRenderer sp = GetComponent<SpriteRenderer>() as SpriteRenderer;
				if(sp!=null) sp.color = new Color (1f, 0.7f, 0.7f);
				yield return new WaitForSeconds(0.2f);
				if(sp!=null) sp.color = new Color (1f, 1f, 1f);
				yield return new WaitForSeconds(0.2f);
			}
			else {
				SpriteRenderer[] sps;
				sps = GetComponentsInChildren<SpriteRenderer>() as SpriteRenderer[];
				foreach(SpriteRenderer sp in sps){
					if(sp!=null && sp.gameObject.tag!="Range") sp.color = new Color (1f, 0.7f, 0.7f);
				}
				yield return new WaitForSeconds(0.2f);
				foreach(SpriteRenderer sp in sps){
					if(sp!=null && sp.gameObject.tag!="Range") sp.color = new Color (1f, 1f, 1f);
				}
				yield return new WaitForSeconds(0.2f);
			}

			
		}
	}

	protected void HpInitialize(){
		canvas = GameObject.FindGameObjectWithTag ("Canvas").GetComponent<Canvas>();
		HP = maxHP;
		healthPanel = Instantiate(healthPrefab) as GameObject;
		healthPanel.transform.SetParent(canvas.transform, false);
		healthSlider = healthPanel.GetComponentInChildren<Slider>();
	}

	protected void HpUpdate(){
		healthSlider.value = HP /(float) maxHP;
		Vector3 worldPos = new Vector3(transform.position.x, transform.position.y + healthPanelOffset, transform.position.z);
		Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
		healthPanel.transform.position = new Vector3(screenPos.x, screenPos.y, screenPos.z);
	}

	/*
	void OnGUI(){
		Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
		if (this.gameObject.tag != "Rock") {
			// draw health bar background
			GUI.color = new Color(0.7f , 0.7f , 0.7f , 0.3f) ;
			GUI.DrawTexture (new Rect(pos.x-26, Screen.height - pos.y + 30, 52, 7), frame);
			
			// draw health bar amount
			GUI.color = new Color(0f , 0.8f , 0f , 0.5f) ;
			GUI.DrawTexture (new Rect(pos.x-25, Screen.height - pos.y + 31, 50f * (float)HP/maxHP, 5), frame);	
		}

	}*/
}
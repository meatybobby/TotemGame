using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Soomla.Levelup;

public class LevelController : MonoBehaviour {
	
	public static readonly string lastLevelKey = "LastPlayedLevel";
	public List<Vector3> LevelPosList;
	public GameObject player;
	public float time;

	public GameObject btnColliders, mapUI;
	
	private bool moving;
	private int lastLevel;
	private int nowLevel;

	public Collider2D[] colliders;




	
	// Use this for initialization
	void Start () {
		//PlayerPrefs.DeleteAll(); // use this to delete all soomla's save menu
		int lastLevel = PlayerPrefs.GetInt(lastLevelKey);
		if(lastLevel == 0) lastLevel = 1;
		nowLevel = lastLevel;
		player.transform.position = LevelPosList [lastLevel];
		moving = false;

		colliders = btnColliders.GetComponentsInChildren<Collider2D>() as Collider2D[];
		SetMapUI();
	}
	void Update() {
		//Debug.Log (Input.GetMouseButtonDown(0));
		//if(Input.touchCount==1) {
		if(Input.GetMouseButtonDown(0)) {
			Vector3 wp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		//	Vector3 wp = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);

			Vector2 touchPos = new Vector2(wp.x, wp.y);

			for(int i=0; i<colliders.Length; i++) {
				if(colliders[i].OverlapPoint(touchPos)) {
					//Debug.Log ("touching!!!"+i);
					ChooseLevel(i+1);
					break;
				}
			}

		}
	}

	public void SetMapUI() {

		List<Level> levelList = ApplicationModel.levelList;
		GameObject levelObj;
		for(int i=0; i<levelList.Count; i++) {
			levelObj = mapUI.transform.Find("Level " +(i+1)).gameObject;
			if(levelList[i].IsCompleted()) {
				levelObj.transform.Find("lock").gameObject.SetActive(false);
				levelObj.transform.Find("flag").gameObject.SetActive(true);
			}
			else if(!ApplicationModel.levelLockMap[i]) {
				levelObj.transform.Find("lock").gameObject.SetActive(true);
				levelObj.transform.Find("flag").gameObject.SetActive(false);
			}
			else {
				levelObj.transform.Find("lock").gameObject.SetActive(false);
				levelObj.transform.Find("flag").gameObject.SetActive(false);
			}

		}
	}

	public void ChooseLevel(int num){
		if(!moving && ApplicationModel.levelLockMap[num-1])
			StartCoroutine (MovePlayer (num));
	}
	
	public IEnumerator MovePlayer(int toPos) {
		int numPath = Mathf.Abs( toPos - nowLevel);
		while (nowLevel != toPos) {
			while(moving)
				yield return new WaitForSeconds (0.01f);
			if(nowLevel > toPos)
				nowLevel--;
			else
				nowLevel++;
			moving = true;
			StartCoroutine(MoveThroughLevel(LevelPosList[nowLevel],time/numPath));
		}
		while(moving)
			yield return new WaitForSeconds (0.01f);

		ApplicationModel.SetCurrentLevel(toPos);

		PlayerPrefs.SetInt(lastLevelKey, toPos);
		Application.LoadLevel ("Game");

	}
	
	private IEnumerator MoveThroughLevel(Vector3 nextPos,float time){
		float onePace = (nextPos - player.transform.position).magnitude * Time.deltaTime / time;
		while (player.transform.position != nextPos) {
			player.transform.position = Vector3.MoveTowards(player.transform.position, nextPos , onePace);
			yield return null;
		}
		moving = false;
	}
}

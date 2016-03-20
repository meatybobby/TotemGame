using UnityEngine;
using System.Collections;

public class BGM : MonoBehaviour {

	public GameObject musicPlayer;
	void Awake() {
		DontDestroyOnLoad(this.gameObject);
	}


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

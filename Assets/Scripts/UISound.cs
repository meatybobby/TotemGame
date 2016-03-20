using UnityEngine;
using System.Collections;

public class UISound : MonoBehaviour {

	public AudioSource myAudio;


	// Use this for initialization
	void Start () {
		myAudio = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void PlaySound() {
		myAudio.Play ();
	}
}

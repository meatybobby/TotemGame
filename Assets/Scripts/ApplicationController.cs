using UnityEngine;
using System.Collections;

public class ApplicationController : MonoBehaviour {
	
	public GameObject fader;
	private FadeEffect fadeEffect;
	
	public void EnterSelectLevelMap() {
		fadeEffect = fader.GetComponent<FadeEffect> ();
		fadeEffect.FadeOut ();
		StartCoroutine (WaitForFadeOut());
	}
	
	private IEnumerator WaitForFadeOut() {
		while (!fadeEffect.fadeOutFinish) {
			yield return new WaitForSeconds (0.01f);
		}
		Application.LoadLevel ("LevelSelect");
	}
}

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FadeEffect : MonoBehaviour
{
	//UI fade in
	public GameObject fadeIn;
	public GameObject fadeOut;
	public float fadeSpeed;
	public bool fadeInFinish;
	public bool fadeOutFinish;
	
	public void FadeIn ()
	{
		fadeIn.SetActive (true);
		fadeIn.GetComponent<RectTransform> ().sizeDelta = new Vector2 (Screen.width,Screen.height);
		fadeInFinish = false;
		StartCoroutine (FadeInEvent ());
	}

	private IEnumerator FadeInEvent ()
	{
		Color targetColor = new Color (
			fadeIn.GetComponent<Image> ().color.r,
			fadeIn.GetComponent<Image> ().color.g,
			fadeIn.GetComponent<Image> ().color.b,
			0.0f
		);
		while (fadeIn.GetComponent<Image>().color.a > 0.05f) {
			fadeIn.GetComponent<Image> ().color = Color.Lerp (fadeIn.GetComponent<Image> ().color,targetColor , fadeSpeed * Time.deltaTime);
			yield return null;
		}
		fadeIn.GetComponent<Image> ().color = targetColor;
		fadeIn.SetActive (false);
		fadeInFinish = true;
	}

	public void FadeOut ()
	{
		fadeOutFinish = false;
		fadeOut.GetComponent<RectTransform> ().sizeDelta = new Vector2 (Screen.width,Screen.height);
		fadeOut.SetActive (true);
		StartCoroutine (FadeOutEvent ());
	}

	private IEnumerator FadeOutEvent ()
	{
		Color targetColor = new Color (
			fadeOut.GetComponent<Image> ().color.r,
			fadeOut.GetComponent<Image> ().color.g,
			fadeOut.GetComponent<Image> ().color.b,
			1.0f
		);
		while (fadeOut.GetComponent<Image>().color.a < 0.95f) {
			fadeOut.GetComponent<Image> ().color = Color.Lerp (fadeOut.GetComponent<Image> ().color, targetColor, fadeSpeed * Time.deltaTime);
			yield return null;
		}
		fadeOut.GetComponent<Image> ().color = Color.black;
		fadeOutFinish = true;
	}
}

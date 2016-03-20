using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class UIController : MonoBehaviour {
	public GameObject menu;
	public GameObject UICanvas;
	public GameObject backpackView;


	public float extendSpeed;
	public Sprite backpackOpen;
	public Sprite backpackClose;

	public GameObject extendButton;
	private RectTransform rectBackpack;
	private RectTransform rectExtendButton;
	private Button[] buttons;

	public bool backpackState;

	private Vector2 backpackSize;

	private bool backpackChaning = false;

	// Use this for initialization
	void Start () {
		buttons = UICanvas.GetComponentsInChildren<Button> ();
	
		backpackState = true;
		rectBackpack = backpackView.GetComponent<RectTransform> ();
		backpackSize = rectBackpack.sizeDelta;
		rectExtendButton = extendButton.GetComponent<RectTransform> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void DisplayMenu() {
		Time.timeScale = 0;
		//GameController.Pause();
		for (int i=0; i<buttons.Length; i++)
			buttons [i].interactable = false;
	
		menu.SetActive (true);
	}

	public void CloseMenu() {
		menu.SetActive (false);
		for (int i=0; i<buttons.Length; i++)
			buttons [i].interactable = true;
	
		Time.timeScale = 1;
	}





	public void ExtendBackpack(){
		Image image = extendButton.GetComponent<Image> ();
		if (backpackState == false && !backpackChaning) {
			image.sprite = backpackOpen;
			backpackState = true;
			backpackView.SetActive(true);
			/*backpackChaning = true;
			StartCoroutine(ExtendScrollView(backpackSize,backpackState));*/
		} else if(!backpackChaning){
			image.sprite = backpackClose;
			backpackState = false;
			backpackView.SetActive(false);
			/*backpackChaning = true;
			StartCoroutine(ExtendScrollView(new Vector2(0,backpackSize.y),backpackState));*/
		}
	}
	private IEnumerator ExtendScrollView(Vector2 target,bool backpackState){
		Vector2 directionVector = new Vector2(target.x - rectBackpack.sizeDelta.x, target.y - rectBackpack.sizeDelta.y).normalized;
		Image image = extendButton.GetComponent<Image> ();
		if (backpackState == true){
			image.sprite = backpackOpen;
			backpackView.SetActive(true);
		}
		while(rectBackpack.sizeDelta != target){
			rectBackpack.sizeDelta = Vector2.MoveTowards(rectBackpack.sizeDelta, target, extendSpeed * Time.deltaTime);
			yield return null;
		}
		if(backpackState == false){
			image.sprite = backpackClose;
			backpackView.SetActive(false);
		}
		backpackChaning = false;
	}
}

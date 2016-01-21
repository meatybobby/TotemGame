using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace UnityStandardAssets.CrossPlatformInput
{
	public class UIController : MonoBehaviour {
		public GameObject menu;
		public GameObject UICanvas;
		public GameObject backpackView;
		public float extendSpeed;
		public Sprite backpackOpen;
		public Sprite backpackClose;

		public Button extendButton;
		private RectTransform rectBackpack;
		private RectTransform rectExtendButton;
		private Button[] buttons;
		private Joystick joystick;
		private bool backpackState;

		// Use this for initialization
		void Start () {
			buttons = UICanvas.GetComponentsInChildren<Button> ();
			joystick = UICanvas.GetComponentInChildren<Joystick> ();
			backpackState = true;
			rectBackpack = backpackView.GetComponent<RectTransform> ();
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
			joystick.enabled = false;
			menu.SetActive (true);
		}

		public void CloseMenu() {
			menu.SetActive (false);
			for (int i=0; i<buttons.Length; i++)
				buttons [i].interactable = true;
			joystick.enabled = true;
			Time.timeScale = 1;
		}





		public void ExtendBackpack(){
			if (backpackState == false) {
				backpackState = true;
				StartCoroutine(ExtendScrollView(new Vector2(300,100),backpackState));
			} else {
				backpackState = false;
				StartCoroutine(ExtendScrollView(new Vector2(1,100),backpackState));
			}
		}
		private IEnumerator ExtendScrollView(Vector2 target,bool backpackState){
			Vector2 directionVector = new Vector2(target.x - rectBackpack.sizeDelta.x, target.y - rectBackpack.sizeDelta.y).normalized;
			Image image = extendButton.GetComponent<Image> ();
			if (backpackState == true)
				image.sprite = backpackOpen;
			while(rectBackpack.sizeDelta != target){
				rectBackpack.sizeDelta = Vector2.MoveTowards(rectBackpack.sizeDelta, target, extendSpeed * Time.deltaTime);
				yield return null;
			}
			if(backpackState == false)
				image.sprite = backpackClose;
		}
	}
}
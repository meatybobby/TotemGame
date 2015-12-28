using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace UnityStandardAssets.CrossPlatformInput
{
	public class UIController : MonoBehaviour {
		public GameObject menu;
		public GameObject UICanvas;
		private Button[] buttons;
		private Joystick joystick;

		// Use this for initialization
		void Start () {
			buttons = UICanvas.GetComponentsInChildren<Button> ();
			joystick = UICanvas.GetComponentInChildren<Joystick> ();
		}
		
		// Update is called once per frame
		void Update () {
		
		}

		public void DisplayMenu() {
			Time.timeScale = 0;
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
	}
}
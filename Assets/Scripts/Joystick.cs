using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UnityStandardAssets.CrossPlatformInput
{
	public class Joystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
	{
		public enum AxisOption
		{
			// Options for which axes to use
			Both, // Use both
			OnlyHorizontal, // Only horizontal
			OnlyVertical // Only vertical
		}
		//own edition begin
		public GameObject reference_left;
		public GameObject stick_left;
		public GameObject reference_right;
		public GameObject stick_right;
		//own edition finish
		public int MovementRange = 100;
		public AxisOption axesToUse = AxisOption.Both; // The options for the axes that the still will use
		public string horizontalAxisNameLeft = "Horizontal_Left"; // The name given to the horizontal axis for the cross platform input
		public string verticalAxisNameLeft = "Vertical_Left"; // The name given to the vertical axis for the cross platform input
		public string horizontalAxisNameRight = "Horizontal_Right"; // The name given to the horizontal axis for the cross platform input
		public string verticalAxisNameRight = "Vertical_Right"; // The name given to the vertical axis for the cross platform input

		Vector3 m_StartPos;
		bool m_UseX; // Toggle for using the x axis
		bool m_UseY; // Toggle for using the Y axis
		CrossPlatformInputManager.VirtualAxis m_HorizontalVirtualAxis; // Reference to the joystick in the cross platform input
		CrossPlatformInputManager.VirtualAxis m_VerticalVirtualAxis; // Reference to the joystick in the cross platform input

		void OnEnable()
		{
			CreateVirtualAxes();
		}

        void Start()
        {
            m_StartPos = transform.position;
        }

		void UpdateVirtualAxes(Vector3 value)
		{
			var delta = m_StartPos - value;
			delta.y = -delta.y;
			delta /= MovementRange;
			if (m_UseX)
			{
				m_HorizontalVirtualAxis.Update(-delta.x);
			}

			if (m_UseY)
			{
				m_VerticalVirtualAxis.Update(delta.y);
			}
		}

		void CreateVirtualAxes()
		{
			// set axes to use
			m_UseX = (axesToUse == AxisOption.Both || axesToUse == AxisOption.OnlyHorizontal);
			m_UseY = (axesToUse == AxisOption.Both || axesToUse == AxisOption.OnlyVertical);

			// create new axes based on axes to use
			if (m_UseX)
			{
				if(this.gameObject.name == "JoystickObject")
				{
					if (CrossPlatformInputManager.AxisExists(horizontalAxisNameLeft))
					{
						CrossPlatformInputManager.UnRegisterVirtualAxis(horizontalAxisNameLeft);
					}
					m_HorizontalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(horizontalAxisNameLeft);
					CrossPlatformInputManager.RegisterVirtualAxis(m_HorizontalVirtualAxis);
				}
				else
				{
					if (CrossPlatformInputManager.AxisExists(horizontalAxisNameRight))
					{
						CrossPlatformInputManager.UnRegisterVirtualAxis(horizontalAxisNameRight);
					}
					m_HorizontalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(horizontalAxisNameRight);
					CrossPlatformInputManager.RegisterVirtualAxis(m_HorizontalVirtualAxis);
				}
			}
			if (m_UseY)
			{
				if(this.gameObject.name == "JoystickObject")
				{
					if (CrossPlatformInputManager.AxisExists(verticalAxisNameLeft))
					{
						CrossPlatformInputManager.UnRegisterVirtualAxis(verticalAxisNameLeft);
					}
					m_VerticalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(verticalAxisNameLeft);
					CrossPlatformInputManager.RegisterVirtualAxis(m_VerticalVirtualAxis);
				}
				else
				{
					if (CrossPlatformInputManager.AxisExists(verticalAxisNameRight))
					{
						CrossPlatformInputManager.UnRegisterVirtualAxis(verticalAxisNameRight);
					}
					m_VerticalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(verticalAxisNameRight);
					CrossPlatformInputManager.RegisterVirtualAxis(m_VerticalVirtualAxis);
				}
			}
		}


		public void OnDrag(PointerEventData data)
		{
			Vector3 newPos = Vector3.zero;

			if (m_UseX) {
				int delta = (int)(data.position.x - m_StartPos.x);
				//delta = Mathf.Clamp(delta, - MovementRange, MovementRange); //standard
				newPos.x = delta;
			}

			if (m_UseY) {
				int delta = (int)(data.position.y - m_StartPos.y);
				//delta = Mathf.Clamp(delta, -MovementRange, MovementRange); //standard
				newPos.y = delta;
			}
			//transform.position = new Vector3 (m_StartPos.x + newPos.x, m_StartPos.y + newPos.y, m_StartPos.z + newPos.z); //standard
			//transform.position = Vector3.ClampMagnitude (new Vector3 (newPos.x, newPos.y, newPos.z), MovementRange) + m_StartPos; //own edition

			if (this.gameObject.name == "JoystickObject") {
				stick_left.transform.position = /*Vector3.ClampMagnitude (*/new Vector3 (newPos.x, newPos.y, newPos.z)/*, MovementRange)*/ + m_StartPos;//own edition
			} 
			else {
				stick_right.transform.position = /*Vector3.ClampMagnitude (*/new Vector3 (newPos.x, newPos.y, newPos.z)/*, MovementRange)*/ + m_StartPos;//own edition
			}
			UpdateVirtualAxes( Vector3.ClampMagnitude (new Vector3 (newPos.x, newPos.y, newPos.z), MovementRange) + m_StartPos);
			if(new Vector3 (newPos.x, newPos.y, newPos.z).magnitude > Vector3.ClampMagnitude (new Vector3 (newPos.x, newPos.y, newPos.z), MovementRange).magnitude){
				m_StartPos += new Vector3 (newPos.x, newPos.y, newPos.z) - Vector3.ClampMagnitude (new Vector3 (newPos.x, newPos.y, newPos.z), MovementRange);
				if(this.gameObject.name == "JoystickObject"){
					reference_left.transform.position = m_StartPos;
				}
				else{
					reference_right.transform.position = m_StartPos;
				}
			}
		}


		public void OnPointerUp(PointerEventData data)
		{
			//transform.position = m_StartPos;
			UpdateVirtualAxes(m_StartPos);
			if (this.gameObject.name == "JoystickObject") {
				stick_left.SetActive (false);  //own edition
				reference_left.SetActive (false);  //own edition
			} 
			else {
				stick_right.SetActive (false);  //own edition
				reference_right.SetActive (false);  //own edition
			}
		}


		public void OnPointerDown(PointerEventData data) 
		{
			//own edition
			if (this.gameObject.name == "JoystickObject") {
				stick_left.SetActive (true);
				reference_left.SetActive (true);
				stick_left.transform.position = new Vector3 (data.position.x, data.position.y, 0);
				reference_left.transform.position = new Vector3 (data.position.x, data.position.y, 0);
				m_StartPos = reference_left.transform.position;
			}
			else {
				stick_right.SetActive (true);
				reference_right.SetActive (true);
				stick_right.transform.position = new Vector3 (data.position.x, data.position.y, 0);
				reference_right.transform.position = new Vector3 (data.position.x, data.position.y, 0);
				m_StartPos = reference_right.transform.position;
			}
		}

		void OnDisable()
		{
			// remove the joysticks from the cross platform input
			if (m_UseX)
			{
				m_HorizontalVirtualAxis.Remove();
			}
			if (m_UseY)
			{
				m_VerticalVirtualAxis.Remove();
			}
		}
	}
}
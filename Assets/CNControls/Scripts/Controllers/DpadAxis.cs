using UnityEngine;
using System.Collections;

namespace CnControls
{
    public class DpadAxis : MonoBehaviour
    {
        public string AxisName;
        public float AxisMultiplier;
		public float speed = 2.0f;

        public RectTransform RectTransform { get; private set; }
        public int LastFingerId { get; set; }
        private VirtualAxis _virtualAxis;
		private bool pressed;

        private void Awake()
        {
            RectTransform = GetComponent<RectTransform>();
        }

        private void OnEnable()
        {
            _virtualAxis = _virtualAxis ?? new VirtualAxis(AxisName);
            LastFingerId = -1;

            CnInputManager.RegisterVirtualAxis(_virtualAxis);
        }

        private void OnDisable()
        {
            CnInputManager.UnregisterVirtualAxis(_virtualAxis);
        }

        public void Press(Vector2 screenPoint, Camera eventCamera, int pointerId)
        {
			pressed = true;
			StartCoroutine (AxisAdder());
            LastFingerId = pointerId;
        }

        public void TryRelease(int pointerId)
        {
            if (LastFingerId == pointerId)
            {
				pressed = false;
                _virtualAxis.Value = 0f;
                LastFingerId = -1;
            }
        }

		protected IEnumerator AxisAdder(){
			while(pressed){
				_virtualAxis.Value = Mathf.Clamp(_virtualAxis.Value + AxisMultiplier * Time.deltaTime * speed, -1f, 1f);
				yield return null;
			}
		}
    }
}
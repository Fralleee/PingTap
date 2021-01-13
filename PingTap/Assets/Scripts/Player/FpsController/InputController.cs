using Fralle.Core.Enums;
using UnityEngine;

namespace Fralle.FpsController
{
	public class InputController : MonoBehaviour
	{
		[HideInInspector] public Vector2 Move;
		[HideInInspector] public Vector2 Mouse;
		[HideInInspector] public Vector2 MouseRaw;

		[HideInInspector] public bool JumpButtonDown;
		[HideInInspector] public bool JumpButtonHold;

		[HideInInspector] public bool DashButtonDown;
		[HideInInspector] public bool DashButtonHold;
		[HideInInspector] public bool DashButtonUp;

		[HideInInspector] public bool CrouchButtonHold;

		[HideInInspector] public bool Mouse1ButtonDown;
		[HideInInspector] public bool Mouse1ButtonHold;
		[HideInInspector] public bool Mouse1ButtonUp;

		[HideInInspector] public bool Mouse2ButtonDown;
		[HideInInspector] public bool Mouse2ButtonHold;
		[HideInInspector] public bool Mouse2ButtonUp;

		public float MouseSensitivity = 1f;
		public float MouseZoomModifier = 0.5f;

		float currentSensitivity = 1f;

		void Awake()
		{
			currentSensitivity = MouseSensitivity;
		}

		void Update()
		{
			GatherInputs();
		}

		void LateUpdate()
		{
			GatherCameraInputs();
		}

		void GatherInputs()
		{
			Move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

			JumpButtonDown = Input.GetButtonDown("Jump");
			JumpButtonHold = Input.GetButton("Jump");
			DashButtonDown = Input.GetKeyDown(KeyCode.LeftShift);
			DashButtonHold = Input.GetKey(KeyCode.LeftShift);
			DashButtonUp = Input.GetKeyUp(KeyCode.LeftShift);
			CrouchButtonHold = Input.GetKey(KeyCode.LeftControl);

			Mouse1ButtonDown = Input.GetMouseButtonDown(0);
			Mouse1ButtonHold = Input.GetMouseButton(0);
			Mouse1ButtonUp = Input.GetMouseButtonUp(0);

			Mouse2ButtonDown = Input.GetMouseButtonDown(1);
			Mouse2ButtonHold = Input.GetMouseButton(1);
			Mouse2ButtonUp = Input.GetMouseButtonUp(1);
		}

		void GatherCameraInputs()
		{
			Mouse += new Vector2(Input.GetAxis("Mouse X") * Time.deltaTime * currentSensitivity, -Input.GetAxis("Mouse Y") * Time.deltaTime * currentSensitivity);
			MouseRaw = new Vector2(Input.GetAxis("Mouse X") * currentSensitivity, -Input.GetAxis("Mouse Y") * currentSensitivity);
		}

		public bool GetMouseButton(MouseButton button, MouseButtonState state)
		{
			switch (button)
			{
				case MouseButton.Left:
					switch (state)
					{
						case MouseButtonState.Up:
							return Mouse1ButtonUp;
						case MouseButtonState.Down:
							return Mouse1ButtonDown;
						case MouseButtonState.Hold:
							return Mouse1ButtonHold;
					}

					break;
				case MouseButton.Right:
					switch (state)
					{
						case MouseButtonState.Up:
							return Mouse2ButtonUp;
						case MouseButtonState.Down:
							return Mouse2ButtonDown;
						case MouseButtonState.Hold:
							return Mouse2ButtonHold;
					}

					break;
			}

			return false;
		}

		public bool GetKeyDown(string key) => Input.GetKeyDown(key);

		public void Lock(bool doLock)
		{
			enabled = !doLock;
			Move = Vector2.zero;
			JumpButtonDown = false;
			JumpButtonHold = false;
			DashButtonDown = false;
			DashButtonHold = false;
			DashButtonUp = false;
			Mouse1ButtonDown = false;
			Mouse1ButtonHold = false;
			Mouse1ButtonUp = false;
			Mouse2ButtonDown = false;
			Mouse2ButtonHold = false;
			Mouse2ButtonUp = false;
		}
	}
}

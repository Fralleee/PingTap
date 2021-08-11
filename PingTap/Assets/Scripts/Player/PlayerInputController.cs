using UnityEngine;
using UnityEngine.InputSystem;

namespace Fralle
{
	public class PlayerInputController : MonoBehaviour
	{
		PlayerInput playerInput;

		void Start()
		{
			playerInput = GetComponent<PlayerInput>();

			// Console
		}

		public static void ConfigureCursor(bool doLock = true)
		{
			if (doLock)
			{
				Cursor.visible = false;
				Cursor.lockState = CursorLockMode.Locked;
			}
			else
			{
				Cursor.visible = true;
				Cursor.lockState = CursorLockMode.None;
			}
		}

		public static void ConfigureInput(bool disable = true)
		{
			if (disable)
				playerInput.DeactivateInput();
			else
				playerInput.ActivateInput();
		}
	}
}

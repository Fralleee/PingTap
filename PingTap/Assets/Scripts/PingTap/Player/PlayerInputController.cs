using Fralle.Gameplay;
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

			if (Managers.Instance.UiManager != null)
				Managers.Instance.UiManager.OnMenuToggle += OnMenuToggle;

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

		void OnMenuToggle(bool isOpen)
		{
			if (isOpen)
				playerInput.DeactivateInput();
			else
				playerInput.ActivateInput();
			ConfigureCursor(!isOpen);
		}

		void OnDestroy()
		{
			if (!Managers.Destroyed)
				Managers.Instance.UiManager.OnMenuToggle -= OnMenuToggle;
		}
	}
}

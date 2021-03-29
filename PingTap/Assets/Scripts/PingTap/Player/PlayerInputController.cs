using Fralle.Gameplay;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Fralle
{
	public class PlayerInputController : MonoBehaviour
	{
		PlayerInput playerInput;

		void Awake()
		{
			playerInput = GetComponent<PlayerInput>();

			Managers.Instance.UIManager.OnMenuToggle += OnMenuToggle;
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
			Managers.Instance.UIManager.OnMenuToggle -= OnMenuToggle;
		}
	}
}

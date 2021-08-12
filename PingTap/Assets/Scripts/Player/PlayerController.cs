using Fralle.FpsController;
using UnityEngine;
using UnityEngine.InputSystem;
using static PlayerControls;

namespace Fralle.PingTap
{
	public class PlayerController : RigidbodyController, IMovementActions
	{
		[HideInInspector] public PlayerControls controls;

		static PlayerController activeController;

		public static void Toggle(bool enabled)
		{
			if (enabled)
				activeController?.controls.Enable();
			else
				activeController?.controls.Disable();
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
				Cursor.lockState = CursorLockMode.Confined;
			}
		}

		protected override void Awake()
		{
			controls = new PlayerControls();
			controls.Movement.SetCallbacks(this);
			controls.Movement.Enable();

			activeController = this;

			base.Awake();
		}

		protected override void FixedUpdate()
		{
			if (!controls.Movement.enabled)
			{
				MouseLook = Vector2.zero;
				Movement = Vector2.zero;
				jumpButton = false;
				crouchButton = false;
			}

			base.FixedUpdate();
		}

		public void OnMovement(InputAction.CallbackContext context)
		{
			Movement = context.ReadValue<Vector2>();

			if (animator)
			{
				animator.SetFloat(animHorizontal, Movement.x);
				animator.SetFloat(animVertical, Movement.y);
			}
		}

		public void OnLook(InputAction.CallbackContext context)
		{
			MouseLook = context.ReadValue<Vector2>();
		}

		public void OnJump(InputAction.CallbackContext context)
		{
			jumpButton = context.ReadValueAsButton();
		}

		public void OnCrouch(InputAction.CallbackContext context)
		{
			crouchButton = context.ReadValueAsButton();
		}
	}
}

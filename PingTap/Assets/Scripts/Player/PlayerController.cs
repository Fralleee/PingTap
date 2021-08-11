using Fralle.FpsController;
using UnityEngine;
using UnityEngine.InputSystem;
using static PlayerControls;

namespace Fralle.PingTap
{
	public class PlayerController : RigidbodyController, IMovementActions
	{
		PlayerControls controls;

		protected override void Awake()
		{
			controls = new PlayerControls();
			controls.Movement.SetCallbacks(this);
			controls.Movement.Enable();

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

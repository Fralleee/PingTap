using UnityEngine;

namespace Fralle.FpsController.Moves
{
	public class GravityAdjuster : MonoBehaviour
	{
		[SerializeField] float fallMultiplier = 2.5f;
		[SerializeField] float lowJumpModifier = 2f;

		PlayerController controller;

		Rigidbody rigidBody;

		void Awake()
		{
			controller = GetComponentInParent<PlayerController>();
			rigidBody = GetComponent<Rigidbody>();
		}

		public void ControlledFixedUpdate()
		{
			Adjust();
		}

		void Adjust()
		{
			if (controller.IsGrounded)
				return;

			if (rigidBody.velocity.y < 0)
				rigidBody.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
			else if (rigidBody.velocity.y > 0 && !controller.Input.JumpButtonHold)
				rigidBody.velocity += Vector3.up * Physics.gravity.y * (lowJumpModifier - 1) * Time.fixedDeltaTime;
		}
	}
}

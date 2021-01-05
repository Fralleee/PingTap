using System;
using UnityEngine;

namespace Fralle.FpsController.Moves
{
	public class GroundController : MonoBehaviour
	{
		public event Action<float> OnGroundEnter = delegate { };
		public event Action OnGroundLeave = delegate { };

		[SerializeField] float maxSlopeAngle = 35;
		[SerializeField] float maxWalkableSlopeAngle = 45;
		[SerializeField] float groundCheckDistance = 0.01f; // distance for checking if the controller is grounded ( 0.01f seems to work best for this )rigidBody
		[SerializeField] float stickToGroundHelperDistance = 0.5f; // stops the character

		PlayerController controller;

		Rigidbody rigidBody;
		CapsuleCollider capsule;

		void Awake()
		{
			controller = GetComponentInParent<PlayerController>();
			rigidBody = GetComponent<Rigidbody>();
			capsule = GetComponent<CapsuleCollider>();
		}

		public void GroundedCheck()
		{
			controller.PreviouslyGrounded = controller.IsGrounded;
			var distance = ((capsule.height / 2f) - capsule.radius) + groundCheckDistance;
			if (controller.PreviouslyGrounded)
				distance += capsule.height * controller.stepHeight;

			if (Physics.SphereCast(transform.position, capsule.radius, Vector3.down, out RaycastHit hitInfo, distance, Physics.AllLayers, QueryTriggerInteraction.Ignore))
			{
				controller.IsGrounded = true;
				controller.groundContactNormal = hitInfo.normal;
			}
			else
			{
				controller.IsGrounded = false;
				controller.groundContactNormal = Vector3.up;
			}

			if (controller.PreviouslyGrounded != controller.IsGrounded)
			{
				if (controller.IsGrounded)
				{
					OnGroundEnter(rigidBody.velocity.y);
					controller.IsJumping = false;
				}
				else
					OnGroundLeave();
			}
		}

		public void SlopeControl()
		{
			rigidBody.useGravity = true;
			if (controller.IsGrounded && rigidBody.velocity.y <= 0.2f)
			{
				var slopeAngle = Vector3.Angle(controller.groundContactNormal, Vector3.up);
				if (slopeAngle > maxWalkableSlopeAngle)
				{
					rigidBody.AddForce(Physics.gravity * 3f);
				}
				if (slopeAngle > maxSlopeAngle + 1f)
				{
					return;
				}

				rigidBody.useGravity = false;
				rigidBody.AddForce(-controller.groundContactNormal * 150f);
			}
		}

		public void StickToGroundHelper()
		{
			var distance = ((capsule.height / 2f) - capsule.radius) + stickToGroundHelperDistance;
			if (Physics.SphereCast(transform.position, capsule.radius, Vector3.down, out RaycastHit hitInfo, distance, Physics.AllLayers, QueryTriggerInteraction.Ignore))
			{
				if (Mathf.Abs(Vector3.Angle(hitInfo.normal, Vector3.up)) < maxWalkableSlopeAngle)
				{
					rigidBody.velocity = Vector3.ProjectOnPlane(rigidBody.velocity, hitInfo.normal);
				}
			}
		}

	}
}

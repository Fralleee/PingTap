using Fralle.Core.Extensions;
using UnityEngine;

namespace Fralle.FpsController.Moves
{
	public class AirMovement : MonoBehaviour
	{
		[SerializeField] float airControl = 0.5f;
		[SerializeField] float stopTime = 0.05f;

		PlayerController controller;
		Rigidbody rigidBody;
		Transform orientation;

		Vector3 damp;

		void Awake()
		{
			controller = GetComponentInParent<PlayerController>();
			rigidBody = GetComponent<Rigidbody>();
			orientation = transform.Find("Orientation");
		}

		public void Move()
		{
			var desiredForce = orientation.right * controller.Input.Move.x + orientation.forward * controller.Input.Move.y;
			rigidBody.AddForce(desiredForce * controller.forwardSpeed * airControl, ForceMode.Impulse);
			StoppingForces();
		}

		void StoppingForces()
		{
			rigidBody.velocity = Vector3.SmoothDamp(rigidBody.velocity, Vector3.zero, ref damp, stopTime).With(y: rigidBody.velocity.y);
		}
	}
}

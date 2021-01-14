using StatsSystem;
using UnityEngine;

namespace Fralle.FpsController.Moves
{
	public class Movement : MonoBehaviour
	{
		[SerializeField] float stopTime = 0.05f;

		PlayerController controller;
		Rigidbody rigidBody;
		Transform orientation;

		Vector3 damp;
		float runSpeedStatMultiplier = 1f;

		void Awake()
		{
			controller = GetComponentInParent<PlayerController>();
			rigidBody = GetComponent<Rigidbody>();
			orientation = transform.Find("Orientation");
		}

		void Start()
		{
			if (controller.Stats)
				controller.Stats.OnStatisticUpdated += OnStatisticUpdated;
		}

		public bool Move()
		{
			var desiredForce = orientation.right * controller.Input.Move.x + orientation.forward * controller.Input.Move.y;
			desiredForce = Vector3.ProjectOnPlane(desiredForce, controller.groundContactNormal).normalized;
			rigidBody.AddForce(desiredForce * controller.forwardSpeed * runSpeedStatMultiplier, ForceMode.Impulse);
			StoppingForces();
			return desiredForce.magnitude > 0;
		}

		void StoppingForces()
		{
			rigidBody.velocity = Vector3.SmoothDamp(rigidBody.velocity, Vector3.zero, ref damp, stopTime);
		}

		void OnStatisticUpdated(Stats stats)
		{
			runSpeedStatMultiplier = stats.runSpeedMultiplier;
		}

		void OnDestroy()
		{
			if (controller.Stats)
				controller.Stats.OnStatisticUpdated -= OnStatisticUpdated;
		}
	}
}

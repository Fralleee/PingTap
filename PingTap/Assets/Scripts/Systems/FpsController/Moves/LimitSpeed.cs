using StatsSystem;
using UnityEngine;

namespace Fralle.FpsController.Moves
{
	public class LimitSpeed : MonoBehaviour
	{
		[SerializeField] float maxSpeed = 7f;

		PlayerController controller;
		Rigidbody rigidBody;
		float runSpeedStatMultiplier = 1f;

		void Awake()
		{
			controller = GetComponentInParent<PlayerController>();
			rigidBody = GetComponent<Rigidbody>();
		}

		void Start()
		{
			if (controller.Stats)
				controller.Stats.OnStatisticUpdated += OnStatisticUpdated;
		}

		public void ControlledFixedUpdate()
		{
			Limit();
		}

		void Limit()
		{
			var horizontalMovement = new Vector3(rigidBody.velocity.x, 0, rigidBody.velocity.z);
			if (horizontalMovement.magnitude <= maxSpeed * runSpeedStatMultiplier)
				return;

			horizontalMovement = horizontalMovement.normalized * maxSpeed * runSpeedStatMultiplier;
			rigidBody.velocity = new Vector3(horizontalMovement.x, rigidBody.velocity.y, horizontalMovement.z);
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

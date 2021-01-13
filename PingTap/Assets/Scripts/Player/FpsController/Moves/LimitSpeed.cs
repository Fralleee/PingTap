using UnityEngine;

namespace Fralle.FpsController.Moves
{
	public class LimitSpeed : MonoBehaviour
	{
		[SerializeField] float maxSpeed = 7f;

		Rigidbody rigidBody;

		void Awake()
		{
			rigidBody = GetComponent<Rigidbody>();
		}

		public void ControlledFixedUpdate()
		{
			Limit();
		}

		void Limit()
		{
			var horizontalMovement = new Vector3(rigidBody.velocity.x, 0, rigidBody.velocity.z);
			if (horizontalMovement.magnitude <= maxSpeed)
				return;

			horizontalMovement = horizontalMovement.normalized * maxSpeed;
			rigidBody.velocity = new Vector3(horizontalMovement.x, rigidBody.velocity.y, horizontalMovement.z);
		}
	}
}

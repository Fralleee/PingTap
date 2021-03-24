using Fralle.AbilitySystem;
using Fralle.Core.Extensions;
using Fralle.FpsController;
using System.Collections;
using UnityEngine;

namespace Fralle.PingTap
{
	[CreateAssetMenu(menuName = "Abilities/Dash")]
	public class DashAbility : ActiveAbility
	{
		[SerializeField] float stopTime = 0.25f;
		[SerializeField] float dashPower = 4;

		AbilityController abilityController;
		PlayerController playerController;
		Rigidbody rigidBody;
		Transform orientation;

		public override void Setup(AbilityController abilityController)
		{
			this.abilityController = abilityController;
			playerController = abilityController.GetComponent<PlayerController>();
			rigidBody = abilityController.GetComponentInChildren<Rigidbody>();
			orientation = rigidBody.transform.Find("Orientation");
		}

		IEnumerator StopDash()
		{
			yield return new WaitForSeconds(stopTime);
			Reset();
		}

		public override void Perform()
		{
			base.Perform();

			playerController.IsLocked = true;
			Vector3 direction = playerController.cameraRig.forward;
			if (playerController.movement.magnitude > 0)
			{
				direction = orientation.TransformDirection(playerController.movement.ToVector3());
				if (playerController.movement.y > 0)
					direction += playerController.cameraRig.forward;
			}

			rigidBody.velocity = Vector3.zero;
			rigidBody.useGravity = false;
			rigidBody.AddForce(direction * dashPower, ForceMode.VelocityChange);

			// Add post process
			// Camera shake

			abilityController.StartCoroutine(StopDash());
		}

		void Reset()
		{
			playerController.IsLocked = false;
			rigidBody.velocity = Vector3.zero;
			rigidBody.useGravity = true;

			// Remove post process
		}
	}
}

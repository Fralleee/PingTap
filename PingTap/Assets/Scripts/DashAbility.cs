using Fralle.AbilitySystem;
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

		//[SerializeField] ShakeTransformEventData cameraShake = null;
		//[SerializeField] ShakeTransform cameraShakeTransform = null;
		//[SerializeField] Volume postProcess;

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


			// Add post process

			playerController.IsLocked = true;
			var direction =
				playerController.movement.y > 0 ? playerController.cameraRig.forward :
				playerController.movement.y < 0 ? -orientation.forward :
				playerController.movement.x > 0 ? orientation.right :
				playerController.movement.x < 0 ? -orientation.right :
				playerController.cameraRig.forward;

			rigidBody.velocity = Vector3.zero;
			rigidBody.useGravity = false;
			rigidBody.AddForce(direction * dashPower, ForceMode.VelocityChange);

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

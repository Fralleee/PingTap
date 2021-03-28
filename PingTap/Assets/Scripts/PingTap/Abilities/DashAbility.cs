using Fralle.AbilitySystem;
using Fralle.Core.CameraControls;
using Fralle.Core.Extensions;
using Fralle.FpsController;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

namespace Fralle.PingTap
{
	[CreateAssetMenu(menuName = "Abilities/Dash")]
	public class DashAbility : ActiveAbility
	{
		[Header("Settings")]
		[SerializeField] float stopTime = 0.25f;
		[SerializeField] float dashPower = 4;

		[Header("Effects")]
		[SerializeField] ShakeTransformEventData cameraShake;
		[SerializeField] VolumeProfile postProcess;
		[SerializeField] float addFov = 10f;

		AbilityController abilityController;
		PlayerController playerController;
		Rigidbody rigidBody;
		Transform orientation;
		ShakeTransform cameraShakeTransform;
		Volume abilityVolume;
		GameObject abilityVolumeGo;

		float defaultFov = 60f;

		public override void Setup(AbilityController abilityController)
		{
			this.abilityController = abilityController;
			playerController = abilityController.GetComponent<PlayerController>();
			rigidBody = abilityController.GetComponentInChildren<Rigidbody>();
			orientation = rigidBody.transform.Find("Orientation");

			abilityVolumeGo = new GameObject("Dash Volume");
			abilityVolumeGo.transform.SetParent(abilityController.postProcess.transform);
			abilityVolume = abilityVolumeGo.AddComponent<Volume>();
			abilityVolume.weight = 0;
			abilityVolume.profile = postProcess;

			cameraShakeTransform = playerController.camera.GetComponentInParent<ShakeTransform>();
			defaultFov = playerController.camera.fieldOfView;
		}

		IEnumerator StopDash()
		{
			float elapsedTime = 0f;
			float waitTime = stopTime;
			while (elapsedTime < waitTime)
			{
				playerController.camera.fieldOfView = Mathf.SmoothStep(defaultFov + addFov, defaultFov, elapsedTime / waitTime);
				abilityVolume.weight = Mathf.SmoothStep(1, 0, 1 - (elapsedTime / waitTime));

				elapsedTime += Time.deltaTime;
				yield return null;
			}

			playerController.camera.fieldOfView = defaultFov;

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

			cameraShakeTransform.AddShakeEvent(cameraShake);
			abilityVolume.weight = 1;
			playerController.camera.fieldOfView = defaultFov + addFov;

			abilityController.StartCoroutine(StopDash());
		}

		void Reset()
		{
			playerController.IsLocked = false;
			rigidBody.velocity = Vector3.zero;
			rigidBody.useGravity = true;

			playerController.camera.fieldOfView = defaultFov;
			abilityVolume.weight = 0;
		}

		void OnDestroy()
		{
			Destroy(abilityVolumeGo);
		}
	}
}

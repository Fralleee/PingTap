using Fralle.Core.CameraControls;
using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace Fralle.FpsController.Moves
{
	[Serializable]
	public class DashingSerialized
	{
		public event Action OnDashStart = delegate { };
		public event Action OnDashEnd = delegate { };

		public float stopTime = 0.25f;
		public Transform cameraRig = null;
		public ShakeTransformEventData cameraShake = null;
		public ShakeTransform cameraShakeTransform = null;
		public Volume postProcess;

		PlayerController controller;
		InputController input;
		Rigidbody rigidBody;
		Transform orientation;

		float cooldownTimer;
		float stopDashTimer;

		bool queueDash;

		public void Setup(PlayerController playerController, InputController inputController, Rigidbody rigidbody, Transform orientationTransform)
		{
			controller = playerController;
			input = inputController;
			rigidBody = rigidbody;
			orientation = orientationTransform;
		}

		public void Update()
		{
			if (cooldownTimer > 0)
				cooldownTimer -= Time.deltaTime;
			if (input.DashButtonDown && cooldownTimer <= 0)
				queueDash = true;
			if (controller.IsDashing)
				Stopping();
		}

		public void Dash()
		{
			if (!queueDash || controller.IsDashing)
				return;

			OnDashStart();
			queueDash = false;
			controller.IsDashing = true;
			Perform();
		}

		void Stopping()
		{
			stopDashTimer -= Time.deltaTime;
			if (postProcess)
				postProcess.weight = Mathf.SmoothStep(1, 0, 1 - (stopDashTimer / stopTime));

			Camera.main.fieldOfView = Mathf.SmoothStep(70, 60, 1 - (stopDashTimer / stopTime));

			if (stopDashTimer < 0)
				Reset();
		}

		void Perform()
		{
			cooldownTimer = controller.dashCooldown;
			postProcess.weight = 1;

			var direction =
				input.Move.y > 0 ? cameraRig.forward :
				input.Move.y < 0 ? -orientation.forward :
				input.Move.x > 0 ? orientation.right :
				input.Move.x < 0 ? -orientation.right :
				cameraRig.forward;

			rigidBody.velocity = Vector3.zero;
			rigidBody.useGravity = false;
			rigidBody.AddForce(direction * controller.dashPower, ForceMode.VelocityChange);

			if (cameraShakeTransform && cameraShake)
			{
				cameraShakeTransform.AddShakeEvent(cameraShake);
			}

			stopDashTimer = stopTime;
		}

		public void Abort()
		{
			Reset();
		}

		void Reset()
		{
			rigidBody.velocity = Vector3.zero;
			rigidBody.useGravity = true;
			controller.IsDashing = false;
			OnDashEnd();
		}

	}
}

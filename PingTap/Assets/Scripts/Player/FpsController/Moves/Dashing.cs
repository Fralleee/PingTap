using Fralle.Core.CameraControls;
using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace Fralle.FpsController.Moves
{
	public class Dashing : MonoBehaviour
	{
		public event Action OnDashStart = delegate { };
		public event Action OnDashEnd = delegate { };

		[SerializeField] float stopTime = 0.25f;
		[SerializeField] Transform cameraRig = null;
		[SerializeField] ShakeTransformEventData cameraShake = null;
		[SerializeField] ShakeTransform cameraShakeTransform = null;
		[SerializeField] Volume postProcess;

		PlayerController controller;
		Rigidbody rigidBody;
		Transform orientation;

		float cooldownTimer;
		float stopDashTimer;

		bool queueDash;

		void Awake()
		{
			controller = GetComponentInParent<PlayerController>();
			rigidBody = GetComponent<Rigidbody>();
			orientation = transform.Find("Orientation");
		}

		void Update()
		{
			if (cooldownTimer > 0)
				cooldownTimer -= Time.deltaTime;
			if (controller.Input.DashButtonDown && cooldownTimer <= 0)
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

			controller.Camera.camera.fieldOfView = Mathf.SmoothStep(70, 60, 1 - (stopDashTimer / stopTime));

			if (stopDashTimer < 0)
				Reset();
		}

		void Perform()
		{
			cooldownTimer = controller.dashCooldown;
			postProcess.weight = 1;

			var direction =
				controller.Input.Move.y > 0 ? cameraRig.forward :
				controller.Input.Move.y < 0 ? -orientation.forward :
				controller.Input.Move.x > 0 ? orientation.right :
				controller.Input.Move.x < 0 ? -orientation.right :
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

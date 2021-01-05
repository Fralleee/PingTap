using Fralle.FpsController.Moves;
using UnityEngine;

namespace Fralle.FpsController
{
	public class PlayerController : MonoBehaviour
	{
		// PlayerController should only hold togglable settings.
		// Only settings that can be configured in menu. Not permanent settings.

		[Header("Mouse look")]
		public float mouseSensitivity = 50f;
		public float mouseZoomSensitivityModifier = 0.4f;

		[Header("Movement")]
		public float forwardSpeed = 5f;
		public float strafeSpeed = 4f;
		public float stepHeight = 0.4f;

		[Header("Jump")]
		public float jumpStrength = 8f;


		[Header("Dash")]
		public float dashPower = 24f;
		public float dashCooldown = 5f;


		[HideInInspector] public bool IsGrounded;
		[HideInInspector] public bool PreviouslyGrounded;
		[HideInInspector] public bool IsMoving;
		[HideInInspector] public bool IsJumping;
		[HideInInspector] public bool IsDashing;
		[HideInInspector] public Vector3 groundContactNormal;

		[HideInInspector] public CameraController Camera;
		[HideInInspector] public InputController Input;
		[HideInInspector] public GroundController GroundController;
		[HideInInspector] public StepClimber StepClimber;
		[HideInInspector] public GravityAdjuster GravityAdjuster;
		[HideInInspector] public LimitSpeed LimitSpeed;
		[HideInInspector] public Movement Movement;
		[HideInInspector] public AirMovement AirMovement;
		[HideInInspector] public Jumping Jumping;
		[HideInInspector] public Crouching Crouching;
		[HideInInspector] public Dashing Dashing;

		void Awake()
		{
			Camera = GetComponent<CameraController>();
			Input = GetComponent<InputController>();

			GroundController = GetComponentInChildren<GroundController>();
			StepClimber = GetComponentInChildren<StepClimber>();
			GravityAdjuster = GetComponentInChildren<GravityAdjuster>();
			LimitSpeed = GetComponentInChildren<LimitSpeed>();
			Movement = GetComponentInChildren<Movement>();
			AirMovement = GetComponentInChildren<AirMovement>();
			Jumping = GetComponentInChildren<Jumping>();
			Crouching = GetComponentInChildren<Crouching>();
			Dashing = GetComponentInChildren<Dashing>();
		}

		void FixedUpdate()
		{
			if (IsDashing)
				return;

			GroundController.GroundedCheck();

			if (IsGrounded)
			{
				GroundController.SlopeControl();
				StepClimber.ClimbSteps();
				IsMoving = Movement.Move();
			}
			else
				AirMovement.Move();

			Crouching.ControlledFixedUpdate();
			Jumping.ControlledFixedUpdate();
			GravityAdjuster.ControlledFixedUpdate();
			LimitSpeed.ControlledFixedUpdate();

			if (PreviouslyGrounded && !IsJumping)
			{
				GroundController.StickToGroundHelper();
			}

			Dashing.Dash();
		}
	}
}

using Fralle.FpsController.Moves;
using UnityEngine;

namespace Fralle.FpsController
{
  public class PlayerController : MonoBehaviour
  {
    // PlayerController should only hold toggalable settings.
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
    [HideInInspector] public bool IsJumping;
    [HideInInspector] public bool IsDashing;
    [HideInInspector] public Vector3 groundContactNormal;

    GroundController groundController;
    StepClimber stepClimber;
    GravityAdjuster gravityAdjuster;
    LimitSpeed limitSpeed;
    Movement movement;
    AirMovement airMovement;
    Jumping jumping;
    Crouching crouching;
    Dashing dashing;

    void Awake()
    {
      groundController = GetComponentInChildren<GroundController>();
      stepClimber = GetComponentInChildren<StepClimber>();
      gravityAdjuster = GetComponentInChildren<GravityAdjuster>();
      limitSpeed = GetComponentInChildren<LimitSpeed>();
      movement = GetComponentInChildren<Movement>();
      airMovement = GetComponentInChildren<AirMovement>();
      jumping = GetComponentInChildren<Jumping>();
      crouching = GetComponentInChildren<Crouching>();
      dashing = GetComponentInChildren<Dashing>();
    }

    void FixedUpdate()
    {
      if (IsDashing) return;

      groundController.GroundedCheck();

      if (IsGrounded)
      {
        groundController.SlopeControl();
        stepClimber.ClimbSteps();
        movement.Move();
      }
      else airMovement.Move();

      crouching.ControlledFixedUpdate();
      jumping.ControlledFixedUpdate();
      gravityAdjuster.ControlledFixedUpdate();
      limitSpeed.ControlledFixedUpdate();

      if (PreviouslyGrounded && !IsJumping)
      {
        groundController.StickToGroundHelper();
      }

      dashing.Dash();
    }
  }
}
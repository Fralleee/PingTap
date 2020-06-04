using System;
using UnityEngine;

namespace Fralle.Movement
{
  public class PlayerMovement_old : MonoBehaviour
  {
    public event Action<Vector3, float> OnMovement = delegate { };
    public event Action<Vector3> OnJump = delegate { };
    public event Action<Vector3> OnDash = delegate { };
    public event Action<bool> OnCrouch = delegate { };
    public event Action<bool, float> OnGroundChanged = delegate { };

    public PlayerMovementState state;

    [SerializeField] Transform Ui;

    [Header("Debug")]
    [SerializeField] GameObject debugUi;
    [SerializeField] bool debugMode;

    [HideInInspector] public MovementDebugUi movementDebugUi;
    [HideInInspector] public MovementGroundCheck groundCheck;
    [HideInInspector] public MovementRun run;
    [HideInInspector] public MovementJump jump;
    [HideInInspector] public MovementDash dash;
    [HideInInspector] public MovementCrouch crouch;

    Rigidbody rigidBody;

    void Awake()
    {
      rigidBody = GetComponentInChildren<Rigidbody>();
      rigidBody.freezeRotation = true;

      groundCheck = GetComponentInChildren<MovementGroundCheck>();
      run = GetComponentInChildren<MovementRun>();
      jump = GetComponentInChildren<MovementJump>();
      dash = GetComponentInChildren<MovementDash>();
      crouch = GetComponentInChildren<MovementCrouch>();

      InitializeDebug();
    }

    public void Movement(Vector3 force, float percentageOfMaxSpeed)
    {
      OnMovement(force, percentageOfMaxSpeed);
      if (debugMode) movementDebugUi.SetVelocityText(new Vector3(rigidBody.velocity.x, 0, rigidBody.velocity.z).magnitude);
    }

    public void Jump(Vector3 force)
    {
      OnJump(force);
      state = PlayerMovementState.InAir;
    }

    public void Dash(Vector3 force)
    {
      OnDash(force);
    }

    public void Crouch(bool enter)
    {
      state = enter ? PlayerMovementState.Crouching : PlayerMovementState.Ready;
      OnCrouch(enter);
    }

    public void GroundChange(bool isGrounded, float velocityY, RaycastHit hit)
    {
      OnGroundChanged(isGrounded, velocityY);
      if (!isGrounded && state == PlayerMovementState.Ready) state = PlayerMovementState.InAir;
      if (isGrounded && state == PlayerMovementState.InAir) state = PlayerMovementState.Ready;


      if (debugMode) movementDebugUi.SetGroundedText(isGrounded, isGrounded ? hit.transform.name : "");
    }

    public void UpdateSlope(float slopeAngle)
    {
      if (debugMode) movementDebugUi.SetSlopeAngleText(slopeAngle);
    }

    void InitializeDebug()
    {
      var activeDebugUi = Instantiate(debugUi, Ui);
      movementDebugUi = activeDebugUi.GetComponentInChildren<MovementDebugUi>();
    }
  }
}
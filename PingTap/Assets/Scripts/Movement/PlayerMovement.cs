using System;
using UnityEngine;

namespace Fralle.Movement
{
  public class PlayerMovement : MonoBehaviour
  {
    public event Action<Vector3> OnMovement = delegate { };
    public event Action<Vector3> OnJump = delegate { };
    public event Action<Vector3> OnDash = delegate { };
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

    public void Movement(Vector3 force)
    {
      OnMovement(force);
      if (debugMode) movementDebugUi.SetVelocityText(new Vector3(rigidBody.velocity.x, 0, rigidBody.velocity.z).magnitude);
    }

    public void Jump(Vector3 force)
    {
      OnJump(force);
    }

    public void Dash(Vector3 force)
    {
      OnDash(force);
    }

    public void GroundChange(bool isGrounded, float velocityY, RaycastHit hit)
    {
      OnGroundChanged(isGrounded, velocityY);
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
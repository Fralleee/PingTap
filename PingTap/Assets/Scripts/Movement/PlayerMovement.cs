using Fralle.Core;
using Fralle.Player;
using NaughtyAttributes;
using UnityEngine;

namespace Fralle.Movement
{
  public class PlayerMovement : MonoBehaviour
  {
    StateMachine stateMachine;
    [ReadOnly] public string currentState;

    PlayerInputController input;
    MovementGroundCheck groundCheck;
    HeadBob headBob;

    MovementRun run;
    MovementJump jump;
    MovementGravityAdjuster gravityAdjuster;
    MovementAirControl airControl;
    MovementDash dash;
    MovementCrouch crouch;

    Rigidbody rigidBody;

    void Awake()
    {
      rigidBody = GetComponentInChildren<Rigidbody>();
      rigidBody.freezeRotation = true;

      stateMachine = new StateMachine();

      // utilities
      input = GetComponent<PlayerInputController>();
      groundCheck = GetComponentInChildren<MovementGroundCheck>();
      headBob = GetComponentInChildren<HeadBob>();

      // moves
      run = GetComponentInChildren<MovementRun>();
      jump = GetComponentInChildren<MovementJump>();
      airControl = GetComponentInChildren<MovementAirControl>();
      gravityAdjuster = GetComponentInChildren<MovementGravityAdjuster>();
      dash = GetComponentInChildren<MovementDash>();
      crouch = GetComponentInChildren<MovementCrouch>();

      groundCheck.OnGroundChanged += HandleGroundedChange;
      // modifications
      run.headBob = headBob;

      // states
      var grounded = new MovementStateGrounded(run, crouch, jump, headBob);
      var airborne = new MovementStateAirborne(airControl, gravityAdjuster, headBob, rigidBody);
      var dashing = new MovementStateDashing(dash);
      //var blocked = new MovementBlocked();

      stateMachine.AddTransition(grounded, airborne, () => !groundCheck.IsGrounded);
      stateMachine.AddTransition(grounded, dashing, () => input.dashButtonDown);

      stateMachine.AddTransition(airborne, grounded, () => groundCheck.IsGrounded);
      stateMachine.AddTransition(airborne, dashing, () => input.dashButtonDown);

      stateMachine.AddTransition(dashing, grounded, () => dashing.dashComplete && groundCheck.IsGrounded);
      stateMachine.AddTransition(dashing, airborne, () => dashing.dashComplete && !groundCheck.IsGrounded);

      DisableAllMoves();

      stateMachine.SetState(grounded);
    }

    void Update()
    {
      stateMachine.Tick();
      currentState = stateMachine.currentState.ToString();
    }

    void DisableAllMoves()
    {
      run.enabled = false;
      jump.enabled = false;
      gravityAdjuster.enabled = false;
      dash.enabled = false;
      crouch.enabled = false;
      airControl.enabled = false;
    }

    void HandleGroundedChange(bool isGrounded, float velocityY)
    {
      headBob.HandleGroundHit(isGrounded, velocityY);
    }
  }
}
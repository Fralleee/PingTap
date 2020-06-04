using Fralle.Core;
using Fralle.Movement.Moves;
using Fralle.Movement.States;
using Fralle.Player;
using NaughtyAttributes;
using UnityEngine;

namespace Fralle.Movement
{
  public class PlayerMovement : MonoBehaviour
  {
    StateMachine stateMachine;
    [ReadOnly] public string currentState;

    [Header("Debug")]
    public bool debug;
    [HideInInspector] public MovementDebugUi debugUi;
    [SerializeField] Transform UiParent;
    [SerializeField] GameObject debugUiPrefab;


    PlayerInputController input;
    MovementGroundCheck groundCheck;
    MovementHeadBob movementHeadBob;

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
      movementHeadBob = GetComponentInChildren<MovementHeadBob>();

      // moves
      run = GetComponentInChildren<MovementRun>();
      jump = GetComponentInChildren<MovementJump>();
      airControl = GetComponentInChildren<MovementAirControl>();
      gravityAdjuster = GetComponentInChildren<MovementGravityAdjuster>();
      dash = GetComponentInChildren<MovementDash>();
      crouch = GetComponentInChildren<MovementCrouch>();

      groundCheck.OnGroundChanged += HandleGroundedChange;
      // modifications
      run.movementHeadBob = movementHeadBob;

      // states
      var grounded = new MovementStateGrounded(run, crouch, jump, movementHeadBob);
      var airborne = new MovementStateAirborne(airControl, gravityAdjuster, movementHeadBob, rigidBody);
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

      InitializeDebug();
    }

    void Update()
    {
      stateMachine.Tick();
      currentState = stateMachine.currentState.ToString();

      debugUi.gameObject.SetActive(debug);
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

    void InitializeDebug()
    {
      var activeDebugUi = Instantiate(debugUiPrefab, UiParent);

      debugUi = activeDebugUi.GetComponentInChildren<MovementDebugUi>();
      debugUi.rigidBody = rigidBody;
      debugUi.gameObject.SetActive(debug);
    }

    void HandleGroundedChange(bool isGrounded, float velocityY)
    {
      movementHeadBob.HandleGroundHit(isGrounded, velocityY);
    }
  }
}
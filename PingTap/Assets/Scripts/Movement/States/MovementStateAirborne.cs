using Fralle.Movement.Moves;
using UnityEngine;

namespace Fralle.Movement.States
{
  public class MovementStateAirborne : IState
  {
    readonly MovementAirControl airControl;
    readonly MovementGravityAdjuster gravityAdjuster;
    readonly MovementHeadBob movementHeadBob;
    readonly Rigidbody rigidBody;

    public MovementStateAirborne(MovementAirControl airControl, MovementGravityAdjuster gravityAdjuster, MovementHeadBob movementHeadBob, Rigidbody rigidBody)
    {
      this.airControl = airControl;
      this.gravityAdjuster = gravityAdjuster;
      this.movementHeadBob = movementHeadBob;
      this.rigidBody = rigidBody;
    }

    public void OnEnter()
    {
      gravityAdjuster.enabled = true;
      airControl.enabled = true;

      var horizontalMovement = new Vector3(rigidBody.velocity.x, 0, rigidBody.velocity.z);
      airControl.startSpeed = horizontalMovement.magnitude;
    }

    public void Tick()
    {
      movementHeadBob.AirborneTick();
    }

    public void OnExit()
    {
      airControl.startSpeed = 0f;
      airControl.enabled = false;
      gravityAdjuster.enabled = false;
    }
  }
}
